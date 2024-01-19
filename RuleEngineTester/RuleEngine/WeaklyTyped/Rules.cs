using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RuleEngineTester.RuleEngine.WeaklyTyped;
public class Rules
{
    private IEnumerable<IRule> _rules;
    private readonly ILogger _logger;
    private List<RuleExecutionResult> _rulesExecutionResult = [];

    public Rules(ILogger logger)
    {
        _rules = Enumerable.Empty<IRule>();
        _logger = logger;
    }

    public void Add(IRule rule)
    {
        _rules = _rules.Append(rule);
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
                _logger.LogInformation($"{applyToType} not found in rule definition");
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

