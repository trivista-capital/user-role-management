using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Exceptions;
using Trivister.ApplicationServices.Features.Account;
using Trivister.Common.Model;

namespace Trivister.ApplicationServices.Features.OTP_Management;

public static class ValidateOTPController
{
    public static void ValidateOTPEndpoint(this WebApplication app)
    {
        app.MapPost("/validateOTP", async ([FromBody] ValidateOTPQuery query, IMediator mediator) =>
            {
                var isUserRegistered = await mediator.Send(query);
                return Results.Ok(isUserRegistered);
            }).WithName("ValidateOTP")
            .Produces<ErrorResult<bool>>(StatusCodes.Status200OK)
            .Produces<ErrorResult<bool>>(StatusCodes.Status400BadRequest)
            .WithTags("OTP")
            .RequireCors("AllowSpecificOrigins");
    }
}

public class ValidateOTPQueryValidation : AbstractValidator<ValidateOTPQuery>
{
    public ValidateOTPQueryValidation()
    {
        RuleFor(x => x.email).NotNull().NotEmpty().NotEqual("string").WithMessage("Email can not be empty");
        RuleFor(x => x.otp).NotNull().NotEmpty().NotEqual("string").WithMessage("OTP can not be empty");
    }   
}

public sealed record ValidateOTPQuery(string email, string otp): IRequest<ErrorResult<bool>>;

public sealed class ValidateOTPQueryHandler : IRequestHandler<ValidateOTPQuery, ErrorResult<bool>>
{
    private readonly IGlobalTSDbContext _dbContext;
    private readonly ILogger<ValidateOTPQueryHandler> _logger;
    
    public ValidateOTPQueryHandler(IGlobalTSDbContext dbContext, ILogger<ValidateOTPQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<ErrorResult<bool>> Handle(ValidateOTPQuery request, CancellationToken cancellationToken)
    {
        var otpResult = await _dbContext.OTPStore.Where(x => x.Email == request.email && x.OTP == request.otp)
                              .FirstOrDefaultAsync(cancellationToken);
        
        if(otpResult == null) throw new BadRequestException("OTP is not valid");
        
        if (DateTime.UtcNow.Date == otpResult.ExpiryDate.Date)
        {
            if (otpResult != null && DateTime.UtcNow.TimeOfDay < otpResult.ExpiryDate.TimeOfDay)
            {
                if (otpResult.IsExpired || otpResult.IsUsed)
                {
                    return ErrorResult.Fail<bool>("OTP is expired");
                }
                otpResult.IsExpired = true;
                otpResult.IsUsed = true;
                otpResult.LastModified = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync(cancellationToken);
                return ErrorResult.Ok<bool>(true);
            }   
        }
        return ErrorResult.Fail<bool>("OTP is expired");
    }
}