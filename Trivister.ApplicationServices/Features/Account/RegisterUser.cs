using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Dto;
using Trivister.ApplicationServices.Exceptions;
using Trivister.ApplicationServices.Features.OTP_Management;
using Trivister.Common.Model;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Features.Account;

public static class RegisterUserController
{
    public static void RegisterUserEndpoint(this WebApplication app)
    {
        app.MapPost("/register", async ([FromBody] RegistrationCommand userModel, IMediator mediator) =>
            {
                var isUserRegistered = await mediator.Send(userModel);
                return Results.Ok(isUserRegistered);
            }).WithName("UserRegistration")
            .Produces<ErrorResult<string>>(StatusCodes.Status200OK)
            .Produces<ErrorResult<string>>(StatusCodes.Status400BadRequest)
            .WithTags("Authentication")
            .RequireCors("AllowSpecificOrigins");
    }
}

public class RegistrationCommandValidation : AbstractValidator<RegistrationCommand>
{
    public RegistrationCommandValidation()
    {
        RuleFor(x => x.FirstName).NotNull().NotEmpty().NotEqual("string").WithMessage("FirstName can not be empty");
        RuleFor(x => x.LastName).NotNull().NotEmpty().NotEqual("string").WithMessage("LastName can not be empty");
        RuleFor(x => x.Email).NotNull().NotEmpty().NotEqual("string").WithMessage("Email can not be empty");
        RuleFor(x => x.UserType).NotNull().NotEmpty().NotEqual("string").WithMessage("Usertype can not be empty");
        RuleFor(x => x.Password).NotNull().NotEmpty().NotEqual("string").WithMessage("Password can not be empty");
    }   
}

/// <inheritdoc />
// ReSharper disable once ClassNeverInstantiated.Global
public record RegistrationCommand(string Username, string Email, string FirstName, string MiddleName, string LastName, string Phone, string UserType, string Password, string Address) : IRequest<ErrorResult<string>>;

public class RegistrationCommandHandler: IRequestHandler<RegistrationCommand, ErrorResult<string>>
{
    private readonly IMediator _mediator;
    private readonly IIdentityService _identityService;
    private readonly ICustomerClient _customerClient;
    public RegistrationCommandHandler(IMediator mediator, 
                                      IIdentityService identityService, 
                                      ICustomerClient customerClient)
    {

        _mediator = mediator;
        _identityService = identityService;
        _customerClient = customerClient;
    }

    public async Task<ErrorResult<string>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser user = ApplicationUser.Factory.Create();
        var generatedPassword = new Random(7).ToString();
        Guid userId = Guid.NewGuid();
        var password = string.IsNullOrEmpty(request.Password) ? generatedPassword : request.Password;
        var (result, appuser) = await _identityService.CreateUserAsync(userId, request.FirstName, request.MiddleName, request.LastName,
            request.Email, request.Phone, password!, request.Address);
        if (!result.IsSuccess) throw new BadRequestException(result.Error);
        var isUseRoleCreatedResponse = await _identityService.AddUserToRole(appuser, request.UserType);
        var (roleId, isCreated) = isUseRoleCreatedResponse.Value;
        if (!isCreated)
        {
            await _identityService.DeleteUserAsync(appuser.Id.ToString());
            throw new BadRequestException(isUseRoleCreatedResponse.Error);
        }
        //Call and send OTP here
        //await _mediator.Send(new OTPCommand(request.Email), cancellationToken);
        //appuser!.Apply(new UserEvents.UserCreated() { Email = appuser.Email, Id = userId });
        await _customerClient.PublishCustomer(new AddCustomerCommand
        {
            Id = userId, FirstName = request.FirstName, MiddleName = request.MiddleName, LastName = request.LastName,
            Dob = "03/05/1977", Email = request.Email, PhoneNumber = request.Phone, Sex = "Male", RoleId = roleId,
            Address = request.Address
        });
        return ErrorResult.Ok<string>("Registered successfully");
    }
}

