using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Features.Account.EventHandlers;
using Trivister.Common.Model;

namespace Trivister.ApplicationServices.Features.Account;

public static class WelcomeEmailTriggerController
{
    public static void WelcomeTriggerEndpoint(this WebApplication app)
    {
        app.MapGet("/welcomeEmailTrigger/{email}", async (IMediator mediator, string email) =>
            {
                var isEmailTriggered = await mediator.Send(new WelcomeEmailQuery(email));
                return Results.Ok(isEmailTriggered);
            }).WithName("Welcome Email Trigger")
            .Produces<ErrorResult>(StatusCodes.Status200OK)
            .WithTags("Authentication")
            .RequireCors("AllowSpecificOrigins");
    }
}

public record WelcomeEmailQuery(string Email) : IRequest<ErrorResult>;

public class WelcomeEmailQueryHandler: IRequestHandler<WelcomeEmailQuery, ErrorResult>
{
    private readonly IIdentityService _identityService;
    private readonly IPublisher _publisher;
    
    public WelcomeEmailQueryHandler(IIdentityService identityService, IPublisher publisher)
    {
        _identityService = identityService;
        _publisher = publisher;
    }

    public async Task<ErrorResult> Handle(WelcomeEmailQuery request, CancellationToken cancellationToken)
    {
        //ApplicationUser user = ApplicationUser.Factory.Create();
        var user = await _identityService.GetUserByEmail(request.Email);

        //appuser!.Apply(new UserEvents.UserCreated() { Email = appuser.Email, Id = userId });
        _publisher.Publish(new RegisteredSuccessfullyEvent()
        {
            Email = request.Email,
            Name = $"{user.Value.FirstName} {user.Value.LastName}"
        });

        return ErrorResult.Ok();
    }
}