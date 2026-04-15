//using RuleEngineTester.RuleEngine.Conditions;

using LsRuleEngine.Enums;

namespace LsRuleEngine;

public record RuleConditionDefinition(
    string PropertyName,
    ConditionType ConditionType,
    // Type PropertyType,
    object? Value,
    OperatorType ConditionOperator);
