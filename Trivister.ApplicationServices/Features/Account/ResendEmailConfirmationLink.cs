using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Exceptions;
using Trivister.Common.Model;
using Trivister.Common.Options;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Features.Account;

public static class ResendEmailConfirmationLinkController
{
    public static void ResendEmailConfirmationLinkEndpoint(this WebApplication app)
    {
        app.MapGet("/resendEmailConfirmationLink", async (string email, IMediator mediator) =>
        {
            var result = await mediator.Send(new ResendEmailConfirmationLinkCommand(email));
            return Results.Ok(result);
        }).WithName("ResendEmailConfirmationLink")
            .Produces<ErrorResult>(StatusCodes.Status200OK)
            .Produces<ErrorResult>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResult>(StatusCodes.Status500InternalServerError)
            .WithTags("Authentication")
            .RequireCors("AllowSpecificOrigins");
    }
}

public record ResendEmailConfirmationLinkCommand(string Email): IRequest<ErrorResult>;

public class ResendEmailConfirmationLinkCommandValidation : AbstractValidator<ResendEmailConfirmationLinkCommand>
{
    public ResendEmailConfirmationLinkCommandValidation()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty().NotEqual("string").WithMessage("Email can not be empty");
    }   
}
public class ResendEmailConfirmationLinkCommandHandler : IRequestHandler<ResendEmailConfirmationLinkCommand, ErrorResult>
    {
        private readonly IIdentityService _identityService;
        private readonly IPublisher _publisher;
        private readonly IConfiguration _configuration;
        private readonly MailOptions _mailOptions;
        private readonly ILogger<ResendEmailConfirmationLinkCommandHandler> _logger;

        public ResendEmailConfirmationLinkCommandHandler(IIdentityService identityService, IPublisher publisher, 
            IConfiguration configuration, ILogger<ResendEmailConfirmationLinkCommandHandler> logger,
            IOptions<MailOptions> mailOptions)
        {
            _identityService = identityService;
            _publisher = publisher;
            _configuration = configuration;
            _logger = logger;
            _mailOptions = mailOptions.Value;
        }

        public async Task<ErrorResult> Handle(ResendEmailConfirmationLinkCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Entered the ResendEmailConfirmationLinkCommandHandler");
            _logger.LogInformation("Going to identity service to get the user");
            var user = await this._identityService.GetUserByEmail(request.Email);
            _logger.LogInformation("Got back from identityservice");
            if (user.Value == null)
            {
                throw new NotFoundException("User not found");
            }
            var isUserEmailVerified = await _identityService.IsUserEmailVerified(user.Value);
            if (isUserEmailVerified.IsSuccess)
            {
                return ErrorResult.Ok("User has been verified");
            }

            _logger.LogInformation("Found user with email {@EMAIL}", request.Email);
            _logger.LogInformation("Going to identity service to generate link");
            var link = await this._identityService.GenerateEmailConfirmationLink(user.Value);
            if (string.IsNullOrEmpty(link.Value)) return ErrorResult.Fail<string>("Unable to generate link at the moment. Please try again later");
            _logger.LogInformation("Generated the link");
            var mailObject = Mail.Factory.Create(_mailOptions.From, request.Email,
                _configuration.GetSection("GeneratePasswordResetMailSubject").Value, link.Value);
            _logger.LogInformation("building the link to send to customer");
            var baseConfirmationLink = $"{_configuration.GetSection("EmailConfirmationBaseUrl").Value}";
            var confirmEmail = $"{baseConfirmationLink}?email={user.Value.Email}&token={link.Value}";
            _logger.LogInformation("About publishing to the bus");
            // await _publisher.Publish(new LeatherbackSharedLibrary.Messages.SendEmailMessage()
            // {
            //     To = new List<string>() { mailObject.To },
            //     Subject = mailObject.Subject,
            //     Bcc = new List<string>(),
            //     Cc = new List<string>(),
            //     Attachments = new List<string>(),
            //     EmailProviderName = "",
            //     EmailTemplateName = "EmailVerificationTemplate",
            //     ApplicationName = "Authentication service",
            //     ReplaceableParameters = new List<string>()
            //     {
            //         request.Email,
            //         confirmEmail
            //     }
            // });
            _logger.LogInformation("Publish to the bus");
            return ErrorResult.Ok(request.Email);
        }
    }