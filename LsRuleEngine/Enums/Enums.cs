namespace LsRuleEngine.Enums;

public enum ConditionType
{
    None,
    Null,
    NotNull,
    Empty,
    NotEmpty,
    Equals,
    NotEquals,
    GreaterThan,
    LessThan,
    GreaterThanOrEquals,
    LessThanOrEquals,
    Include,
    Composite
}

public enum OperatorType
{
    None,
    And,
    Or
}
