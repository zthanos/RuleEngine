using RuleEngineTester.RuleEngine.Conditions;

namespace RuleEngineTester.RuleEngine.WeaklyTyped;

public record RuleConditionDefinition(
    string PropertyName,
    ConditionType ConditionType,
   // Type PropertyType,
    object? Value,
    OperatorType ConditionOperator);
