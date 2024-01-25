using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Schema;

namespace RuleEngineTester.RuleEngine.WeaklyTyped;


public class RuleBuilder
{
    private readonly ILogger _logger;
    private IRule _rule;
    public RuleBuilder(ILogger logger)
    {
        _logger = logger;
        _rule = new Rule(logger);
    }

    public RuleBuilder ForType(string type, JSchema jsonSchema)
    {
        _rule.SetType(type, jsonSchema);
        _logger.LogDebug($"Type: {type}");
        _logger.LogDebug(jsonSchema.ToString());
        return this;
    }

    public RuleBuilder ForType(string type)
    {
        _rule.SetType(type, null);
        _logger.LogDebug($"Type: {type}");
        
        return this;
    }

    public RuleBuilder WithName(string name)
    {
        return this;
    }

    public RuleBuilder AddCondition(RuleCondition ruleCondition)
    {
        _rule.AddCondition(ruleCondition);
        return this;
    }

    public RuleBuilder AddAction(RuleAction action)
    {
        _rule.AddAction(action);
        return this;
    }

    public IRule Build() => _rule;
}

