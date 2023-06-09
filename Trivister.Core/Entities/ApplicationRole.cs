using Microsoft.AspNetCore.Identity;
using Trivister.Core.DomainEvents;

namespace Trivister.Core.Entities;

public sealed class ApplicationRole: IdentityRole<Guid>
{
    internal ApplicationRole()
    {
        CreatedOn = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    private ApplicationRole(string name, string description)
    {
        Description = description;
        CreatedOn = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
        this.Name = name;
    }
    public string? Description { get; set; }
    
    public List<ApplicationUser> ApplicationUsers { get; set; } = new();
    public DateTime CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
    private static readonly List<object> Events = new List<object>();

    public class Factory
    {
        public static ApplicationRole Create(string name, string description)
        {
            return new ApplicationRole(name, description);
        }
        
        public static ApplicationRole Create()
        {
            return new ApplicationRole();
        }
    }
    
    public ApplicationRole SetRoleNme(string name)
    {
        this.Name = name;
        return this;
    }
    
    public ApplicationRole SetDescription(string description)
    {
        this.Description = description;
        return this;
    }
    
    public ApplicationRole SetNormalizedName(string name)
    {
        this.NormalizedName = name.ToUpper();
        return this;
    }
    
    public ApplicationRole SetId(Guid id)
    {
        this.Id = id;
        return this;
    }
    
    public ApplicationRole SetLastModified()
    {
        this.LastModified = DateTime.UtcNow;
        return this;
    }

    private void Apply(object @event)
    {
        When(@event);
        AddEvents(@event);
    }
    
    private void When(object @event)
    {
        switch (@event)
        {
            case RoleEvents.RoleCreated e:
                Id = e.Id;
                Name = e.Name;
                break;
            case RoleEvents.RoleDeleted e:
                Id = e.Id;
                Name = e.Name;
                break;
            case RoleEvents.RoleEdited e:
                Id = e.Id;
                Name = e.Name;
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