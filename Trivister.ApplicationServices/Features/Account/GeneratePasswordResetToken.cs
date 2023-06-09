using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Common.Model;
using Trivister.Common.Options;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Features.Account;

public static class GeneratePasswordResetTokenController
{
    public static void GeneratePasswordResetTokenEndpoint(this WebApplication app)
    {
        app.MapPost("/generatePasswordResetToken",
            async ([FromBody] GeneratePasswordResetTokenCommand body, IMediator mediator) =>
            {
                var result = await mediator.Send(body);
                return Results.Ok(result);
            }).WithName("GeneratePasswordResetToken")
            .WithTags("Authentication")
            .RequireCors("AllowSpecificOrigins");
    }
}

public record GeneratePasswordResetTokenCommand(string Email) : IRequest<ErrorResult>;

public class GeneratePasswordResetTokenCommandValidation : AbstractValidator<GeneratePasswordResetTokenCommand>
{
    public GeneratePasswordResetTokenCommandValidation()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty().NotEqual("string").WithMessage("Email can not be empty");
    }   
}

public class GeneratePasswordResetTokenCommandHandler : IRequestHandler<GeneratePasswordResetTokenCommand, ErrorResult>
{
    private readonly IIdentityService _identityService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GeneratePasswordResetTokenCommandHandler> _logger;
    private readonly MailOptions _mailOptions;
    public GeneratePasswordResetTokenCommandHandler(IIdentityService identityService, IPublisher publisher, IConfiguration configuration, 
        ILogger<GeneratePasswordResetTokenCommandHandler> logger, IOptions<MailOptions> mailOptions)
    {
        _identityService = identityService;
        _configuration = configuration;
        _logger = logger;
        _mailOptions = mailOptions.Value;
    }
    
    public async Task<ErrorResult> Handle(GeneratePasswordResetTokenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Entered the GeneratePasswordResetTokenCommandHandler");
            var user = await this._identityService.GetUserByEmail(request.Email);
            if (user?.Value == null)
            {
              ErrorResult.Fail<string>("User not found");
            }
            _logger.LogInformation("About generating the link");
            var link = await this._identityService.GeneratePasswordResetTokenLink(user?.Value!);
            _logger.LogInformation("Generated the link");
            
            if (string.IsNullOrEmpty(link?.Value)) return ErrorResult.Fail<string>("Unable to generate link at the moment. Please try again later");
            var mailObject = Mail.Factory.Create(_mailOptions.From, request.Email,
                    _configuration.GetSection("GeneratePasswordResetMailSubject").Value, link.Value);
            
            var baseConfirmationLink = _configuration.GetSection("EmailResetBaseUrl").Value;
            var confirmationLink = $"{baseConfirmationLink}?email={request.Email}&token={link.Value}";
            _logger.LogInformation("About publishing the SendEmailMessage event");
            
            // await _publisher.Publish(new SendEmailMessage()
            // {
            //     To = new List<string>() { mailObject.To },
            //     Subject = mailObject.Subject,
            //     Bcc = new List<string>(),
            //     Cc = new List<string>(),
            //     Attachments = new List<string>(),
            //     EmailProviderName = "",
            //     EmailTemplateName = "ForgotPasswordTemplate",
            //     ApplicationName = "Authentication service",
            //     ReplaceableParameters = new List<string>()
            //     {
            //         request.Email,
            //         confirmationLink
            //     }
            // });
            _logger.LogInformation("Published the SendEmailMessage event");
            return ErrorResult.Ok();
    }
}