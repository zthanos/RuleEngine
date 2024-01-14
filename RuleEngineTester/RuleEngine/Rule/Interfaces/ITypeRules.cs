using RuleEngineTester.RuleEngine.Conditions;

namespace RuleEngineTester.RuleEngine.Rule.Interfaces;

public interface IRulesProcessor<T>
{
    void ApplyRules(T target);
    void AddRule(List<Condition> conditions);
}