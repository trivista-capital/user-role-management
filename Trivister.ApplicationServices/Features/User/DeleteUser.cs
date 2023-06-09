using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Common.Model;

namespace Trivister.ApplicationServices.Features.User;

public static class DeleteUserController
{
    public static WebApplication DeleteUserEndpoint(this WebApplication app)
    {
        app.MapPost("/deleteUser", async([FromBody]DeleteUserCommand body, IMediator mediator) =>
            {
                var result = await mediator.Send(body);
                return Results.Ok(result);
            }).WithName("DeleteUser")
            .Produces<ErrorResult>(StatusCodes.Status200OK)
            .Produces<ErrorResult>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResult>(StatusCodes.Status500InternalServerError)
            .WithTags("User Management")
            .RequireCors("AllowSpecificOrigins");
        return app;
    }
}
public sealed record DeleteUserCommand(Guid Id): IRequest<ErrorResult>;

public class DeleteUserCommandValidation : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidation()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Email can not be empty");
    }   
}

public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ErrorResult>
{
    private readonly IIdentityService _identityService;
    public DeleteUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ErrorResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var isUserDeleted = await _identityService.DeleteUserAsync(request.Id.ToString());
        return isUserDeleted.IsSuccess ? ErrorResult.Ok() : ErrorResult.Fail<bool>("Unable to delete user");
    }
}