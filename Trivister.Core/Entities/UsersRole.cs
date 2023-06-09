using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Trivister.Core.DomainEvents;

namespace Trivister.Core.Entities;

public class UsersRole: IdentityUserRole<Guid>
{
    internal UsersRole(Guid userId, Guid roleId, string? discriminator)
    {
        RoleId = roleId;
        UserId = userId;
        Discriminator = discriminator;
        CreatedOn = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }
    [NotMapped]
    public string RoleName { get; set; }
    [NotMapped]
    public string Name { get; set; }
    private DateTime CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime LastModified { get; set; }
    private string? LastModifiedBy { get; set; }
    private string Discriminator { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
    private static readonly List<object> Events = new List<object>();

    public class Factory
    {
        public static UsersRole Create(Guid userId, Guid roleId)
        {
            return new UsersRole(userId, roleId, "UsersRole");
        }
    }
    
    protected void Apply(object @event)
    {
        When(@event);
        AddEvents(@event);
    }
    
    private void When(object @event)
    {
        switch (@event)
        {
            case UsersRoleEvents.UserRoleCreated e:
                Name = e.Name;
                RoleName = e.RoleName;
                break;
            case UsersRoleEvents.UserRoleEdited e:
                Name = e.Name;
                RoleName = e.RoleName;
                break;
                break;
            case UsersRoleEvents.UserRoleDeleted e:
                Name = e.Name;
                RoleName = e.RoleName;
                break;
                break;
        }
    }
        
    private static void AddEvents(object @event)
    {
        Events.Add(@event);
    }
    
    public IEnumerable<object> GetChanges() => Events.AsEnumerable();
    
    public void ClearChanges() => Events.Clear();
}