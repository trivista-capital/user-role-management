using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Exceptions;
using Trivister.Common.Model;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Features.Role;

public static class GetRoleController
{
    public static WebApplication GetRoleEndpoint(this WebApplication app)
    {
        app.MapGet("/getRole/{id}", async ([FromQuery]Guid id, IMediator mediator) =>
        {
            var role = await mediator.Send(new GetRoleByIdQuery(id));
            return role;
        }).WithName("GetRoleById")
            .WithTags("Role Management")
            .Produces<ErrorResult<GetRoleDto>>(StatusCodes.Status200OK)
            .Produces<ErrorResult<GetRoleDto>>(StatusCodes.Status404NotFound)
            .Produces<ErrorResult>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResult>(StatusCodes.Status500InternalServerError)
            .RequireCors("AllowSpecificOrigins");
        return app;
    }
}

public sealed class GetRoleDto
{
    public Guid RoleId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<GetPermissionsDto> Permissions { get; set; }
    public sealed class GetPermissionsDto
    {
        public int PermissionId { get; set; }
        public string Name { get; set; }
    }
    
    // public static Expression<Func<Ro, GetUsersDto>> Projection
    // {
    //     get
    //     {
    //         return x => new GetUsersDto(x.Id, x.FirstName, x.MiddleName, x.LastName, x.Address, x.RoleId);
    //     }
    // }

    public static GetRoleDto ToGetRoleDto(ApplicationRole role, IEnumerable<Permission> permissions)
    {
        return new GetRoleDto()
        {
            RoleId = role.Id,
            Name = role.Name,
            Description = role.Description,
            Permissions = permissions.Select(x=> new GetPermissionsDto()
            {
                PermissionId = x.Id,
                Name = x.Name
            }).ToList()
        };
    }
}

public sealed record GetRoleByIdQuery(Guid Id) : IRequest<ErrorResult<GetRoleDto>>;

public sealed class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, ErrorResult<GetRoleDto>>
{
    private readonly IGlobalTSDbContext _dbContext;
    private readonly IIdentityService _identityService;
    public GetRoleByIdQueryHandler(IGlobalTSDbContext dbContext, IIdentityService identityService)
    {
        _dbContext = dbContext;
        _identityService = identityService;
    }
    
    public async Task<ErrorResult<GetRoleDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.Equals(default))
            throw new BadRequestException("Role is invalid");
        var permissionsList = new List<Permission>();
        var permissionIds = await _dbContext.RolesPermissions.Where(x => x.RoleId == request.Id).ToListAsync(cancellationToken);
        if (!permissionIds.Any())
            throw new NotFoundException("No permission has been configured for this role");
        var role = await _identityService.GetRoleById(request.Id);
        if (role == null)
            throw new NotFoundException("Role does not exist");
        foreach (var permissionId in permissionIds)
        {
            var permission = new Permission
            {
                Id = permissionId.PermissionId,
                Name = await _dbContext.Permissions.Where(x => x.Id == permissionId.PermissionId).Select(x=>x.Name).FirstOrDefaultAsync(cancellationToken)
            };
            permissionsList.Add(permission);
        }
        var rolesPermissions = GetRoleDto.ToGetRoleDto(role, permissionsList);
        return ErrorResult.Ok(rolesPermissions);

    }
}
