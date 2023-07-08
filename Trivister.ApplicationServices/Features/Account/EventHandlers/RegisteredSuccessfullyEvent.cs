using MediatR;
using Trivister.ApplicationServices.Abstractions;

namespace Trivister.ApplicationServices.Features.Account.EventHandlers;

public sealed class RegisteredSuccessfullyEvent: INotification
{
    public string Email { get; set; }
    
    public string Name { get; set; }
}

public sealed class RegisteredSuccessfullyEventHandler: INotificationHandler<RegisteredSuccessfullyEvent>
{
    private readonly IMailManager _mailManager;
    
    public RegisteredSuccessfullyEventHandler(IMailManager mailManager)
    {
        _mailManager = mailManager;
    }
    
    public async Task Handle(RegisteredSuccessfullyEvent notification, CancellationToken cancellationToken)
    {
        _mailManager.BuildWelcomeMessage(name: notification.Name, to: notification.Email);
    }
}