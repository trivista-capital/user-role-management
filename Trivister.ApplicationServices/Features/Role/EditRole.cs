using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Exceptions;
using Trivister.ApplicationServices.Features.User;
using Trivister.Common.Model;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Features.Role;

public static class EditRoleController
{
    public static WebApplication EditRoleEndpoint(this WebApplication app)
    {
        app.MapPost("/editRole/{id}", async ([FromBody] EditRoleCommand body, IMediator mediator) =>
            {
                var result = await mediator.Send(body);
                return Results.Ok(result);
            }).WithName("EditRole")
            .WithTags("Role Management")
            .Produces<ErrorResult>(StatusCodes.Status200OK)
            .Produces<ErrorResult>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResult>(StatusCodes.Status404NotFound)
            .Produces<ErrorResult>(StatusCodes.Status500InternalServerError)
            .RequireCors("AllowSpecificOrigins");
        return app;
    }
}
public sealed record EditRoleCommand(Guid Id, string Name, string Description): IRequest<ErrorResult>;

public class EditRoleCommandValidation : AbstractValidator<EditRoleCommand>
{
    public EditRoleCommandValidation()
    {
        RuleFor(x => x.Name).NotNull().WithMessage("RoleName can not be empty");
        RuleFor(x => x.Id).Must(x=>x != default).NotNull().WithMessage("Id is invalid");
    }   
}

public sealed class EditRoleCommandHandler : IRequestHandler<EditRoleCommand, ErrorResult>
{
    private readonly IIdentityService _identityService;
    public EditRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ErrorResult> Handle(EditRoleCommand request, CancellationToken cancellationToken)
    {
        var isRoleEdited = await _identityService.AddOrEditRole(request.Id, request.Name, request.Description);
        return isRoleEdited.IsSuccess ? ErrorResult.Ok() : ErrorResult.Fail<bool>("Unable to update role");
    }
}