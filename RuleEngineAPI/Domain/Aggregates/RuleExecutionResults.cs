using LsRuleEngine;

namespace RuleEngineAPI.Domain.Aggregates;

public class RuleExecutionResults(Guid id, string inputData, string outputData, bool ruleApplied, IEnumerable<ConditionResult> conditionResults)
{
    public Guid Id { get; } = id;
    public DateTime ExecutionDate { get; } = DateTime.Now;
    public string InputData { get; } = inputData;
    public string OutputData { get; } = outputData;
    public bool RuleApplied { get; } = ruleApplied;
    public IEnumerable<ConditionResult> ConditionResults { get; } = conditionResults;
}
