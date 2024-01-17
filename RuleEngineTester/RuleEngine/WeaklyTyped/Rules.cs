using Microsoft.Extensions.Logging;
using RuleEngineTester.RuleEngine.Conditions;
using RuleEngineTester.RuleEngine.Evaluators;
using System.Linq.Expressions;
using static RuleEngineTester.RuleEngine.WeaklyTyped.Rule;

namespace RuleEngineTester.RuleEngine.WeaklyTyped;
public class Rules
{
    private IEnumerable<IRule> _rules;
    private readonly ILogger _logger;

    public Rules(ILogger logger)
    {
        _rules = Enumerable.Empty<IRule>();
        _logger = logger;
    }

    public void Add(IRule rule)
    {
        _rules = _rules.Append(rule);
    }

    public void ExecuteRules(Dictionary<string, string> targets)
    {
        foreach (var rule in _rules)
        {
            var applyToType = rule.GetApplyToTypeName();
            if (targets.TryGetValue(applyToType, out string? jsonData))
            {
                if (jsonData != null)
                {
                    rule.ApplyRule(jsonData);
                }
                else
                {
                    // Log or handle the case where the target for the rule is not found
                    _logger.LogInformation($"{applyToType} not found in rule definition");
                }
            }
            else
            {
                _logger.LogInformation($"No target found for {applyToType}");
            }
        }
    }

    private object RetrieveTargetToApplyRule(string applyToType)
    {
        // Apply search to retrieve the specific object based on applyToType
        return _rules.First(f => f.GetApplyToTypeName() == applyToType);
    }
}

public class RuleAction
{

}

