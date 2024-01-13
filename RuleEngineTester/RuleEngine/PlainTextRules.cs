using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RuleEngineTester.RuleEngine.Evaluators;
using RuleEngineTester.RuleEngine.Parser;
using System.Text.RegularExpressions;
namespace RuleEngineTester.RuleEngine;

/// <summary>
/// Rule Name: KYCValidationRule

//When:
//  - Customer.Age > 18
//  - Customer.HomeAddress is not null
//  - Customer.Email is not null and IsValidEmail(Customer.Email)
//  - Customer.Phone is not null and IsValidPhoneNumber(Customer.Phone)

//Then:
//  - Set Customer.IsKYCValid to true
/// </summary>
public class PlainTextRules<T> : RuleParserBase<T> where T : class
{
    protected readonly string _RuleText;
    public PlainTextRules(string ruleText)
    {
        _RuleText = ruleText;
    }
    private string GetTypeFromDescription(string description)
    {
        return description.Trim() switch
        {
            ">=" => "GreaterThanOrEquals",
            "<=" => "LessThanOrEquals",
            "<" => "LessThan",
            ">" => "GreaterThan",
            "==" => "Equals",
            "!=" => "NotEquals",
            "is notNull" => "NotNull",
            "is null" => "Null",
            "notEmpty" => "NotEmpty",
            "Empty" => "Empty",
            "Composite" => "Composite",
            _ => "",
        };
    }
    public List<IRule> Parse(string fn)
    {
        var pattern = @"(>=)|(<=)|(<)|(>)|(==)|(!=)|(is notNull)|(is null)|(notEmpty)|(Empty)";
        RegexOptions options = RegexOptions.Multiline;
        var conditions = ExtractConditions(_RuleText);
        RuleSet ruleSet = new();
        var ruleToAdd = new JsonRule
        {
            Name = ExtractRuleName(_RuleText),
            AppliesTo = ExtractClassName(_RuleText),
            RuleConditions = [],
            Actions = []
        };


        foreach (var condition in conditions)
        {
            var subs = ExtractSubConditions(condition);
            if (subs.Count() > 1)
            {
                Condition cnd = CreateCompositeCondition();
                var idx = 0;
                while (idx < subs.Length)
                {
                    var res = subs.Select(s => s).Skip(idx).Take(2).ToArray();
                    idx += 2;
                    var match = Regex.Split(res[0], pattern, options);
                    cnd.SubConditions.Add(ParseCondition(1, res[0]));
                }
                //ruleToAdd.Conditions.Add(ruleCondition);
                ruleToAdd.RuleConditions.Add(cnd);

            }
            else
            {

                ruleToAdd.RuleConditions.Add(ParseCondition(1, subs[0]));
            }
        }

        var actions = ExtractActions(_RuleText);
        foreach (var actionStr in actions)
        {
            var action = ParseAction(actionStr);
            ruleToAdd.Actions.Add(action);
        }

        ruleSet.Rules.Add(ruleToAdd);


        return (List<IRule>)ProcessRuleSet(ruleSet);

    }

    private Condition ParseCondition(int idx, string condition)
    {
        var pattern = @"(>=)|(<=)|(<)|(>)|(==)|(!=)|(is notNull)|(is null)|(notEmpty)|(Empty)";
        RegexOptions options = RegexOptions.Multiline;

        var match = Regex.Split(condition, pattern, options);

        return new Condition(idx,
                          match[0].Trim(),
                          match[2].Trim(),
                          GetTypeFromDescription(match[1]).Trim(),
                          "and");
    }

    private Condition CreateCompositeCondition() => new Condition(1,
                            "",
                            null,
                            "Composite",
                            "and");

    private Action ParseAction(string action)
    {
        var pattern = @"(Set | to )";
        RegexOptions options = RegexOptions.Multiline;

        var match = Regex.Split(action.RemoveNewLine(), pattern, options).Where(w=> !string.IsNullOrEmpty(w));
        if (match.Count() != 4) {
            throw new Exception("Error in action Definition");
        }

        return new Action(match.Skip(1).First(), match.Skip(3).First(), null);
    }

