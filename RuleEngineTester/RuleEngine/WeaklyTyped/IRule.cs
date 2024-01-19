﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace RuleEngineTester.RuleEngine.WeaklyTyped;

public interface IRule
{
    RuleExecutionResult ApplyRule(string jsonData);
    JSchema GetApplyToType();
    string GetApplyToTypeName();
    void SetType(string type, JSchema jsonSchema);
    //Dictionary<string, object> SetRuleType(object target);
    void AddCondition(RuleCondition ruleCondition);
    void AddAction(RuleAction action);
}
