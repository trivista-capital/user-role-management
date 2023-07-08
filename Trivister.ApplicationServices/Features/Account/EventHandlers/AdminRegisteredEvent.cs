using MediatR;
using Microsoft.Extensions.Logging;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Features.OTP_Management;

namespace Trivister.ApplicationServices.Features.Account.EventHandlers;

public class AdminRegisteredEvent: INotification
{
    public string Email { get; set; }
    public string Name { get; set; }
}

public class AdminRegisteredEventHandler: INotificationHandler<AdminRegisteredEvent>
{
    private readonly IMailManager _mailManager;
    private readonly ILogger<AdminRegisteredEventHandler> _logger;

    public AdminRegisteredEventHandler(IMailManager mailManager, ILogger<AdminRegisteredEventHandler> logger)
    {
        _mailManager = mailManager;
        _logger = logger;
    }
    
    public async Task Handle(AdminRegisteredEvent notification, CancellationToken cancellationToken)
    {
        //Call and send OTP here
        try
        {
            _mailManager.BuildAdminUserInvitationMessage(to: notification.Email, adminName: notification.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured sending email to Admin user");
        }
    }
}