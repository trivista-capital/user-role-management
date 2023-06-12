using Microsoft.AspNetCore.Identity;
using Trivister.Core.DomainEvents;

namespace Trivister.Core.Entities;

public sealed class ApplicationUser: IdentityUser<Guid>
{
    internal ApplicationUser()
    {
        CreatedOn=DateTime.UtcNow;
        LastModified=DateTime.UtcNow;
        IsDisabled = false;
    }
    private ApplicationUser(Guid id, string firstName, string lastName, string email)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        UserName = email;
        LastName = lastName;
        CreatedOn=DateTime.UtcNow;
        LastModified=DateTime.UtcNow;
        IsDisabled = false;
    }
    
    private ApplicationUser(string username, string password)
    {
        UserName = username;
        CreatedOn = DateTime.UtcNow;
        LastModified=DateTime.UtcNow;
        IsDisabled = false;
    }
    public string FirstName { get; private set; }
    public string MiddleName { get; private set; }
    public string LastName { get; private set; }
    public string Address { get; set; }
    public int RoleId { get; set; }
    public bool IsDisabled { get; set; }
    public DateTime CreatedOn { get; private set; }
    
    public string? CreatedBy { get; set; }
    public DateTime LastModified { get; private set; }
    
    public string? LastModifiedBy { get; set; }
    
    public DateTime? DeletedOn { get; set; }
    
    public bool IsDeleted { get; set; }
    
    private static readonly List<object> Events = new List<object>();
    
    public class Factory
    {
        public static ApplicationUser Create(Guid id, string firstName, string lastName, string email)
        {
            return new ApplicationUser(id, firstName, lastName, email);
        }
        
        public static ApplicationUser Create(string username, string password)
        {
            return new ApplicationUser(username, password);
        }
        
        public static ApplicationUser Create()
        {
            return new ApplicationUser();
        }
    }

    public ApplicationUser SetFirstName(string firstName)
    {
        this.FirstName = firstName;
        return this;
    }
    
    public ApplicationUser SetMiddleName(string middleName)
    {
        this.MiddleName = middleName;
        return this;
    }
    
    public ApplicationUser SetLastName(string lastName)
    {
        this.LastName = lastName;
        return this;
    }
    
    public ApplicationUser SetPhoneNumber(string phoneNumber)
    {
        this.PhoneNumber = phoneNumber;
        return this;
    }
    
    public ApplicationUser SetEmail(string email)
    {
        this.Email = email;
        return this;
    }
    
    public ApplicationUser SetAddress(string address)
    {
        this.Address = address;
        return this;
    }
    public ApplicationUser SetLastModifiedDate()
    {
        this.LastModified = DateTime.UtcNow;
        return this;
    }
    
    public ApplicationUser SetCreatedOn()
    {
        this.CreatedOn = DateTime.UtcNow;
        return this;
    }
    
    public void Apply(object @event)
    {
        When(@event);
        AddEvents(@event);
    }
    
    private void When(object @event)
    {
        switch (@event)
        {
            case UserEvents.UserLoggedIn e:
                Id = e.Id;
                Email = e.Email;
                break;
            case UserEvents.UserCreated e:
                Id = e.Id;
                Email = e.Email;
                break;
            case UserEvents.UserEdited e:
                Id = e.Id;
                Email = e.Email;
                break;
            case UserEvents.UserDeleted e:
                Id = e.Id;
                Email = e.Email;
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