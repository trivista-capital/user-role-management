using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Common.Model;

namespace Trivister.ApplicationServices.Features.Account;

public static class ChangePasswordController
{
    public static WebApplication ChangePasswordEndpoint(this WebApplication app)
    {
        app.MapPost("/changePassword", 
                ChangePassword)
            .WithName("ChangePassword")
            .Produces<ErrorResult>(StatusCodes.Status200OK)
            .Produces<ErrorResult>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResult>(StatusCodes.Status500InternalServerError)
            .WithTags("Authentication")
            .RequireCors("AllowSpecificOrigins");

        return app;
    }
    
    /// <summary>
    /// Says howdy to the name
    /// </summary>
    /// <remarks>Awesomeness!</remarks>
    /// <param name="changePasswordModel" example="Khalid">name</param>
    /// <param name="mediator">name</param>
    /// <response code="200">Howdy</response>
    private static async Task<IResult> ChangePassword([FromBody]ChangePasswordCommand changePasswordModel, IMediator mediator)
    {
        var result = await mediator.Send(changePasswordModel);
        return Results.Ok(result);
    }
}

public record ChangePasswordCommand(string Username, string OldPassword, string NewPassword) : IRequest<ErrorResult>;

public class ChangePasswordCommandValidation: AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidation()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty().NotEqual("string").WithMessage("Username can not be empty");
        RuleFor(x => x.NewPassword).NotNull().NotEmpty().NotEqual("string").WithMessage("New password can not be empty");
        RuleFor(x => x.OldPassword).NotNull().NotEmpty().NotEqual("string").WithMessage("Old password can not be empty");
    }
}

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ErrorResult>
{
        private readonly IIdentityService _identityService;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;
        public ChangePasswordCommandHandler(IIdentityService identityService, ILogger<ChangePasswordCommandHandler> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }
        
        public async Task<ErrorResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await this._identityService.ValidateApplicationUser(request.Username, request.OldPassword);
                if (!user.IsSuccess) return ErrorResult.Fail<bool>("Username or old password incorrect");
                var isUserDisabled = await _identityService.IsUserDisabled(request.Username);
                if (!isUserDisabled.Value) return ErrorResult.Fail<bool>(isUserDisabled.Error);
                var isUserLockedOut = await _identityService.IsUserLocked(request.Username);
                if (!isUserLockedOut.Value) return ErrorResult.Fail<bool>(isUserLockedOut.Error);
                var result = await _identityService.ChangePasswordAsync(user.Value, request.NewPassword);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured when changing password");
                throw new Exception("An error occured when changing password");
            }
        }
}