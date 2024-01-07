namespace RuleEngineTester.RuleEngine;

public interface IRulesProcessor<T>
{
    void ApplyRules(T target);
    void AddRule(List<Condition> conditions);
}