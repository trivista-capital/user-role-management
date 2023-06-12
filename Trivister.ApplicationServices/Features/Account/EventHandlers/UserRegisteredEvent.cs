using MediatR;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Features.OTP_Management;

namespace Trivister.ApplicationServices.Features.Account.EventHandlers;

public class UserRegisteredEvent: INotification
{
    public string Email { get; set; }
}

public class UserRegisteredEventHandler: INotificationHandler<UserRegisteredEvent>
{
    private readonly IMediator _mediator;
    private readonly IMailManager _mailManager;
    
    public UserRegisteredEventHandler(IMediator mediator, IMailManager mailManager)
    {
        _mediator = mediator;
        _mailManager = mailManager;
    }
    
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        //Call and send OTP here
        var otp = await _mediator.Send(new OTPCommand(notification.Email), cancellationToken);
        _mailManager.BuildOTPMessage(otp.Value, notification.Email);
    }
}