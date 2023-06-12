using MediatR;
using Microsoft.Extensions.Logging;
using Trivister.ApplicationServices.Abstractions;

namespace Trivister.ApplicationServices.Features.Role.EventHandlers;

public sealed record RoleCreatedEvent(Guid Id, string RoleName, string Description) : INotification;

public sealed class RoleCreatedEventHandler: INotificationHandler<RoleCreatedEvent>
{
   private readonly ICustomerClient _customerClient;
   private readonly ILogger<RoleCreatedEventHandler> _logger;
    
   public RoleCreatedEventHandler(ICustomerClient customerClient, ILogger<RoleCreatedEventHandler> logger)
   {
      _customerClient = customerClient;
      _logger = logger;
   }
    
   public async Task Handle(RoleCreatedEvent notification, CancellationToken cancellationToken)
   {
      try
      {
         _logger.LogInformation("Publishing role to loan app from RoleCreatedEventHandler");
         await _customerClient.PublishRole(new(notification.Id, notification.RoleName, 
            notification.Description));
         _logger.LogInformation("Published role successfully from RoleCreatedEventHandler");
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "An error occured in the RoleCreatedEventHandler class");
         throw;
      }
   }
}