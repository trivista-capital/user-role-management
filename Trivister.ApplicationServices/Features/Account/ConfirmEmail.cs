using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Common.Model;

namespace Trivister.ApplicationServices.Features.Account;

public static class ConfirmEmailController
{
    public static void ConfirmEmailEndpoint(this WebApplication app)
    {
        app.MapGet("/confirmEmail", async([FromQuery]string email, [FromQuery]string token, IMediator mediator) =>
        {
            var result = await mediator.Send(new ConfirmEmailCommand(email, token));
        })
            .WithName("Verify-Email")
            .WithTags("Authentication")
            .RequireCors("AllowSpecificOrigins");
    }
}

public record ConfirmEmailCommand(string Email, string Token) : IRequest<ErrorResult>;

public class ConfirmEmailCommandValidation : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidation()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty().NotEqual("string").WithMessage("Email can not be empty");
        RuleFor(x => x.Token).NotNull().NotEmpty().NotEqual("string").WithMessage("Token can not be empty");
    }   
}
public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ErrorResult>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<ConfirmEmailCommandHandler> _logger;
        
    public ConfirmEmailCommandHandler(IIdentityService identityService,
        ILogger<ConfirmEmailCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<ErrorResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Entered the ConfirmEmailCommandHandler handle method");
        _logger.LogInformation("About going to verify email");
        var result = await _identityService.ConfirmEmail(request.Email, request.Token);
        return !result.IsSuccess ? ErrorResult.Fail("Unable to confirm email") : result;
    }
}