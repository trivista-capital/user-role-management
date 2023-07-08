using MediatR;
using Microsoft.Extensions.Logging;
using Trivister.ApplicationServices.Abstractions;

namespace Trivister.ApplicationServices.Features.Account.EventHandlers;

public class NewCustomerRegisteredEvent: INotification
{
    public string AdminEmail { get; set; }
    public string AdminName { get; set; }
    
    public string CustomerFullName { get; set; }
    
    public string CustomerEmailAddress { get; set; }
    
    public string CustomerPhoneNumber { get; set; }
    
    public string DatOfRegistration { get; set; }
}

public class NewCustomerRegisteredEventHandler: INotificationHandler<NewCustomerRegisteredEvent>
{
    private readonly IMailManager _mailManager;
    private readonly ILogger<NewCustomerRegisteredEventHandler> _logger;

    public NewCustomerRegisteredEventHandler(IMailManager mailManager, ILogger<NewCustomerRegisteredEventHandler> logger)
    {
        _mailManager = mailManager;
        _logger = logger;
    }
    
    public async Task Handle(NewCustomerRegisteredEvent notification, CancellationToken cancellationToken)
    {
        //Call and send OTP here
        try
        {
            _mailManager.BuildMessageToAdminOnCustomerRegistrationMessage(notification.AdminEmail, notification.AdminName, notification.CustomerFullName, 
                notification.CustomerEmailAddress, notification.CustomerPhoneNumber, notification.DatOfRegistration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured sending email to Admin user");
        }
    }
}