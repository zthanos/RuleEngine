

namespace RuleEngineTester.RuleEngine;
public interface IRule<T>
{
    public void ApplyRules(T target);
}

public interface IRule
{
    void AddActions(IList<Action> actions);
    void AddConditions(IList<Condition> conditions);

}