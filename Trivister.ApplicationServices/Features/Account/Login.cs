using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Common.Model;

namespace Trivister.ApplicationServices.Features.Account;

public static class LoginController
{
    public static void LoginEndpoint(this WebApplication app)
    {
        app.MapPost("/login", async ([FromBody] LoginCommand loginModel, IMediator mediator) =>
            {
                var isUserLoggedId = await mediator.Send(loginModel);
                return Results.Ok(isUserLoggedId);
            }).WithName("Login")
            .Produces<ErrorResult<(Guid, ErrorResult)>>(StatusCodes.Status200OK)
            .Produces<ErrorResult<(Guid, ErrorResult)>>(StatusCodes.Status400BadRequest)
            .WithTags("Authentication")
            .RequireCors("AllowSpecificOrigins");
    }
}
public record LoginCommand(string Username, string Password): IRequest<(Guid, ErrorResult)>;

public class LoginCommandValidation : AbstractValidator<LoginCommand>
{
    public LoginCommandValidation()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty().NotEqual("string").WithMessage("Username can not be empty");
        RuleFor(x => x.Password).NotNull().NotEmpty().NotEqual("string").WithMessage("Password can not be empty");
    }   
}

public class LoginCommandHandler: IRequestHandler<LoginCommand, (Guid, ErrorResult)>
{
    private readonly IGlobalTSDbContext _dbContext;
    private readonly IIdentityService _identityService;
    private readonly IConfiguration _configuration;
    public LoginCommandHandler(IGlobalTSDbContext dbContext, IConfiguration configuration, IIdentityService identityService)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _identityService = identityService;
    }

    public async Task<(Guid, ErrorResult)> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _identityService.ValidateApplicationUser(request.Username, request.Password);
            if(!result.IsSuccess) return (Guid.Empty, ErrorResult.Fail<string>(result.Message, result.Message, "400"));
            return (result.Value.Id, ErrorResult.Ok<string>("", "", "200"));
        }
        catch (Exception ex)
        {
            return (Guid.Empty, ErrorResult.Fail<string>("An error occured",  "500"));
        }
    }
}