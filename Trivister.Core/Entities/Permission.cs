using Trivister.Core.Contract;
using Trivister.Core.DomainEvents;

namespace Trivister.Core.Entities;

public class Permission: BaseEntityOfT<int>, IEvent
{
    public Permission()
    {
        CreatedOn = DateTime.UtcNow;
        LastModified=DateTime.UtcNow;
    }
    public string Name { get; set; }
    public string? Description { get; set; }

    protected override void When(object @event)
    {
        switch (@event)
        {
            case PermissionEvents.PermissionCreated p:
                Id = p.Id;
                Name = p.Name;
                break;
            case PermissionEvents.PermissionEdited p:
                Id = p.Id;
                Name = p.Name;
                break;
        }
    }
}