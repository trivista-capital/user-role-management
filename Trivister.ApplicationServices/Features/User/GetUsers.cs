using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Common.Model;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Features.User;

public static class GetUsersController
{
    public static void GetUsersEndpoint(this WebApplication app)
    {
        app.MapGet("getUsers", async (IMediator mediator) =>
        {
            var users = await mediator.Send(new GetUsersQuery());
            return Results.Ok(users);
        }).WithName("GetUsers")
            .Produces<ErrorResult<List<GetUsersDto>>>(StatusCodes.Status200OK)
            .Produces<ErrorResult<List<GetUsersDto>>>(StatusCodes.Status404NotFound)
            .WithTags("User Management")
            .RequireCors("AllowSpecificOrigins");
    }
}

public record GetUsersDto(Guid Id, string? FirstName, string? MiddleName, string? LastName, string? Address, int RoleId, string Email, string Gender)
{
    public string? RoleName { get; set; }
    public static Expression<Func<ApplicationUser, GetUsersDto>> Projection
    {
        get
        {
            return x => new GetUsersDto(x.Id, x.FirstName, x.MiddleName, x.LastName, x.Address, x.RoleId, x.Email, "");
        }
    }

    // public static explicit operator ToGetUsersDto(ApplicationUser user)
    // {
    //     return new GetUsersDto(user.Id, user.FirstName, user.MiddleName, user.LastName, user.Address, user.RoleId);
    // }
}

public sealed record GetUsersQuery() : IRequest<ErrorResult<List<GetUsersDto>>>;

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ErrorResult<List<GetUsersDto>>>
{
    private readonly IIdentityService _identityService;
    public GetUsersQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<ErrorResult<List<GetUsersDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var userIds = await _identityService.GetUserNotInRoleAsync("Customer");
        var usersFromDb = await _identityService.GetUserByIdAsync(userIds);
        var users = usersFromDb.Select(x => new GetUsersDto(x.Id, x.FirstName, x.MiddleName, x.LastName, x.Address,
                                    x.RoleId, x.Email, "")).ToList();
        for (int i = 0; i< users.Count; i ++)
        {
            var role = await _identityService.GetUsersRole(users[i].Id);
            users[i].RoleName = role?.Name;
        }
        return ErrorResult.Ok(users);
    }
}