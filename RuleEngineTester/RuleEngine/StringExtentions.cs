using System.Text.RegularExpressions;

namespace RuleEngineTester.RuleEngine;

public static class StringExtentions
{
    public static string RemoveNewLine(this string str)
    {
        return Regex.Replace(str, @"\t|\n|\r", "");
    }
}
