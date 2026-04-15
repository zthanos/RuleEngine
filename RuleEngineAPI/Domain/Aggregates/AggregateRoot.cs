using static EventStore.Client.StreamMessage;

namespace RuleEngineAPI.Domain.Aggregates;

public abstract class AggregateRoot
{
    private readonly List<Event> _uncommittedEvents = new List<Event>();
    public Guid Id { get; protected set; }
    public int Version { get; private set; } = -1;

    public IReadOnlyList<Event> GetUncommittedEvents() => _uncommittedEvents.AsReadOnly();

    public void ClearUncommittedEvents() => _uncommittedEvents.Clear();

    protected void RaiseEvent(Event @event)
    {
        _uncommittedEvents.Add(@event);
        Apply(@event);
    }

    protected void ApplyEvent(Event @event, bool isNew)
    {
        // Dynamically invoke the apply method for the event
        ((dynamic)this).Apply((dynamic)@event);
        if (isNew) Version++;
    }

    public void LoadFromHistory(IEnumerable<Event> history)
    {
        foreach (var @event in history)
        {
            ApplyEvent(@event, false);
            Version++;
        }
    }

    protected abstract void Apply(Event @event);
}
