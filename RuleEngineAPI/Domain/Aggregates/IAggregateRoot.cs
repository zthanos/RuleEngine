using EventStore.Client;
using RuleEngineAPI.Application.Events;

namespace RuleEngineAPI.Domain.Aggregates;

public interface IAggregateRoot
{
    Guid Id { get; }
    string AggregateId { get; }
    int Version { get; }
    /// <summary>
    ///  Retrieves the list of events that have been raised by the aggregate but not yet saved to the event store. 
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<IEvent> GetUncommittedEvents();
    /// <summary>
    ///  Clears the list of uncommitted events, typically called after the events have been successfully persisted.
    /// </summary>
    void ClearUncommittedEvents();
    /// <summary>
    /// Rehydrates the aggregate by applying a sequence of events. This method is used when loading an aggregate from the event store, ensuring it reflects its latest state.
    /// </summary>
    /// <param name="history"></param>
    void LoadFromHistory(IEnumerable<ResolvedEvent> history);
}

