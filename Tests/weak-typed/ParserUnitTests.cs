using Antlr4.Runtime;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Schema;
using RuleEngineTester.RuleEngine.Parser.Antlr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tests.weak_typed;

[TestClass]
public class ParserUnitTests
{

    [TestMethod]
    public void TestCustomerSampleRule()
    {
        ILogger logger = LoggerFactory.Create(builder =>
        {
            // Configure logging options here if needed
            builder.AddConsole(); // Example: Add console logging
        }).CreateLogger("");

        JSchemaGenerator generator = new();
        JSchema schema = generator.Generate(typeof(Buyer));

        GetTestData();

        //String ruleText = "Rule Name: \"SampleRule\"\n\nApplies to: \"SomeContext\"\n\nWhen: \n    & (Age > 18 and HomeAddress isNotNull) or Email notEmpty\n\n    & HomeAddress notEmpty\n\n    | Email notEmpty\n\nThen:\n    Set Total to PurchaseAmount * (1 - Discount / 100)\n    Set Discount to Discount + 5\n    Set Discount to 4\n    Set Total to PurchaseAmount\n";
        var ruleFile = File.ReadAllText("applyDiscount.rl");
        logger.LogInformation(ruleFile);
        var inputStream = new AntlrInputStream(ruleFile);
        var lexer = new RulesLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);
        var parser = new RulesParser(tokenStream);

        var tree = parser.ruleFile(); // Start rule
        var visitor = new LsRuleVisitor(schema, logger);
        visitor.Visit(tree);
        var rules = visitor.ParsedRules;
        foreach (var rule in rules)
        {
            rule.ApplyRule(GetTestData());
        }

        Assert.AreEqual(visitor.RuleName, "SampleRule");
        Assert.AreEqual(visitor.AppliesTo, "SomeContext");



    }

    private static string GetTestData()
    {
        var buyer = new Buyer
        {
            CustomerSince = new DateTime(1999, 5, 10),
            PurchaseAmount = 180,
            HomeAddress = "Somewhere 22",
            Email = "paparia@balls.com",
            Age = 34,
            Name = "Thanos",
            Total = 0,
            Discount = 0,
            IsPremium = false
        };
        return JsonConvert.SerializeObject(buyer);
    }
}
