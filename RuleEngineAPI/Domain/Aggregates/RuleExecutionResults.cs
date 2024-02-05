using LsRuleEngine;

namespace RuleEngineAPI.Domain.Aggregates;

public class RuleExecutionResults(string inputData, string outputData, bool ruleApplied, IEnumerable<ConditionResult> conditionResults)
{
    public DateTime ExecutionDate { get; } = DateTime.Now;
    public string InputData { get; } = inputData;
    public string OutputData { get; } = outputData;
    public bool RuleApplied { get; } = ruleApplied;
    public IEnumerable<ConditionResult> ConditionResults { get; } = conditionResults;
}
