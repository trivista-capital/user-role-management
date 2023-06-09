namespace Trivister.Core.Entities;

public abstract class BaseEntity
{
    protected BaseEntity(){}
    
    public DateTime CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
    private static readonly List<object> _events = new List<object>();

    protected void Apply(object @event)
    {
        When(@event);
        _events.Add(@event);
    }
        
    protected static void AddEvents(object @event)
    {
        _events.Add(@event);
    }
    public IEnumerable<object> GetChanges() => _events.AsEnumerable();
    public void ClearChanges() => _events.Clear();
    protected abstract void When(object @event);
}