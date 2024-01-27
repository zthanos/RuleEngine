using Newtonsoft.Json.Schema;


namespace LsRuleEngine.Interfaces;

public interface IRule
{
    RuleExecutionResult ApplyRule(string jsonData);
    JSchema GetApplyToType();
    string GetApplyToTypeName();
    void SetType(string type, JSchema jsonSchema);
    //Dictionary<string, object> SetRuleType(object target);
    void AddCondition(RuleCondition ruleCondition);
    void AddAction(RuleAction action);
    public string Name { get; }
    //public void AddName(string name);
    public IEnumerable<RuleCondition> Conditions { get; }
    public IEnumerable<RuleAction> Actions { get; }
    public string TypeToApplyRule { get; }


}
