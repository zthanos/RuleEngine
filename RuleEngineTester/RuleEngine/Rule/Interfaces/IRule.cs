﻿using RuleEngineTester.RuleEngine.Actions;
using RuleEngineTester.RuleEngine.Conditions;

namespace RuleEngineTester.RuleEngine.Rule.Interfaces;
public interface IRule<T>
{
    public void ApplyRules(T target);
}

public interface IRule
{
    void AddActions(IList<RuleAction> actions);
    void AddConditions(IList<Condition> conditions);
    void SetRuleName(string name);
    void SetAppliesTo(string  appliesTo);
    void AddCondition(Condition condition);
    void AddAction(RuleAction action);

}