using EventStore.Client;
using Newtonsoft.Json;
using RuleEngineAPI.Application.Events;
using RuleEngineAPI.Infrastructure.Interfaces;
using System.Text;

namespace RuleEngineAPI.Infrastructure.Services;

public class EventStoreService(EventStoreClient client) : IEventStore
{
    private readonly EventStoreClient _client = client;

    public async Task AppendEventAsync(string aggregateId, IEvent @event)
    {
        await _client.AppendToStreamAsync(aggregateId, StreamState.Any, new[]
            {
                new EventData(
                   Uuid.NewUuid(),
                   @event.GetType().Name,
                   System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType()), // Specify runtime type here
                   metadata: null)
            });
    }

    public Task AppendEventAsync<IEvent>(string aggregateId, IEvent @event, int expectedVersion) where IEvent : StreamMessage.Event
    {
        throw new NotImplementedException();
    }

    public Task ExecuteTransactionAsync(Func<Task> transactionOperations)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<StreamMessage.Event>> QueryEventsAsync(Func<StreamMessage.Event, bool> filter)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ResolvedEvent>> oldReadEventsAsync(string aggregateId)
    {

        // Read the stream forward from the start
        var readResult = _client.ReadStreamAsync(
            Direction.Forwards,
            aggregateId,
            StreamPosition.Start);
        IEnumerable<ResolvedEvent> results = [];
        return await readResult.ToListAsync();

    }
    public async Task<IEnumerable<IEvent>> ReadEventsAsync(string aggregateId)
    {
        var events = new List<ResolvedEvent>();
        var results = new List<IEvent>();

        // Read the stream forward from the start
        var readResult = _client.ReadStreamAsync(
            Direction.Forwards,
            aggregateId,
            StreamPosition.Start);

        // Process in batches
        const int batchSize = 500; // Adjust based on your needs and testing
        var batch = new List<ResolvedEvent>(batchSize);

        await foreach (var resolvedEvent in readResult)
        {
            batch.Add(resolvedEvent);
            //if (batch.Count >= batchSize)
            //{
            // Process the batch here (e.g., deserialization, transformation)
            var processedEvent = DeserializeEvent(resolvedEvent);
            if (processedEvent != null)
                results.Add(processedEvent);
            events.AddRange(batch);
            //batch.Clear(); // Prepare for next batch
            //}
        }

        // Process any remaining events in the last batch
        //if (batch.Count > 0)
        //{
        //    events.AddRange(batch);
        //}


        return results;
    }

    public IDisposable Subscribe<TEvent>(Action<TEvent> handler) where TEvent : StreamMessage.Event
    {
        throw new NotImplementedException();
    }


    private IEvent? DeserializeEvent(ResolvedEvent streamEvent)
    {
        var json = Encoding.UTF8.GetString(streamEvent.Event.Data.ToArray());
        if (json is null) return null;

        var eventType = streamEvent.Event.EventType;
        switch (eventType)
        {
            case nameof(RuleSetCreated):
                return JsonConvert.DeserializeObject<RuleSetCreated>(json)!;
            case nameof(RuleExecuted):
                return JsonConvert.DeserializeObject<RuleExecuted>(json)!;
            // Handle other event types
            default:
                throw new InvalidOperationException("Unknown event type.");
        }
    }
}