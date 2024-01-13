//using Microsoft.Extensions.FileSystemGlobbing.Internal;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace RuleEngineTester.RuleEngine.Parser;
//public class KYCValidationRuleParser //: RuleParserBase
//{
//    public override Rule Parse(string ruleText)
//    {
//        // Extract the rule name
//        var ruleName = ExtractRuleName(ruleText);

//        // Extract the when clause
//        var whenClause = ExtractWhenClause(ruleText);

//        // Extract the then clause
//        var thenClause = ExtractThenClause(ruleText);

//        // Parse the when clause into conditions
//        var conditions = ParseConditions(whenClause);

//        // Parse the then clause into actions
//        var actions = ParseActions(thenClause);

//        // Create the rule object
//        var rule = new Rule()
//        {
//            Name = ruleName,
//            Conditions = conditions,
//            Actions = actions
//        };

//        return rule;
//    }

//    private string ExtractRuleName(string ruleText)
//    {
//        var pattern = @"Rule Name:\s*(.*?)\n";
//        RegexOptions options = RegexOptions.Multiline;
//        // Find the line that starts with "Rule Name:"
//        var m = Regex.Matches(ruleText, pattern, options);
//        if (m.Count() < 1)
//            throw new Exception("Invalid rule format");
//        return m.First().Value;
//    }

//    private string ExtractWhenClause(string ruleText)
//    {
//        // Find the line that starts with "When:"
//        Match whenClauseMatch = ruleText.Match("When:\s*(.*)Then:");
//        if (!whenClauseMatch.Success) throw new Exception("Invalid rule format");

//        // Extract the when clause from the match group
//        return whenClauseMatch.Groups[1].Value;
//    }

//    private string ExtractThenClause(string ruleText)
//    {
//        // Find the line that starts with "Then:"
//        Match thenClauseMatch = ruleText.Match("When:\s*.*Then:\s*(.*)");
//        if (!thenClauseMatch.Success) throw new Exception("Invalid rule format");

//        // Extract the then clause from the match group
//        return thenClauseMatch.Groups[1].Value;
//    }

//    private Dictionary<string, bool> ParseConditions(string whenClause)
//    {
//        // Split the when clause into conditions
//        var conditions = whenClause.Split("and");

//        // Create a dictionary to store the conditions
//        var conditionDictionary = new Dictionary<string, bool>();

//        // Parse each condition into a key-value pair
//        foreach (string condition in conditions)
//        {
//            // Extract the condition operator and value
//            Match conditionMatch = condition.Match("(?<operator>\w+)\s+(?<value>.*)");
//            if (!conditionMatch.Success) throw new Exception("Invalid condition format");

//            // Set the condition in the dictionary
//            conditionDictionary.Add(conditionMatch.Groups["operator"].Value, bool.Parse(conditionMatch.Groups["value"].Value));
//        }

//        return conditionDictionary;
//    }

//    private List<Action> ParseActions(string thenClause)
//    {
//        // Split the then clause into actions
//        var actions = thenClause.Split("Set Customer.");

//        // Create a list to store the actions
//        var actionList = new List<Action>();

//        // Parse each action into an Action object
//        foreach (string action in actions)
//        {
//            // Extract the property and value
//            Match actionMatch = action.Match("(?<property>\w+)\s+(?<value>.*)");
//            if (!actionMatch.Success) throw new Exception("Invalid action format");

//            // Create an Action object
//            actionList.Add(new Action() { Property = actionMatch.Groups["property"].Value, Value = actionMatch.Groups["value"].Value });
//        }

//        return actionList;
//    }
//}