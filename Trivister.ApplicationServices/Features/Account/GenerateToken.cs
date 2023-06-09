using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Common.Model;

namespace Trivister.ApplicationServices.Features.Account;

public static class GenerateTokenController
{
    public static void GenerateTokenEndpoint(this WebApplication app)
    {
        app.MapPost("/generateToken", async ([FromBody] GenerateTokenCommand model, IMediator mediator) =>
            {
                var isUserLoggedId = await mediator.Send(model);
                return Results.Ok(isUserLoggedId);
            }).WithName("GenerateToken")
            .Produces<ErrorResult<(Guid, ErrorResult)>>(StatusCodes.Status200OK)
            .Produces<ErrorResult<(Guid, ErrorResult)>>(StatusCodes.Status400BadRequest)
            .WithTags("Authentication")
            .RequireCors("AllowSpecificOrigins");
    }
}

public sealed record GenerateTokenCommand(string Username, string Password): IRequest<ErrorResult<TokenResult>>;

public class GenerateTokenCommandValidation : AbstractValidator<GenerateTokenCommand>
{
    public GenerateTokenCommandValidation()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty().NotEqual("string").WithMessage("Username can not be empty");
        RuleFor(x => x.Password).NotNull().NotEmpty().NotEqual("string").WithMessage("Password can not be empty");
    }   
}

public sealed class GenerateTokenCommandHandler: IRequestHandler<GenerateTokenCommand, ErrorResult<TokenResult>>
{
    private readonly IGetTokenClient _getTokenClient;
    private readonly IConfiguration _configuration;
    public GenerateTokenCommandHandler(IConfiguration configuration, IGetTokenClient getTokenClient)
    {
        _configuration = configuration;
        _getTokenClient = getTokenClient;
    }

    public async Task<ErrorResult<TokenResult>> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getTokenClient.GetToken(request.Username, request.Password);
            if (result != null)
                return ErrorResult.Ok(result);
            return ErrorResult.Fail<TokenResult>(result.Error);
        }
        catch (Exception ex)
        {
            return ErrorResult.Fail<TokenResult>("n error occured");
        }
    }
}