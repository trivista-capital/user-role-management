using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Common.Options;
using Trivister.Common.Model;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Features.OTP_Management;

public static class GenerateOTPController
{
    public static void GenerateOTPEndpoint(this WebApplication app)
    {
        app.MapPost("/generateOTP", async ([FromBody]OTPCommand command, IMediator mediator) =>
            {
                var otpGenerated = await mediator.Send(command);
                return Results.Ok(otpGenerated);
            }).WithName("GenerateOTP")
            .Produces<ErrorResult<bool>>(StatusCodes.Status200OK)
            .Produces<ErrorResult<bool>>(StatusCodes.Status400BadRequest)
            .WithTags("OTP")
            .RequireCors("AllowSpecificOrigins");
    }
}

public class GenerateRandomNumbers//: IGenerateRandomNumbers
{
    private static Random RNG = new Random();

    private static string Create16DigitString(int number)
    {
        var builder = new StringBuilder();
        while (builder.Length < number)
        {
            builder.Append(RNG.Next(10).ToString());
        }
        return builder.ToString();
    }

    private static HashSet<string> Results = new HashSet<string>();

    public static string CreateUniqueDigitString(int number)
    {
        var result = Create16DigitString(number);
        while (!Results.Add(result))
        {
            result = Create16DigitString(number);
        }

        return result;
    }
}

public class GenerateOTPCommandValidation : AbstractValidator<OTPCommand>
{
    public GenerateOTPCommandValidation()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty().NotEqual("string").WithMessage("Email can not be empty");
    }   
}
public sealed record OTPCommand(string Email): IRequest<ErrorResult<string>>;

public sealed record OTPCommandHandler: IRequestHandler<OTPCommand, ErrorResult<string>>
{
    private readonly IGlobalTSDbContext _dbContext;
    private readonly ILogger<OTPCommandHandler> _logger;
    private readonly OTPConfiguration _options;

    public OTPCommandHandler(IGlobalTSDbContext dbContext, ILogger<OTPCommandHandler> logger, IOptions<OTPConfiguration> options)
    {
        _dbContext = dbContext;
        _logger = logger;
        _options = options.Value;
    }
    
    public async Task<ErrorResult<string>> Handle(OTPCommand request, CancellationToken cancellationToken)
    {
        var generatedOtp = GenerateRandomNumbers.CreateUniqueDigitString(5);
        var otpStoreId = Guid.NewGuid();
        var otp = OTPStore.Factory.GenerateOto(otpStoreId, generatedOtp, request.Email, null, _options.ExpiryTIme);
        await _dbContext.OTPStore.AddAsync(otp, cancellationToken);
        var isOtpSaved = await _dbContext.SaveChangesAsync(cancellationToken);
        if (isOtpSaved > 0)
        {
            return ErrorResult.Ok<string>(generatedOtp);    
        }
        return ErrorResult.Fail<string>("Unable to generate OTP");
    }
}