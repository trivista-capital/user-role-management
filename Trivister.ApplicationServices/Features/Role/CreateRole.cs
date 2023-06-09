using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Exceptions;
using Trivister.Common.Model;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Features.Role;

public static class CreateRoleController
{
    public static void CreateRoleEndpoint(this WebApplication app)
    {
        app.MapPost("/createRole",
                //[Authorize("UserCanAddRole")]
                async ([FromBody] CreateRoleCommand role, IMediator mediator) =>
                {
                    var result = await mediator.Send(role);
                    return Results.Ok(result);
                }).WithName("CreateRole")
            .WithTags("Role Management")
            .Produces<ErrorResult<bool>>(StatusCodes.Status200OK)
            .Produces<ErrorResult<bool>>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResult<bool>>(StatusCodes.Status500InternalServerError)
            .RequireCors("AllowSpecificOrigins");
        //.RequireAuthorization();
    }
}

public record CreateRoleCommand(string RoleName, string Description): IRequest<ErrorResult<bool>>;

public class CreateRoleCommandValidation : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidation()
    {
        RuleFor(x => x.RoleName).NotEqual("string").NotNull().WithMessage("RoleName must not empty");
        RuleFor(x => x.Description).NotEqual("string").NotNull().WithMessage("Description must not empty");
    }   
}

public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ErrorResult< bool>>
{
    private readonly IIdentityService _identityService;
    private readonly IGlobalTSDbContext _dbContext;
    private readonly ICustomerClient _customerClient;
    
    public CreateRoleCommandHandler(IGlobalTSDbContext dbContext, IIdentityService identityService, ICustomerClient customerClient)
    {
        _dbContext = dbContext;
        _identityService = identityService;
        _customerClient = customerClient;
    }
    
    public async Task<ErrorResult<bool>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var isRoleCreated = await _identityService.CreateRoleAsync(ApplicationRole.Factory.Create(request.RoleName, request.Description));
        if (!isRoleCreated.IsSuccess)
            throw new BadRequestException(isRoleCreated.Message);
        await _customerClient.PublishRole(new Dto.CreateRoleCommand(request.RoleName, request.Description));
        return ErrorResult.Ok<bool>(isRoleCreated.IsSuccess);
    }
}