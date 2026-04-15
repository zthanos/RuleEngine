using LsRuleEngine.Enums;

namespace LsRuleEngine.Resolvers;

public static partial class Resolver
{


    private static readonly Dictionary<string, ConditionType> ConditionTypeMappings = new()
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

    public static ConditionType ResolveType(string description) => ConditionTypeMappings.TryGetValue(description.Trim(), out ConditionType result) ? result : ConditionType.None;
}
