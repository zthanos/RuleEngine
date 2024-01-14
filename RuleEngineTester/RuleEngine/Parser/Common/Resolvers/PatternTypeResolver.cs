using RuleEngineTester.RuleEngine.Parser.Common.Types;

namespace RuleEngineTester.RuleEngine.Parser.Common.Resolvers;

public static class PatternTypeResolver
{
    public static string ResolvePatternForType(PatternType patternType)
    {
        return patternType switch
        {
            PatternType.RuleName => @"Rule Name:\s*(.*?)\n",
            PatternType.ClassName => @"Applies to:\s*(.*?)\n",
            PatternType.ConditionsSplit => @"When:([\S\s]*?)Then:",
            PatternType.ConditionsParse => @"- \s*(.*?)\n",
            PatternType.SubConditions => @"(and)|(or)",
            PatternType.ActionsSplit => @"Then:([\S\s]*?)Rule End",
            PatternType.ActionsParse => @"(Set | to )",
            PatternType.Operator => ConditionOperatorResolver.GetPattern(),
            _ => throw new ArgumentException("does not exist pattern for {patternType} ", nameof(patternType))
        };
    }
}
