using MediatR;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Features.OTP_Management;

namespace Trivister.ApplicationServices.Features.Account.EventHandlers;

public sealed class PasswordSuccessfullyResetTokenEvent: INotification
{
    public string Email { get; set; }
    
    public string Name { get; set; }
}

public sealed class PasswordSuccessfullyResetTokenEventHandler: INotificationHandler<PasswordSuccessfullyResetTokenEvent>
{
    private readonly IMediator _mediator;
    private readonly IMailManager _mailManager;
    
    public PasswordSuccessfullyResetTokenEventHandler(IMediator mediator, IMailManager mailManager)
    {
        _mediator = mediator;
        _mailManager = mailManager;
    }
    
    public async Task Handle(PasswordSuccessfullyResetTokenEvent notification, CancellationToken cancellationToken)
    {
        _mailManager.BuildPasswordSuccessfullyResetMessage(to: notification.Email, name: notification.Name);
    }
}