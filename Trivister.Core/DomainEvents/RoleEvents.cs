using Trivister.Core.Contract;

namespace Trivister.Core.DomainEvents;

public static class RoleEvents
{
    public class RoleCreated: IDomainEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    
    public class RoleLoggedIn: IDomainEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    
    public class RoleEdited: IDomainEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    
    public class RoleDeleted: IDomainEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}