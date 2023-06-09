using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Exceptions;
using Trivister.Common.Model;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Features.Role;

public static class GetPermissionController
{
    public static WebApplication GetPermissionEndpoint(this WebApplication app)
    {
        app.MapGet("/permissions", async(IMediator mediator) =>
        {
            var result = await mediator.Send(new GetPermissionsQuery());
            return Results.Ok(result);

        }).WithName("Permissions")
            .WithTags("Role Management")
            .Produces<ErrorResult<List<GetPermissionOnlyDto>>>(StatusCodes.Status200OK)
            .Produces<ErrorResult<List<GetPermissionOnlyDto>>>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResult>(StatusCodes.Status404NotFound)
            .Produces<ErrorResult>(StatusCodes.Status500InternalServerError)
            .RequireCors("AllowSpecificOrigins");
        return app;
    }
}

public class GetPermissionOnlyDto
{
    public int Id { get; set; }
    public string Name{get; set; }

    public static List<GetPermissionOnlyDto> ToGetPermissionsDto(List<Permission> permissions)
    {
        return permissions.Select(x => new GetPermissionOnlyDto()
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }
}

public record GetPermissionsQuery : IRequest<ErrorResult<List<GetPermissionOnlyDto>>>;

public sealed class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, ErrorResult<List<GetPermissionOnlyDto>>>
{
    private readonly IGlobalTSDbContext _dbContext;
    public GetPermissionsQueryHandler(IGlobalTSDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ErrorResult<List<GetPermissionOnlyDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissionsFromDb = await _dbContext.Permissions.ToListAsync(cancellationToken);
        if (!permissionsFromDb.Any())
            throw new NotFoundException("No permissions has been configured yet");
        var permissionDto = GetPermissionOnlyDto.ToGetPermissionsDto(permissionsFromDb);
        return ErrorResult.Ok(permissionDto);
    }
}