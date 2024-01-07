namespace RuleEngineTester.RuleEngine;

public class Condition
{
    public int Id { get; }
    public string Description { get; set; }
    public string Name { get; }
    public object Value { get; }
    public ConditionType ConditionType { get; }
    public OperatorType Operator { get; set; }


    public Condition(int id, string propertyName, object? value, string type, string operatorName)
    {
        Id = id;
        Description = $"{propertyName} {type} {value}";
        Name = propertyName;
        Value = value;
        ConditionType = GetConditionTypeByName(type);
        Operator = GetOperatorTypeByName(operatorName);
    }
    public Condition(int id, string propertyName, string value, ConditionType type, string operatorName)
    {
        Id = id;
        Name = propertyName;
        Value = value;
        ConditionType = type;
        Operator = GetOperatorTypeByName(operatorName);
    }

    public ConditionType GetConditionTypeByName(string name)
    {
        switch (name)
        {
            case "Null": return ConditionType.Null;
            case "NotNull": return ConditionType.NotNull;
            case "Equals": return ConditionType.Equals;
            case "NotEquals": return ConditionType.NotEquals;
            case "GreaterThan": return ConditionType.GreaterThan;
            case "LessThan": return ConditionType.LessThan;
            case "GreaterThanOrEquals": return ConditionType.GreaterThanOrEquals;
            case "LessThanOrEquals": return ConditionType.LessThanOrEquals;
            case "Include": return ConditionType.Include;
            default: return ConditionType.None;
        }
    }

    public OperatorType GetOperatorTypeByName(string name)
    {
        switch (name)
        {
            case "And": return OperatorType.None;
            case "Or": return OperatorType.And;
            default: return OperatorType.Or;
        }
    }

    public bool ExecuteCondition(string property, object? value, object? expectedValue)
    {
        switch (Value)
        {
            case ConditionType.NotNull: return value != null;
            case ConditionType.NotEquals: return !object.Equals(value, expectedValue);
            case ConditionType.GreaterThan: return CompareValues(value, expectedValue) > 0;
            case ConditionType.LessThan: return CompareValues(value, expectedValue) < 0;
            case ConditionType.GreaterThanOrEquals: return CompareValues(value, expectedValue) >= 0;
            case ConditionType.LessThanOrEquals: return CompareValues(value, expectedValue) <= 0;
            case ConditionType.Include: return IncludeCheck(value, expectedValue);
        }
        return true;

    }

    private int CompareValues(object? value, object? expectedValue)
    {
        // Implement comparison logic for different types if necessary
        if (value is IComparable comparableValue && expectedValue is IComparable comparableExpectedValue)
        {
            return comparableValue.CompareTo(comparableExpectedValue);
        }

        // Default to equality if types are not comparable
        return object.Equals(value, expectedValue) ? 0 : 1;
    }

    private bool IncludeCheck(object? value, object? expectedValue)
    {
        // Implement logic to check if value contains expectedValue (e.g., for string or collection types)
        if (value is string stringValue && expectedValue is string expectedStringValue)
        {
            return stringValue.Contains(expectedStringValue);
        }
        else if (value is IEnumerable<object> collectionValue && expectedValue is IEnumerable<object> expectedCollectionValue)
        {
            return collectionValue.Intersect(expectedCollectionValue).Any();
        }

        // Default to true if types are not suitable for an "include" check
        return true;
    }

}

public enum ConditionType
{
    None,
    Null,
    NotNull,
    NotEmpty,
    Equals,
    NotEquals,
    GreaterThan,
    LessThan,
    GreaterThanOrEquals,
    LessThanOrEquals,
    Include
}

public enum OperatorType
{
    None,
    And,
    Or
}
