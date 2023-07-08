using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Exceptions;
using Trivister.Common.Model;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Features.Role;

public static class GetRolesController
{
    public static WebApplication GetRolesEndpoint(this WebApplication app)
    {
        app.MapGet("/getRoles", async (IMediator mediator) =>
            {
                var roles = await mediator.Send(new GetRolesByIdQuery());
                return Results.Ok(roles);
            }).WithName("GetRoles")
            .WithTags("Role Management")
            .Produces<ErrorResult<List<GetRolesDto>>>(StatusCodes.Status200OK)
            .Produces<ErrorResult>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResult>(StatusCodes.Status404NotFound)
            .Produces<ErrorResult>(StatusCodes.Status500InternalServerError)
            .RequireCors("AllowSpecificOrigins");
        return app;
    }
}

public sealed class GetRolesDto
{
    public Guid RoleId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<GetRolesPermissionsDto> Permissions { get; set; }
    public sealed class GetRolesPermissionsDto
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

    public static GetRolesDto ToGetRolesDto(ApplicationRole role, IEnumerable<Permission> permissions)
    {
        return new GetRolesDto()
        {
            RoleId = role.Id,
            Name = role.Name,
            Description = role.Description,
            Permissions = permissions.Select(x=> new GetRolesPermissionsDto()
            {
                PermissionId = x.Id,
                Name = x.Name
            }).ToList()
        };
    }
    
    public static List<GetRolesPermissionsDto> ToGetRolePermissionsDto(IEnumerable<Permission> permissionsList)
    {
        var permissions = permissionsList.Select(x => new GetRolesPermissionsDto()
        {
            PermissionId = x.Id,
            Name = x.Name
        }).ToList();
        return permissions;
    }
}
public sealed record GetRolesByIdQuery: IRequest<ErrorResult<List<GetRolesDto>>>;

public sealed class GetRolesByIdQueryHandler : IRequestHandler<GetRolesByIdQuery, ErrorResult<List<GetRolesDto>>>
{
    private readonly IGlobalTSDbContext _dbContext;
    private readonly IIdentityService _identityService;
    public GetRolesByIdQueryHandler(IGlobalTSDbContext dbContext, IIdentityService identityService)
    {
        _dbContext = dbContext;
        _identityService = identityService;
    }
    
    public async Task<ErrorResult<List<GetRolesDto>>> Handle(GetRolesByIdQuery request, CancellationToken cancellationToken)
    {
        var getRolesDtoList = new List<GetRolesDto>();
        var queryAbleRoles = _identityService.GetAllRoles().Where(x=>x.Name != "Customer" && x.Name != "Staff");
        var applicationRoles = queryAbleRoles.ToList();
        if (!applicationRoles.Any())
            throw new NotFoundException("No role exist in the application");
        foreach (var role in applicationRoles)
        {
            var getRolesDto = new GetRolesDto
            {
                Name = role.Name,
                RoleId = role.Id,
                Description = role.Description
            };

            var permissionIds = await _dbContext.RolesPermissions.Where(x => x.RoleId == role.Id).ToListAsync(cancellationToken);
            if (permissionIds.Any())
            {
                var permissionsList = new List<Permission>();
                foreach (var permissionId in permissionIds)
                {
                    var permission = new Permission
                    {
                        Id = permissionId.PermissionId,
                        Name = await _dbContext.Permissions.Where(x => x.Id == permissionId.PermissionId).Select(x=>x.Name).FirstOrDefaultAsync(cancellationToken)
                    };
                    permissionsList.Add(permission);
                }

                getRolesDto.Permissions = permissionsList.AsEnumerable().Select(x =>
                    new GetRolesDto.GetRolesPermissionsDto()
                    {
                        PermissionId = x.Id,
                        Name = x.Name
                    }).ToList(); 
                getRolesDtoList.Add(getRolesDto);
                //permissionsList.Clear();
            }
            else
            {
                getRolesDtoList.Add(getRolesDto);   
            }
        }
        return ErrorResult.Ok(getRolesDtoList);

    }
}