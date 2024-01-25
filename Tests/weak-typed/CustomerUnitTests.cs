using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;


using RuleEngineTester.RuleEngine;
using RuleEngineTester.RuleEngine.Data;
using RuleEngineTester.RuleEngine.WeaklyTyped;
using RuleEngineTester.RuleEngine.Conditions;
using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json.Linq;

namespace Tests.weak_typed;


[TestClass]
public class CustomerUnitTests
{
    [TestMethod]
    public void TestCustomerIsKYCValid()
    {
        ILogger logger = LoggerFactory.Create(builder =>
        {
            // Configure logging options here if needed
            builder.AddConsole(); // Example: Add console logging
        }).CreateLogger("");

        var customer = TestData.GetCustomer();
        JSchemaGenerator generator = new();
        JSchema schema = generator.Generate(typeof(Customer));
        var json = JsonConvert.SerializeObject(customer);

        var cond1 = RuleCondition
             .CreateBuilder(logger)
             .InitCondition("Age", ConditionType.Null, 18)
             .Build();
        var cond2 = RuleCondition
            .CreateBuilder(logger)
            .InitCondition("Age", ConditionType.GreaterThan, 18)
            .Build();
        var cond = RuleCondition
            .CreateBuilder(logger)
            .InitCondition("Email", ConditionType.NotNull, null)
            .AndCondition("Email", ConditionType.NotEmpty, null)
            .OrCondition("Email", ConditionType.NotEquals, "1")
            .Build();
        // var data = File.ReadAllText("plain_rules.txt");
        // var ruleEngine = new RuleEngine.WeaklyTyped.Rules(_logger);

        //List<RuleCondition> conditions = new List<RuleCondition>();
        //Func<JObject, bool> compiledLambda = ConditionEvaluator.EvaluateConditions(schema, json, cond.Conditions);

        //conditions.Add(cond);
        var r = Rule
            .CreateBuilder(logger)
            .ForType(nameof(Customer), schema)
            .AddCondition(cond)
            .AddCondition(cond2)
            .AddAction(new RuleAction("IsKYCValid", true))
            .Build();

        RuleExecutionResult result = r.ApplyRule(json);
        JProperty property = result.Target.Properties().FirstOrDefault(f => f.Name == "IsKYCValid")!;

        Assert.IsTrue((bool)property.Value);
    }



    //Consider a set of rules related to discounts in an e-commerce system:

    //Rule: If total purchase amount > $100, apply a 10% discount.
    //Rule: If user is a premium member, apply an additional 5% discount.
    //If a user's total purchase amount is $120 and they are a premium member, forward chaining would:

    //Start with the fact: Total purchase amount is $120.
    //Rule 1 is triggered and applies a 10% discount.
    //Rule 2 is triggered(because the user is a premium member) and applies an additional 5% discount.
    [TestMethod]
    public void CalculateDiscount()
    {
        ILogger logger = LoggerFactory.Create(builder =>
        {
            // Configure logging options here if needed
            builder.AddConsole(); // Example: Add console logging
        }).CreateLogger("");
        JSchemaGenerator generator = new();
        JSchema schema = generator.Generate(typeof(Buyer));


        var buyer = new Buyer
        {
            CustomerSince = new DateTime(1999, 5, 10),
            PurchaseAmount = 180,
            Name = "Thanos",
            Total = 0,
            Discount = 0,
            IsPremium = false
        };
        var json = JsonConvert.SerializeObject(buyer);
        DateTime yearsAsCustomer = DateTime.Now.AddYears(-5);

        RuleCondition ispremium = RuleCondition
            .CreateBuilder(logger)
            .InitCondition("CustomerSince", ConditionType.LessThanOrEquals, yearsAsCustomer)
            .Build();

        RuleCondition premiumDiscount = RuleCondition
            .CreateBuilder(logger)
            .InitCondition("IsPremium", ConditionType.Equals, true)
            .Build();

        RuleCondition amountDiscount = RuleCondition
            .CreateBuilder(logger)
            .InitCondition("PurchaseAmount", ConditionType.GreaterThan, 150)
            .Build();

        var amountRule = Rule.CreateBuilder(logger)
          .ForType(nameof(Buyer), schema)
          .AddCondition(amountDiscount)
          .AddAction(new RuleAction("Discount", "Discount + 10"))
          .AddAction(new RuleAction("Total", "PurchaseAmount * (1 - Discount / 100)"))
          .Build();

        var isPremiumRule = Rule.CreateBuilder(logger)
          .ForType(nameof(Buyer), schema)
          .AddCondition(ispremium)
          .AddAction(new RuleAction("IsPremium", true))
          .Build();

        var premiumDiscountRule = Rule.CreateBuilder(logger)
          .ForType(nameof(Buyer), schema)
          .AddCondition(ispremium)
          .AddAction(new RuleAction("Discount", "Discount + 5"))
          .AddAction(new RuleAction("Total", "PurchaseAmount * (1 - Discount / 100)"))
          .Build();
        Rules rls = new(logger);
        rls.Add(amountRule);
        rls.Add(isPremiumRule);
        rls.Add(premiumDiscountRule);

        var result = rls.ExecuteRules(json);
        Assert.IsTrue(rls.RuleApplied);
        JProperty property = result!.Properties().FirstOrDefault(f => f.Name == "Discount")!;
        JProperty totalProperty = result.Properties().FirstOrDefault(f => f.Name == "Total")!;
        JProperty premiumlProperty = result.Properties().FirstOrDefault(f => f.Name == "Total")!;
        Assert.IsTrue((decimal)property.Value == 15);
        Assert.IsTrue((decimal)totalProperty.Value == 153);
        Assert.IsTrue((bool)premiumlProperty.Value);


    }

}

//.AddAction(new RuleAction("PurchaseAmount", "PurchaseAmount * 0.9")) // 10% discount

public class Buyer
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string HomeAddress { get; set; }
    public string Email { get; set; }
    public DateTime CustomerSince { get; set; }
    public decimal PurchaseAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public bool IsPremium { get; set; }

}