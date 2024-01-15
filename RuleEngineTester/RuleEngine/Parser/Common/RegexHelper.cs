using System.Text.RegularExpressions;
using RuleEngineTester.RuleEngine.Actions;
using RuleEngineTester.RuleEngine.Conditions;
using RuleEngineTester.RuleEngine.ErrorHandling;
using RuleEngineTester.RuleEngine.Parser.Common.Resolvers;
using RuleEngineTester.RuleEngine.Parser.Common.Types;

namespace RuleEngineTester.RuleEngine.Parser.Common;

/// <summary>
/// Utility class for extracting rule-related information using regular expressions.
/// </summary>
public static class RegexHelper
{
    private static readonly RegexOptions _RegexOptions = RegexOptions.Multiline;
    
    /// <summary>
    /// Extracts the name of the rule from the given rule text.
    /// </summary>
    /// <param name="ruleText">The rule text containing the rule name.</param>
    /// <returns>The extracted rule name.</returns>
    public static string ExtractRuleName(string ruleText)
    {
        var m = Regex.Matches(ruleText, PatternTypeResolver.ResolvePatternForType(PatternType.RuleName), _RegexOptions);
        if (m.Count() < 1)
            throw new ParseRuleException(ruleText, PatternTypeResolver.ResolvePatternForType(PatternType.RuleName), "Unable to retieve RuleName");
        return m.Last().Groups[1].Value.RemoveNewLine();
    }

    /// <summary>
    /// Extracts the class name to which the rule is applied from the given rule text.
    /// </summary>
    /// <param name="ruleText">The rule text containing the class name.</param>
    /// <returns>The extracted class name.</returns>
    public static string ExtractClassName(string ruleText)
    {
        var m = Regex.Matches(ruleText, PatternTypeResolver.ResolvePatternForType(PatternType.ClassName), _RegexOptions);
        if (m.Count() < 1)
            throw new ParseRuleException(ruleText, PatternTypeResolver.ResolvePatternForType(PatternType.ClassName), "Unable to retieve Type to Apply the rule");
        return m.Last().Groups[1].Value.RemoveNewLine();
    }

    /// <summary>
    /// Extracts conditions from the given rule text.
    /// </summary>
    /// <param name="ruleText">The rule text containing conditions.</param>
    /// <returns>An enumerable of conditions.</returns>
    public static IEnumerable<Condition> ExtractConditions(string ruleText)
    {
        var result = new List<Condition>();
        var match = Regex.Matches(
            ruleText,
            PatternTypeResolver.ResolvePatternForType(PatternType.ConditionsSplit),
            _RegexOptions);
        if (match.Count() < 1)
            throw new ParseRuleException(ruleText, PatternTypeResolver.ResolvePatternForType(PatternType.ConditionsSplit), "Unable to retieve conditions.");
        var plainTextConditions = match.First().Groups[1].Value;
        var matched_conditions = Regex.Matches(
            plainTextConditions,
            PatternTypeResolver.ResolvePatternForType(PatternType.ConditionsParse),
            _RegexOptions);
        foreach (var matched_condition in matched_conditions.Select(s => s.Groups[1].Value))
        {
            var subconditions = ExtractSubConditions(matched_condition);

            if (subconditions.Count() == 0)
            {
                var parsed = ExtractOperator(matched_condition);
                if (parsed != null)
                {
                    result.Add(new Condition(1, parsed.Left, parsed.Right, parsed.Operation, "and"));
                }
                else
                {
                    throw new ParseRuleException(matched_condition, PatternTypeResolver.ResolvePatternForType(PatternType.ConditionsParse),
                        "Error while parsing condition.");
                }
            }
            else //is composite
            {
                Condition composite = new Condition(1, "", null, "Composite", "and");
                composite.SubConditions.AddRange(subconditions);
                result.Add(composite);
            }
        }
        return result;
    }

    /// <summary>
    /// Extracts subconditions from the given rule text.
    /// </summary>
    /// <param name="ruleText">The rule text containing subconditions.</param>
    /// <returns>An enumerable of subconditions.</returns>
    public static IEnumerable<Condition> ExtractSubConditions(string ruleText)
    {
        var result = new List<Condition>();
        var match = Regex.Split(ruleText, PatternTypeResolver.ResolvePatternForType(PatternType.SubConditions), _RegexOptions);
        for (int idx = 0; idx < match.Length; idx += 2)
        {
            var subConditionText = match.ElementAtOrDefault(idx);
            var logicalOperator = match.ElementAtOrDefault(idx + 1);

            var parsedCondition = ExtractOperator(subConditionText!);
            if (parsedCondition != null)
            {
                var logicalOperatorType = string.Equals(logicalOperator, "or", StringComparison.OrdinalIgnoreCase) ? "or" : "and";
                result.Add(new Condition(1, parsedCondition.Left, parsedCondition.Right, parsedCondition.Operation, logicalOperatorType));
            }
            else
            {
                throw new ParseRuleException(subConditionText!, PatternTypeResolver.ResolvePatternForType(PatternType.ConditionsParse),
                    "Unable to process subCondition.");
            }
        }
        return result;
    }

    /// <summary>
    /// Extracts actions from the given rule text.
    /// </summary>
    /// <param name="ruleText">The rule text containing actions.</param>
    /// <returns>An enumerable of rule actions.</returns>
    public static IEnumerable<RuleAction> ExtractActions(string ruleText)
    {
        List<RuleAction> result = [];
        const int PropertyIdx = 2;
        const int ValueIdx = 4;
        var match = Regex.Matches(ruleText, PatternTypeResolver.ResolvePatternForType(PatternType.ActionsSplit), _RegexOptions);
        if (match.Count() < 1)
            throw new ParseRuleException(ruleText, PatternTypeResolver.ResolvePatternForType(PatternType.ActionsSplit),
                          "Unable to retrieve actions.");


        var plainTextConditions = match.First().Groups[1].Value;
        var matched_conditions = Regex.Split(plainTextConditions, PatternTypeResolver.ResolvePatternForType(PatternType.ActionsParse), _RegexOptions);

        if (matched_conditions.Count() != 5)
        {
            throw new ParseRuleException(plainTextConditions, PatternTypeResolver.ResolvePatternForType(PatternType.ActionsParse),
                      "Error in action Definition");
        }

        result.Add(new RuleAction(matched_conditions[PropertyIdx], matched_conditions[ValueIdx].RemoveNewLine(), null));

        return result;
    }

    /// <summary>
    /// Represents the left-hand side, operator, and right-hand side of a condition.
    /// </summary>
    public static ConditionExpression? ExtractOperator(string condition)
    {

        var match = Regex.Split(condition, PatternTypeResolver.ResolvePatternForType(PatternType.Operator), _RegexOptions);

        if (match.Count() > 1)
        {
            return new ConditionExpression(match[0],
                                           ConditionOperatorResolver.ResolveOperator(match[1]),
                                           match.Length > 2 ? match[2].RemoveNewLine() : null);
        }
        return null;
    }
}

public class ConditionExpression
{
    public ConditionExpression(string left, string operation, string? right)
    {
        Left = left.Trim();
        Operation = operation;
        Right = right?.RemoveNewLine();
    }
    public string Left { get; set; }
    public string? Right { get; set; }
    public string Operation { get; set; }

}