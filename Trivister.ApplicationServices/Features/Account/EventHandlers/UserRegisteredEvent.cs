using MediatR;
using Microsoft.Extensions.Logging;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Features.OTP_Management;

namespace Trivister.ApplicationServices.Features.Account.EventHandlers;

public class UserRegisteredEvent: INotification
{
    public string Email { get; set; }
    public string Name { get; set; }
}

public class UserRegisteredEventHandler: INotificationHandler<UserRegisteredEvent>
{
    private readonly IMediator _mediator;
    private readonly IMailManager _mailManager;
    private readonly ILogger<UserRegisteredEventHandler> _logger;

    public UserRegisteredEventHandler(IMediator mediator, IMailManager mailManager, ILogger<UserRegisteredEventHandler> logger)
    {
        _mediator = mediator;
        _mailManager = mailManager;
        _logger = logger;
    }
    
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        //Call and send OTP here
        try
        {
            var otp = await _mediator.Send(new OTPCommand(notification.Email), cancellationToken);
            _mailManager.BuildSignUpMessage(otp: otp.Value, to: notification.Email, name: notification.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured sending email to registered user");
        }
    }
}