using LsRuleEngine.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace LsRuleEngine;
public class Rules(ILogger logger)
{
    private IEnumerable<IRule> _rules = Enumerable.Empty<IRule>();
    private readonly ILogger _logger = logger;
    private readonly List<RuleExecutionResult> _rulesExecutionResult = [];

    public void Add(IRule rule)
    {
        _rules = _rules.Append(rule);
    }
    public void AddRange(IList<IRule> rules)
    {
        foreach (IRule rule in rules)
        {
            _rules = _rules.Append(rule);
        }
    }

    public IEnumerable<RuleExecutionResult> GetRuleExecutionResults() => _rulesExecutionResult;
    public bool RuleApplied => _rulesExecutionResult.All(w=>w.Succeed);

    public JObject? ExecuteRules(string jsonData)
    {
        JObject? result = null;
        foreach (var rule in _rules)
        {
            var applyToType = rule.GetApplyToTypeName();

            if (jsonData != null)
            {

                var executionResult = rule.ApplyRule(jsonData);
                jsonData = JsonConvert.SerializeObject(executionResult.Target);
                _rulesExecutionResult.Add(executionResult);
                result = executionResult.Target;
            }
            else
            {
                // Log or handle the case where the target for the rule is not found
                _logger.LogInformation("{applyToType} not found in rule definition", applyToType);
            }
        }
        return result;
    }

    private object RetrieveTargetToApplyRule(string applyToType)
    {
        // Apply search to retrieve the specific object based on applyToType
        return _rules.First(f => f.GetApplyToTypeName() == applyToType);
    }
}

