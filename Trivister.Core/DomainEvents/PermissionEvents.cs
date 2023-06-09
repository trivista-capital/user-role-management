using Trivister.Core.Contract;

namespace Trivister.Core.DomainEvents;

public static class PermissionEvents
{
    public class PermissionCreated: IDomainEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public class PermissionEdited: IDomainEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}