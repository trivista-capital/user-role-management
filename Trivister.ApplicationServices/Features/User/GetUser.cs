using System.Linq.Expressions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Common.Model;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Features.User;

public static class GetUserController
{
    public static void GetUserEndpoint(this WebApplication app)
    {
        app.MapGet("/getUser", 
                //[Authorize(Roles = "SuperAdmin")]
                async ([FromQuery]Guid id, IMediator mediator) =>
            {
                var users = await mediator.Send(new GetUserQuery(id));
                return Results.Ok(users);
            }).WithName("GetUser")
            .Produces<ErrorResult<List<GetUserDto>>>(StatusCodes.Status200OK)
            .Produces<ErrorResult<List<GetUserDto>>>(StatusCodes.Status404NotFound)
            .WithTags("User Management")
            .RequireCors("AllowSpecificOrigins");
    }
}

public record GetUserDto(Guid Id, string? FirstName, string? MiddleName, string? LastName, string? Address, int RoleId, string Email)
{
    public string? RoleName { get; set; }
}

public sealed record GetUserQuery(Guid Id) : IRequest<ErrorResult<GetUserDto>>;

public class GetUserQueryValidation : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidation()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Email can not be empty");
    }   
}

public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, ErrorResult<GetUserDto>>
{
    private readonly IIdentityService _identityService;
    
    public GetUserQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<ErrorResult<GetUserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userFromDb = await _identityService.GetUserById(request.Id);
        var role = await _identityService.GetUsersRole(userFromDb.Id);
        var user = new GetUserDto(userFromDb.Id, userFromDb.FirstName, userFromDb.MiddleName,
                                    userFromDb.LastName, userFromDb.Address, userFromDb.RoleId, userFromDb.Email)
        {
            RoleName = role.Name
        };
        return ErrorResult.Ok(user);
    }
}