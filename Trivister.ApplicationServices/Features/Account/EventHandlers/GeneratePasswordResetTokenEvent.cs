using MediatR;
using Trivister.ApplicationServices.Abstractions;

namespace Trivister.ApplicationServices.Features.Account.EventHandlers;

public class GeneratePasswordResetTokenEvent: INotification
{
    public string Email { get; set; }
    public string ConfirmationLink { get; set; }
}

public class GeneratePasswordResetTokenEventHandler: INotificationHandler<GeneratePasswordResetTokenEvent>
{
    private readonly IMediator _mediator;
    private readonly IMailManager _mailManager;
    
    public GeneratePasswordResetTokenEventHandler(IMediator mediator, IMailManager mailManager)
    {
        _mediator = mediator;
        _mailManager = mailManager;
    }
    
    public async Task Handle(GeneratePasswordResetTokenEvent notification, CancellationToken cancellationToken)
    {
        _mailManager.BuildResetPasswordMessage(notification.ConfirmationLink, notification.Email);
    }
}