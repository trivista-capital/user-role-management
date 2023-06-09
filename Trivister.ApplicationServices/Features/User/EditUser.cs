using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trivister.ApplicationServices.Abstractions;
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
public sealed record EditUserCommand(Guid Id, string Email, string FirstName, string MiddleName, string LastName, string Phone, string Address): IRequest<ErrorResult>;

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
        user.SetFirstName(request.FirstName).SetMiddleName(request.MiddleName).SetLastName(request.LastName)
            .SetEmail(request.Email).SetPhoneNumber(request.Phone).SetAddress(request.Address);
        var isUserEdited = await _identityService.UpdateUserAsync(user);
        return isUserEdited.IsSuccess ? ErrorResult.Ok() : ErrorResult.Fail<bool>("Unable to update user");
    }
}