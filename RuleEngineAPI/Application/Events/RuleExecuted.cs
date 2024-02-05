using LsRuleEngine;
using RuleEngineAPI.Domain.Aggregates;

namespace RuleEngineAPI.Application.Events;

public class RuleExecuted(string jsonData, bool ruleApplied, string appliedRuleJsonData, IEnumerable<ConditionResult> conditionsResults) : IEvent
{
    private Guid _id = Guid.NewGuid();
    private DateTimeOffset _created = DateTimeOffset.Now;

    public string JsonData { get; } = jsonData;
    public bool RuleApplied { get; } = ruleApplied;
    public string AppliedRuleJsonData { get; } = appliedRuleJsonData;
    public IEnumerable<ConditionResult> ConditionsResults { get; } = conditionsResults;

    public Guid Id => _id;
    public DateTimeOffset OccurredOn => _created;
}
