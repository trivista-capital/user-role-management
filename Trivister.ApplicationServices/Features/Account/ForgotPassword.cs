using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Common.Model;

namespace Trivister.ApplicationServices.Features.Account;

public static class ForgotPasswordController
{
    public static void ForgotPasswordEndpoint(this WebApplication app)
    {
        app.MapPost("/forgotPassword", async ([FromBody] ForgotPasswordCommand forgotPasswordModel, IMediator mediator) =>
            {
                var result = await mediator.Send(forgotPasswordModel);
                return Results.Ok(result);
            }).WithName("ForgotPassword")
            .Produces<ErrorResult>(StatusCodes.Status200OK)
            .Produces<ErrorResult>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResult>(StatusCodes.Status500InternalServerError)
            .WithTags("Authentication");
    }
}

public record ForgotPasswordCommand(string Token, string Email, string NewPassword, string TokenExpiryTime) : IRequest<ErrorResult>;

public class ForgotPasswordCommandValidation: AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidation()
    {
        RuleFor(x => x.Token).NotNull().NotEmpty().NotEqual("string").WithMessage("Token can not be null or empty");
        RuleFor(x => x.Email).NotNull().NotEmpty().NotEqual("string").WithMessage("Email can not be null or empty");
        RuleFor(x => x.NewPassword).NotNull().NotEmpty().NotEqual("string").WithMessage("NewPassword can not be null or empty");
        RuleFor(x => x.TokenExpiryTime).NotNull().NotEmpty().NotEqual("string").WithMessage("TokenExpiryTime can not be null or empty");
    }
}
public class ForgotPasswordCommandHandler: IRequestHandler<ForgotPasswordCommand, ErrorResult>
{
        private readonly IIdentityService _identityService;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;
        public ForgotPasswordCommandHandler(IIdentityService identityService, ILogger<ForgotPasswordCommandHandler> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }
        
        public async Task<ErrorResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByEmail(request.Email);
                if (user.Value.Id == default) return ErrorResult.Ok("User not found");

                if (string.IsNullOrEmpty(request.TokenExpiryTime))
                {
                    var expiryTime = Convert.ToDateTime(request.TokenExpiryTime);
                    if (expiryTime.CompareTo(DateTime.UtcNow) < 0) return ErrorResult.Fail<string>("Account activation link has expired");
                    var activationResult = await _identityService.AccountActivationAsync(user.Value, ConstantKeys.DefaultPassword, request.NewPassword);
                    if (activationResult.IsSuccess)
                    {
                        user.Value.EmailConfirmed = true;
                        _ = await _identityService.UpdateUserAsync(user.Value);
                    }
                    return !activationResult.IsSuccess ? ErrorResult.Fail<string>(activationResult.Error) : ErrorResult.Ok("Password reset successful");
                }
                var result = await _identityService.ResetPassword(user.Value, request.Token, request.NewPassword);
                if (result.IsSuccess)
                {
                    user.Value.EmailConfirmed = true;
                    _ = await _identityService.UpdateUserAsync(user.Value);
                }
                return !result.IsSuccess ? ErrorResult.Fail<string>(result.Error) : ErrorResult.Ok("Password reset successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured when changing password");
                return ErrorResult.Fail<string>("An error occured when activation forgot password", "500");
            }
        }
}