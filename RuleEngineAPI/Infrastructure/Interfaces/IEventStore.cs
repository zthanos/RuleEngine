using RuleEngineAPI.Application.Events;
using static EventStore.Client.StreamMessage;

namespace RuleEngineAPI.Infrastructure.Interfaces;

//public interface IEventStore
//{
//    Task AppendEventAsync<TEvent>(string streamName, TEvent @event) where TEvent : INotification;
//    Task<IEnumerable<T>> ReadEventsAsync<T>(string streamName) where T : class;
//    Task<T> ReadAggregateAsync<T>(string streamName) where T : class, new(); // For rebuilding aggregates from events

//}


public interface IEventStore
{
    /// <summary>
    /// Purpose: To append new events to the store. This function is used by aggregates or command handlers to persist the events generated as a result of executing commands.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="aggregateId"></param>
    /// <param name="event"></param>
    /// <returns></returns>
    Task AppendEventAsync(string aggregateId, IEvent @event);

    /// <summary>
    /// Purpose: To read events for a specific aggregate. This is essential for rehydrating an aggregate to its current state by replaying its events.
    /// </summary>
    /// <param name="aggregateId"></param>
    /// <returns></returns>
    Task<IEnumerable<IEvent>> ReadEventsAsync(string aggregateId);

    /// <summary>
    /// Purpose: To allow subscribers to listen for new events as they are appended to the store. This is useful for triggering projections, integration events, or any side effects outside the aggregate boundaries.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="handler"></param>
    /// <returns></returns>
    IDisposable Subscribe<TEvent>(Action<TEvent> handler) where TEvent : Event;

    //Task SaveSnapshotAsync<TSnapshot>(TSnapshot snapshot) where TSnapshot : Snapshot;
    //Task<TSnapshot> GetSnapshotAsync<TSnapshot>(string aggregateId) where TSnapshot : Snapshot;

    /// <summary>
    /// Purpose: To handle multiple event append operations as a single atomic transaction. This is useful when a single logical operation results in multiple events that should either all succeed or fail together.
    /// </summary>
    /// <param name="transactionOperations"></param>
    /// <returns></returns>
    Task ExecuteTransactionAsync(Func<Task> transactionOperations);

    /// <summary>
    /// Purpose: To keep track of the version (or sequence number) of the last event for each aggregate. This is crucial for concurrency control and ensuring that events are appended in the correct order.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="aggregateId"></param>
    /// <param name="event"></param>
    /// <param name="expectedVersion"></param>
    /// <returns></returns>
    Task AppendEventAsync<TEvent>(string aggregateId, TEvent @event, int expectedVersion) where TEvent : Event;

    /// <summary>
    /// Purpose: To provide a way to query events based on criteria other than the aggregate ID. This is more advanced and might not be necessary for all event stores but can be useful for certain types of analyses or projections.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<IEnumerable<Event>> QueryEventsAsync(Func<Event, bool> filter);

}