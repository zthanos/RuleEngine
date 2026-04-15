//using EventStore.Client;
//using MediatR;
//using RuleEngineAPI.Application.Events;
//using RuleEngineAPI.Infrastructure.Interfaces;
//using System.Text;
//using System.Text.Json;


//namespace RuleEngineAPI.Infrastructure.Services;

//public class EventStoreDBService : IEventStore
//{
//    private readonly EventStoreClient _client;
//    //private readonly IProjectionUpdater _projectionUpdater;


//    public EventStoreDBService(EventStoreClient client)
//    {
//        _client = client;
//        //_projectionUpdater = projectionUpdater;
//        SubscribeToEvents();
//    }

//    private void SubscribeToEvents()
//    {
//        // Subscribe to $all stream or specific streams based on your requirements
//        _client.SubscribeToAllAsync(
//            FromAll.Start,
//            eventAppeared: async (subscription, resolvedEvent, cancellationToken) =>
//            {
//                // Check event type and handle accordingly
//                var eventType = resolvedEvent.Event.EventType;
//                var eventData = Encoding.UTF8.GetString(resolvedEvent.Event.Data.Span);

//                // Example: call projection updater based on event type
//                //await _projectionUpdater.UpdateProjectionAsync(eventType, eventData);
//            },
//            // Subscription options
//            filterOptions: new SubscriptionFilterOptions(EventTypeFilter.ExcludeSystemEvents()));
//    }

//    public async Task AppendEventAsync<TEvent>(string streamName, TEvent @event) where TEvent : INotification
//    {
//        //var esdb = Environment.GetEnvironmentVariable("EVENTSTORE_CONNECTION_STRING");
//        var eventData = new EventData(
//            Uuid.NewUuid(),
//            typeof(TEvent).Name,
//            JsonSerializer.SerializeToUtf8Bytes(@event),
//            metadata: null);

//        await _client.AppendToStreamAsync(
//            streamName,
//            StreamState.Any,
//            new[] { eventData });
//    }

//    public async Task<IEnumerable<T>> ReadEventsAsync<T>(string streamName) where T : class
//    {
//        var result = _client.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);
//        var events = new List<T>();

//        await foreach (var @event in result)
//        {
//            if (@event.Event.EventType == typeof(T).Name)
//            {
//                var eventData = JsonSerializer.Deserialize<T>(@event.Event.Data.Span);
//                if (eventData != null)
//                {
//                    events.Add(eventData);
//                }
//            }
//        }

//        return events;
//    }

//    public async Task<T> ReadAggregateAsync<T>(string streamName) where T : class, new()
//    {
//        var events = await ReadEventsAsync<T>(streamName);
//        var aggregate = new T();

//        // Apply each event to the aggregate
//        foreach (var @event in events)
//        {
//            // You would have some method on your aggregate to apply the event to its state
//            // For example: aggregate.ApplyEvent(@event);
//        }

//        return aggregate;
//    }
//}
//public interface IProjectionUpdater
//{
//    Task UpdateProjectionAsync(string eventType, string eventData);
//}


////public class ProjectionUpdater : IProjectionUpdater
////{
////    // Example: Using a generic repository or direct database access to update projections
////    private readonly IRuleStorageService _repository;

////    public ProjectionUpdater(IRuleStorageService repository)
////    {
////        _repository = repository;
////    }

////    public async Task UpdateProjectionAsync(string eventType, string eventData)
////    {
////        switch (eventType)
////        {
////            case nameof(RuleSetCreated):
////                var ruleSetCreatedEvent = JsonSerializer.Deserialize<RuleSetCreated>(eventData);
////                if (ruleSetCreatedEvent != null)
////                {
////                    UpdateRuleSetCreatedProjection(ruleSetCreatedEvent);
////                }
////                break;
////                // Handle other event types
////        }
////    }


////    private bool UpdateRuleSetCreatedProjection(RuleSetCreated event)
////{
////        return Task.CompletedTask;
////    }
////    }
////    //private async Task UpdateRuleSetCreatedProjection1(RuleSetCreated event)
////    //{
////    //    // Logic to update the projection for a RuleSetCreated event
////    //    // This might involve adding a new rule set summary to your read model database
////    //    //var projection = new
////    //    //{
////    //    //    RuleSetId = event.RuleSetId,
////    //    //    TypeToApply = event.TypeToApply,
////    //    //    // Other properties
////    //    //};

////    //    //await _repository.AddOrUpdateAsync(projection);
////    //}
////}