    public string ExtractRuleName(string ruleText)
    {
        var pattern = @"Rule Name:\s*(.*?)\n";

        RegexOptions options = RegexOptions.Multiline;
        // Find the line that starts with "Rule Name:"
        var m = Regex.Matches(ruleText, pattern, options);
        if (m.Count() < 1)
            throw new Exception("Invalid rule format");
        return m.Last().Groups[1].Value.RemoveNewLine();
    }

    public string ExtractClassName(string ruleText)
    {
        var pattern = @"Applies to:\s*(.*?)\n";

        RegexOptions options = RegexOptions.Multiline;
        // Find the line that starts with "Rule Name:"
        var m = Regex.Matches(ruleText, pattern, options);
        if (m.Count() < 1)
            throw new Exception("Invalid rule format");
        return m.Last().Groups[1].Value.RemoveNewLine();
    }
    public string[] ExtractConditions(string ruleText)
    {
        var pattern = @"When:([\S\s]*?)Then:";

        RegexOptions options = RegexOptions.Multiline;
        // Find the line that starts with "Rule Name:"
        var match = Regex.Matches(ruleText, pattern, options);
        if (match.Count() < 1)
            throw new Exception("Invalid rule format");

        pattern = @"- \s*(.*?)\n";
        var plainTextConditions = match.First().Groups[1].Value;
        var matched_conditions = Regex.Matches(plainTextConditions, pattern, options);
        var result = matched_conditions.Select(s => s.Groups[1].Value).ToArray();


        return result;
    }
    public string[] ExtractActions(string ruleText)
    {
        var pattern = @"Then:([\S\s]*?)Rule End";

        RegexOptions options = RegexOptions.Multiline;
        // Find the line that starts with "Rule Name:"
        var match = Regex.Matches(ruleText, pattern, options);
        if (match.Count() < 1)
            throw new Exception("Invalid rule format");

        pattern = @"- \s*(.*?)\n";
        var plainTextConditions = match.First().Groups[1].Value;
        var matched_conditions = Regex.Matches(plainTextConditions, pattern, options);
        var result = matched_conditions.Select(s => s.Groups[1].Value).ToArray();


        return result;
    }

    public string[] ExtractSubConditions(string ruleText)
    {
        var pattern = @"(and)|(or)";
        RegexOptions options = RegexOptions.Multiline;
        // Find the line that starts with "Rule Name:"
        var match = Regex.Split(ruleText, pattern, options);
        if (match.Count() < 1)
        {

            //Console.WriteLine(match.First().Value);
            return [];
        }
        else
        {
            //var idx = 0;
            //while (idx < match.Length)
            //{
            //    var res = match.Select(s => s).Skip(idx).Take(2).ToArray();
            //    idx += 2;
            //    if (res.Count() > 1)
            //    {
            //        //Console.WriteLine($"\t{res[0]} - {res[1]}");

            //    }
            //    else
            //    {
            //        //Console.WriteLine($"\t{res[0]} ");

            //    }
            //}
            return match.Select(s => s.RemoveNewLine()).ToArray();
        }
    }

    public string[] ExtractOperator(string condition)
    {
        var pattern = @"(>=)|(<=)|(<)|(>)|(==)|(notEquals)|(Equals)|(notEmpty)|(Empty)";
        RegexOptions options = RegexOptions.Multiline;
        // Find the line that starts with "Rule Name:"
        var match = Regex.Split(condition, pattern, options);
        if (match.Count() < 1)
        {

            //Console.WriteLine(match.First().Value);
            return [];
        }
        else
        {
            var idx = 0;
            while (idx < match.Length)
            {
                var res = match.Select(s => s).Skip(idx).Take(2).ToArray();
                idx += 2;
                if (res.Count() > 1)
                {
                    Console.WriteLine($"\t{res[0]} - {res[1]}");

                }
                else
                {
                    Console.WriteLine($"\t{res[0]} ");

                }
            }

            return match.Select(s => s.RemoveNewLine()).Where((w, i) => i % 2 == 0).ToArray();
        }
    }

}