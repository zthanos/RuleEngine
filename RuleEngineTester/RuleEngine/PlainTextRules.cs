using Newtonsoft.Json;

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
public class PlainTextRules
{

    public PlainTextRules(string ruleText)
    {
        var lines = ruleText.Split("\n").Select(line => line.Trim()).ToList();


        // Assuming the first line is the rule name
        var ruleName = lines[0].Replace("Rule Name: ", "");

        // Extracting conditions from the 'When' section
        var whenSectionIndex = lines.FindIndex(line => line.StartsWith("When:"));
        var thenSectionIndex = lines.FindIndex(line => line.StartsWith("Then:"));
        var rule = new Rule(ruleName);
        lines
            .Skip(whenSectionIndex + 1)
            .Take(thenSectionIndex - whenSectionIndex - 1)
            .Select(line => line.Trim())
            .ToList().ForEach(f => rule.AddCondition(f.TrimStart('-')));

        // Extracting actions from the 'Then' section
        lines
            .Skip(thenSectionIndex + 1)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList().ForEach(f => rule.AddAction(f.TrimStart('-')));


    }

    public void ParseRule()
    {

    }

}

public class Rule
{
    public string RuleName { get; set; }
    public Rule(string name)
    {
        RuleName = name;
        Conditions = new List<string>();
        Actions = new List<string>();
    }
    public void AddCondition(string condition)
    {
        ParseCondition(condition);
        Conditions.Add(condition);
    }

    public void AddAction(string action) => Actions.Add(action);
    public string Name { get; }
    public List<string> Conditions { get; internal set; }
    public List<Condition> ConditionsParsed { get; internal set; }
    public List<string> Actions { get; internal set; }

    private void ParseCondition(string condition)
    {
        if (condition.Contains(">"))
        {
            var parsed = condition.Split(">");
            var a = new Condition(1, parsed[0], parsed[1], ConditionType.GreaterThan, "And");
            Console.WriteLine(JsonConvert.SerializeObject(a));
            var leftSide = parsed[0];
            var rightSide = parsed[1];
            Console.WriteLine(leftSide + " " + rightSide);
        }
        if (condition.Contains("<"))
        {
            var parsed = condition.Split("<");
            var a = new Condition(1, parsed[0], parsed[1], ConditionType.GreaterThan, "And");
            Console.WriteLine(JsonConvert.SerializeObject(a));
            var leftSide = parsed[0];
            var rightSide = parsed[1];
            Console.WriteLine(leftSide + " " + rightSide);
        }
        if (condition.Contains("is not null"))
        {
            var parsed = condition.Split("is not null");
            var leftSide = parsed[0];
            var a = new Condition(1, parsed[0], null, ConditionType.NotNull, "And");
            Console.WriteLine(JsonConvert.SerializeObject(a));
            Console.WriteLine(leftSide + " ");
        }
    }

}