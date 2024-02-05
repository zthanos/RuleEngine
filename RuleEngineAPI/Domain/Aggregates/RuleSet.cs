using EventStore.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using RuleEngineAPI.Application.Events;
using RuleEngineAPI.Domain.Interfaces;
using System.Text;

namespace RuleEngineAPI.Domain.Aggregates;

public class RuleSet : IAggregateRoot
{
    public Guid Id { get; private set; }

    public string TypeToApplyRule { get; private set; }
    public string Schema { get; private set; }
    public string Rules { get; private set; }
    public int Version { get; private set; }
    public bool IsActive { get; private set; }
    public IEnumerable<RuleExecutionResults> ExecutionResults { get; private set; } = new List<RuleExecutionResults>();


    public string AggregateId => TypeToApplyRule;

    private readonly List<IEvent> _uncommittedEvents = new List<IEvent>();



    public void CreateRuleForType(Guid id, string typeToApply, string initialRules, string schema, int version)
    {
        var jschema = JSchema.Parse(schema);

        var ruleSetCreatedevent = new RuleSetCreated(id, typeToApply, schema, initialRules, version);

        _uncommittedEvents.Add(ruleSetCreatedevent);
        ApplyRulesetCreatedEvent(ruleSetCreatedevent);
    }

    public void ExecuteRule(IRuleManagerService ruleManagerService, string jsonData)
    {
        var results = ruleManagerService.ExecuteRules(this, jsonData);
        var ruleExecuted = new RuleExecuted(jsonData, results.RuleApplied, results.OutputData, results.ConditionResults);
        _uncommittedEvents.Add(ruleExecuted);
        ApplyRuleExecutedEvent(ruleExecuted);
    }


    protected void ApplyRulesetCreatedEvent(RuleSetCreated ruleSetCreatedevent)
    {
        TypeToApplyRule = ruleSetCreatedevent.TypeToApply;
        Schema = ruleSetCreatedevent.Schema;
        Rules = ruleSetCreatedevent.Rules;
        Version = ruleSetCreatedevent.version;
        Id = ruleSetCreatedevent.RuleSetId;
        IsActive = true;

    }

    public void ApplyRuleExecutedEvent(RuleExecuted @event)
    {
        RuleExecutionResults results = new(@event.JsonData, @event.AppliedRuleJsonData, @event.RuleApplied, @event.ConditionsResults);
        ExecutionResults.Append(results);
    }

    public void ClearUncommittedEvents() => _uncommittedEvents.Clear();

    public IReadOnlyList<IEvent> GetUncommittedEvents() => _uncommittedEvents;

    public void LoadFromHistory(IEnumerable<ResolvedEvent> history)
    {
        foreach (var streamEvent in history)
        {
            var domainEvent = DeserializeEvent(streamEvent);
            if (domainEvent == null) return;

            // Pattern matching to handle different event types
            switch (domainEvent)
            {
                case RuleSetCreated ruleSetCreatedEvent:
                    ApplyRulesetCreatedEvent(ruleSetCreatedEvent);
                    break;
                case RuleExecuted ruleExecutedEvent:
                    ApplyRuleExecutedEvent(ruleExecutedEvent);
                    break;
                    // Add other cases as needed
            }
        }
    }
    public void LoadFromHistory(IEnumerable<IEvent> history)
    {
        foreach (var domainEvent in history)
        {

            // Pattern matching to handle different event types
            switch (domainEvent)
            {
                case RuleSetCreated ruleSetCreatedEvent:
                    ApplyRulesetCreatedEvent(ruleSetCreatedEvent);
                    break;
                case RuleExecuted ruleExecutedEvent:
                    ApplyRuleExecutedEvent(ruleExecutedEvent);
                    break;
                    // Add other cases as needed
            }
        }
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
