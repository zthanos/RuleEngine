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
using RuleEngineTester.RuleEngine.WeaklyTyped;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace Tests.weak_typed;

[TestClass]
public class ParserUnitTests
{
    private ILogger _logger;
    private readonly decimal PremiumDiscount = 10;
    private readonly decimal AmountDiscount = 5;

    [TestInitialize]
    public void Initialize()
    {
        // Setup logger
        ILogger logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(); // Example: Add console logging
        }).CreateLogger("");
      
        // Common setup logic
        _logger = logger;
    }

    // Generate JSON schema
    public JSchema GenerateBuyerSchema()
    {
        JSchemaGenerator generator = new JSchemaGenerator();
        return generator.Generate(typeof(Buyer));
    }

    public LsRuleVisitor GetLsRuleVisitor(string filename, JSchema schema)
    {
        // Read rule file content
        var ruleFile = File.ReadAllText("applyDiscount.rl");
        _logger.LogInformation(ruleFile);
        var inputStream = new AntlrInputStream(ruleFile);
        var lexer = new RulesLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);
        var parser = new RulesParser(tokenStream);

        var tree = parser.ruleFile(); // Start rule
        var visitor = new LsRuleVisitor(schema, _logger);
        visitor.Visit(tree);
        return visitor;
    }

    [TestMethod]
    public void TestCustomerEligibleWithAmountAndPremiumDiscount()
    {
        Initialize();
        var visitor = GetLsRuleVisitor("applyDiscount.rl", GenerateBuyerSchema());
        var rules = visitor.ParsedRules;

        var jsonData = GetTestData(180);

        Rules rls = new Rules(_logger);
        rls.AddRange(rules.ToList());

        var result = rls.ExecuteRules(jsonData);
        Assert.IsTrue(rls.RuleApplied);

        _logger.LogInformation(jsonData);
        Assert.IsTrue(rls.RuleApplied);
        JProperty property = result!.Properties().FirstOrDefault(f => f.Name == "Discount")!;
        JProperty totalProperty = result.Properties().FirstOrDefault(f => f.Name == "Total")!;
        JProperty premiumlProperty = result.Properties().FirstOrDefault(f => f.Name == "Total")!;
        Assert.IsTrue((decimal)property.Value == PremiumDiscount+AmountDiscount);
        Assert.IsTrue((decimal)totalProperty.Value == 153);
        Assert.IsTrue((bool)premiumlProperty.Value);
    }


    [TestMethod]
    public void TestCustomerEligibleWithPremiumDiscount()
    {
        Initialize();
        var visitor = GetLsRuleVisitor("applyDiscount.rl", GenerateBuyerSchema());
        var rules = visitor.ParsedRules;
        var jsonData = GetTestData(130);


        Rules rls = new Rules(_logger);
        rls.AddRange(rules.ToList());

        var result = rls.ExecuteRules(jsonData);
        //Assert.IsTrue(rls.RuleApplied);

        _logger.LogInformation(jsonData);

        JProperty property = result!.Properties().FirstOrDefault(f => f.Name == "Discount")!;
        JProperty totalProperty = result.Properties().FirstOrDefault(f => f.Name == "Total")!;
        JProperty purchaseAmountProperty = result.Properties().FirstOrDefault(f => f.Name == "PurchaseAmount")!;

        JProperty premiumlProperty = result.Properties().FirstOrDefault(f => f.Name == "Total")!;
        Assert.IsTrue((decimal)property.Value == PremiumDiscount);
        Assert.IsTrue((decimal)totalProperty.Value == CalculateDiscount((decimal)purchaseAmountProperty, (decimal)property));
    }

    [TestMethod]
    public void TestCustomerEligibleWithAmountDiscount()
    {
        Initialize();
        var visitor = GetLsRuleVisitor("applyDiscount.rl", GenerateBuyerSchema());
        var rules = visitor.ParsedRules;
        var jsonData = GetTestDataNoPremium(200);


        Rules rls = new Rules(_logger);
        rls.AddRange(rules.ToList());

        var result = rls.ExecuteRules(jsonData);
        //Assert.IsTrue(rls.RuleApplied);

        _logger.LogInformation(jsonData);

        JProperty property = result!.Properties().FirstOrDefault(f => f.Name == "Discount")!;
        JProperty totalProperty = result.Properties().FirstOrDefault(f => f.Name == "Total")!;
        JProperty purchaseAmountProperty = result.Properties().FirstOrDefault(f => f.Name == "PurchaseAmount")!;

        JProperty premiumlProperty = result.Properties().FirstOrDefault(f => f.Name == "IsPremium")!;
        Assert.IsTrue((decimal)property.Value == AmountDiscount);
        Assert.IsTrue((bool)premiumlProperty.Value == false);
        Assert.IsTrue((decimal)totalProperty.Value == CalculateDiscount((decimal)purchaseAmountProperty, (decimal)property));
    }

    private static decimal CalculateDiscount(decimal totalAmount, decimal discount) => totalAmount * (1 - discount / 100);

    private static string GetTestData(decimal amount)
    {
        var buyer = new Buyer
        {
            CustomerSince = new DateTime(1999, 5, 10),
            PurchaseAmount = amount,
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

    private static string GetTestDataNoPremium(decimal amount)
    {
        var buyer = new Buyer
        {
            CustomerSince = new DateTime(2024, 5, 10),
            PurchaseAmount = amount,
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
