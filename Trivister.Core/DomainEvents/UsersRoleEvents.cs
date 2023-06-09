using Trivister.Core.Contract;

namespace Trivister.Core.DomainEvents;

public static class UsersRoleEvents
{
    public class UserRoleCreated: IDomainEvent
    {
        public string RoleName { get; set; }
        public string Name { get; set; }
    }

    public class UserRoleEdited: IDomainEvent
    {
        public string RoleName { get; set; }
        public string Name { get; set; }
    }
    
    public class UserRoleDeleted: IDomainEvent
    {
        public string RoleName { get; set; }
        public string Name { get; set; }
    }
}