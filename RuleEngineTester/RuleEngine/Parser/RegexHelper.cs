using System.Text.RegularExpressions;

namespace RuleEngineTester.RuleEngine.Parser;
public static class RegexHelper
{
    private static readonly RegexOptions _RegexOptions = RegexOptions.Multiline;
    public static string ExtractRuleName(string ruleText)
    {
        var m = Regex.Matches(ruleText, PatternTypeResolver.ResolvePatternForType(PatternType.RuleName), _RegexOptions);
        if (m.Count() < 1)
            throw new Exception("Invalid rule format");
        return m.Last().Groups[1].Value.RemoveNewLine();
    }

    public static string ExtractClassName(string ruleText)
    {
        var m = Regex.Matches(ruleText, PatternTypeResolver.ResolvePatternForType(PatternType.ClassName), _RegexOptions);
        if (m.Count() < 1)
            throw new Exception("Invalid rule format");
        return m.Last().Groups[1].Value.RemoveNewLine();
    }
    public static List<Condition> ExtractConditions(string ruleText)
    {
        var result = new List<Condition>();
        var match = Regex.Matches(
            ruleText,
            PatternTypeResolver.ResolvePatternForType(PatternType.ConditionsSplit),
            _RegexOptions);
        if (match.Count() < 1)
            throw new Exception("Invalid rule format");
        var plainTextConditions = match.First().Groups[1].Value;
        var matched_conditions = Regex.Matches(
            plainTextConditions,
            PatternTypeResolver.ResolvePatternForType(PatternType.ConditionsParse),
            _RegexOptions);
        foreach(var matched_condition in matched_conditions.Select(s => s.Groups[1].Value))
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
                    Console.WriteLine($"Error while parsing condition: {matched_condition}");
                }

            }
            else //is composite
            {
                Condition composite = new Condition(1, "", null, "Composite", "and");
                composite.SubConditions.AddRange(subconditions);
                result.Add(composite);
            }
        }
        //return matched_conditions.Select(s => s.Groups[1].Value).ToArray();
        return result;
    }

    public static List<Condition> ExtractSubConditions(string ruleText)
    {
        List<Condition> result = [];

        var match = Regex.Split(ruleText, PatternTypeResolver.ResolvePatternForType(PatternType.SubConditions), _RegexOptions);
        if (match.Count() > 1)
        {
            var idx = 0;
            while (idx < match.Length)
            {
                var res = match.Select(s => s).Skip(idx).Take(2).ToArray();
                var parsed = ExtractOperator(res.First());
                if (parsed != null) {
                    string logical_operator = (res.Length > 1 && res.Last() == "or") ? "or" : "and";
                    result.Add(new Condition(1, parsed.Left, parsed.Right, parsed.Operation, logical_operator));
                }
                else
                {
                    Console.WriteLine($"Unable to process subCondition: {res.First()}");
                }

                idx += 2;
            }
        }
        return result;
    }

    public static List<Action> ExtractActions(string ruleText)
    {
        List<Action> result = [];
        const int PropertyIdx = 2;
        const int ValueIdx = 4;
        var match = Regex.Matches(ruleText, PatternTypeResolver.ResolvePatternForType(PatternType.ActionsSplit), _RegexOptions);
        if (match.Count() < 1)
            throw new Exception("Invalid rule format");

        var plainTextConditions = match.First().Groups[1].Value;
        var matched_conditions = Regex.Split(plainTextConditions, PatternTypeResolver.ResolvePatternForType(PatternType.ActionsParse), _RegexOptions);

        if (matched_conditions.Count() != 5)
        {
            throw new Exception("Error in action Definition");
        }

        result.Add(new Action(matched_conditions[PropertyIdx], matched_conditions[ValueIdx].RemoveNewLine(), null));
        
        return result;
    }


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
    public string Left { get; set;}
    public string? Right { get; set; }
    public string Operation{ get; set; }

}