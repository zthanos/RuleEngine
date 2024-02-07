using RuleEngineAPI.Domain.Aggregates;

namespace RuleEngineAPI.Application.Events;

public class RuleSetCreated(
    Guid ruleSetId,
    string typeToApply,
    string schema,
    string rules,
    int version,
    IEnumerable<AvailableRule> availableRules) : IEvent
{
    private Guid _id = Guid.NewGuid();
    private DateTimeOffset _created = DateTimeOffset.Now;

    public Guid RuleSetId { get; } = ruleSetId;
    public string TypeToApply { get; } = typeToApply;
    public string Schema { get; } = schema;
    public string Rules { get; } = rules;
    public int version { get; } = version;
    public IEnumerable<AvailableRule> AvailableRules { get; } = availableRules;

    public Guid Id => _id;
    public DateTimeOffset OccurredOn => _created;
}
