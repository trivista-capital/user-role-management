using Trivister.Core.Contract;

namespace Trivister.Core.DomainEvents;

public static class UserEvents
{
    public class UserCreated: IDomainEvent
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
    
    public class UserLoggedIn: IDomainEvent
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
    
    public class UserEdited: IDomainEvent
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
    
    public class UserDeleted: IDomainEvent
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}