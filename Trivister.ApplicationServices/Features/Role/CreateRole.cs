using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Exceptions;
using Trivister.ApplicationServices.Features.Role.EventHandlers;
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

public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ErrorResult<bool>>
{
    private readonly IIdentityService _identityService;
    private readonly IGlobalTSDbContext _dbContext;
    private readonly ILogger<CreateRoleCommandHandler> _logger;
    private readonly IPublisher _publisher;

    public CreateRoleCommandHandler(IGlobalTSDbContext dbContext, IIdentityService identityService, ILogger<CreateRoleCommandHandler> logger, IServiceProvider provider, IPublisher publisher)
    {
        _dbContext = dbContext;
        _identityService = identityService;
        _logger = logger;
        _publisher = publisher;
    }
    
    public async Task<ErrorResult<bool>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating role");
            var result = await _identityService.CreateRoleReturnIdAsync(ApplicationRole.Factory.Create(request.RoleName, request.Description));
            var (Id, isCreated) = result.Value;
            if (!isCreated)
                throw new BadRequestException(result.Message);
            _logger.LogInformation("Role exists");
            await _publisher.Publish(new RoleCreatedEvent(Id, request.RoleName, request.Description), cancellationToken);
            return ErrorResult.Ok<bool>(isCreated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured in CreateRoleCommandHandler");
            return ErrorResult.Fail<bool>(ex.ToString());
        }
    }
    
}