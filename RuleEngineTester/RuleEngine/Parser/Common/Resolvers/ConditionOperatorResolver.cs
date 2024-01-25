using RuleEngineTester.RuleEngine.Conditions;

namespace RuleEngineTester.RuleEngine.Parser.Common.Resolvers;

public class ConditionOperatorResolver
{
    private static readonly Dictionary<string, string> OperatorMappings = new Dictionary<string, string>
    {
        { ">=", "GreaterThanOrEquals" },
        { "GreaterThanOrEquals", "GreaterThanOrEquals" },
        { "Greater Than Or Equals", "GreaterThanOrEquals" },
        { "<=", "LessThanOrEquals" },
        { "LessThanOrEquals", "LessThanOrEquals" },
        { "Less Than Or Equals", "LessThanOrEquals" },
        { "<", "LessThan" },
        { "LessThan", "LessThan" },
        { "Less Than", "LessThan" },
        { ">", "GreaterThan" },
        { "GreaterThan", "GreaterThan" },
        { "Greater Than", "GreaterThan" },
        { "==", "Equals" },
        { "Equals", "Equals" },
        { "!=", "NotEquals" },
        { "NotEquals", "NotEquals" },
        { "Not Equals", "NotEquals" },
        { "isNotNull", "NotNull" },
        { "is not Null", "NotNull" },
        { "is null", "Null" },
        { "isNull", "Null" },
        { "notEmpty", "NotEmpty" },
        { "not Empty", "NotEmpty" },
        { "Empty", "Empty" },
        { "Composite", "Composite" },
    };

    private static readonly Dictionary<string, ConditionType> ConditionTypeMappings = new Dictionary<string, ConditionType>
    {
        { ">=", ConditionType.GreaterThanOrEquals },
        { "GreaterThanOrEquals", ConditionType.GreaterThanOrEquals },
        { "Greater Than Or Equals", ConditionType.GreaterThanOrEquals },
        { "<=", ConditionType.LessThanOrEquals },
        { "LessThanOrEquals", ConditionType.LessThanOrEquals},
        { "Less Than Or Equals", ConditionType.LessThanOrEquals},
        { "<", ConditionType.LessThan},
        { "LessThan", ConditionType.LessThan},
        { "Less Than", ConditionType.LessThan},
        { ">", ConditionType.GreaterThan},
        { "GreaterThan", ConditionType.GreaterThan},
        { "Greater Than", ConditionType.GreaterThan},
        { "==", ConditionType.Equals},
        { "Equals", ConditionType.Equals},
        { "!=", ConditionType.NotEquals},
        { "NotEquals", ConditionType.NotEquals},
        { "Not Equals", ConditionType.NotEquals},
        { "isNotNull", ConditionType.NotNull},
        { "is not Null", ConditionType.NotNull},
        { "is null", ConditionType.Null},
        { "isNull", ConditionType.Null},
        { "notEmpty", ConditionType.NotEmpty},
        { "not Empty", ConditionType.NotEmpty},
        { "Empty", ConditionType.Empty},
        { "Composite", ConditionType.Composite},
    };

    public static string GetPattern() => string.Join('|', OperatorMappings.Select(m => $"({m.Key})"));
    public static string ResolveOperator(string description)
    {
        return OperatorMappings.TryGetValue(description.Trim(), out var result) ? result : string.Empty;
    }
    public static ConditionType ResolveType(string description) => ConditionTypeMappings.TryGetValue(description.Trim(), out ConditionType result) ? result : ConditionType.None;
}
