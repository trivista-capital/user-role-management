using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Exceptions;
using Trivister.Common.Model;

namespace Trivister.ApplicationServices.Features.User;

public static class EditUserController
{
    public static WebApplication EditUserEndpoint(this WebApplication app)
    {
        app.MapPost("/editUser", async([FromBody]EditUserCommand body, IMediator mediator) =>
            {
                var result = await mediator.Send(body);
                return Results.Ok(result);
            }).WithName("EditUser")
            .Produces<ErrorResult>(StatusCodes.Status200OK)
            .Produces<ErrorResult>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResult>(StatusCodes.Status500InternalServerError)
            .WithTags("User Management")
            .RequireCors("AllowSpecificOrigins");
        return app;
    }
}
public sealed record EditUserCommand(Guid Id, string Email, string FirstName, string UserType, string LastName, string password): IRequest<ErrorResult>;

public sealed class EditUserCommandHandler : IRequestHandler<EditUserCommand, ErrorResult>
{
    private readonly IIdentityService _identityService;
    public EditUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ErrorResult> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserById(request.Id);
        if(user == null)
            return ErrorResult.Fail("User is invalid");
        var isUserRemoved = await _identityService.RemoveUserFromRole(user.Id);
        if (isUserRemoved.Value.Item1 == false) throw new BadRequestException(isUserRemoved.Error);
        user.SetFirstName(request.FirstName).SetLastName(request.LastName).SetEmail(request.Email);
        var isUseRoleCreatedResponse = await _identityService.AddUserToRole(user, request.UserType);
        if (isUseRoleCreatedResponse.Value.Item2)
        {
            var isPasswordChanged = await _identityService.ChangePasswordAsync(user, request.password);
            if (isPasswordChanged.IsSuccess)
            {
                var isUserEdited = await _identityService.UpdateUserAsync(user);
                return isUserEdited.IsSuccess ? ErrorResult.Ok() : ErrorResult.Fail<bool>("Unable to update user");
            }   
        }

        return ErrorResult.Fail("Unable to edit user");
    }
}