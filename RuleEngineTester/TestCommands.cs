using Cocona;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using RuleEngineTester.RuleEngine;
using RuleEngineTester.RuleEngine.Conditions;
using RuleEngineTester.RuleEngine.Data;
using RuleEngineTester.RuleEngine.Parser.Json;
using RuleEngineTester.RuleEngine.Parser.PlainText;
using RuleEngineTester.RuleEngine.Rule;
using RuleEngineTester.RuleEngine.Rule.Interfaces;
using RuleEngineTester.RuleEngine.WeaklyTyped;
using System;
using System.Text.Json.Nodes;

namespace RuleEngineTester
{

    public class TestCommands
    {

        private readonly ILogger<TestCommands> _logger;

        public TestCommands(ILogger<TestCommands> logger)
        {
            _logger = logger;
        }


        [Ignore]
        public static void ValidateCustomer()
        {
            //CSVFileConditionParser conditionParser = new CSVFileConditionParser("customer_rules.csv", "|");
            //var csv_rules = conditionParser.Parse();
            //var rules = new CustomerRules<Customer>();
            //var customer = new Customer();
            //customer.Name = "thanos";
            //customer.Address = "Ioanninon";
            ////customer.HomeAddress = "Ioanninon";
            //customer.City = "korydallos";
            //customer.Region = "region";
            //customer.PostalCode = "18122";
            //customer.Country = "greece";
            //customer.Phone = "645455444";
            //customer.Email = "asda@asdas.com";
            //customer.Age = 48;
            //rules.AddRule(csv_rules);
            //rules.ApplyRules(customer);


            //var conditions = new List<Condition>();
            //foreach (var csvRule in csv_rules)
            //{

            //    Console.WriteLine(JsonConvert.SerializeObject(csvRule));

            //}

            //CustomerRules<Customer> rules = new CustomerRules<Customer>(csv_rules.ToArray());
            //var customer = new Customer();
            //customer.Name = "thanos";
            //customer.Age = 48;

            //rules.ApplyRules(customer);
            //// Display the result
            //Console.WriteLine($"Customer Name: {customer.Name}");
            //Console.WriteLine($"Customer Age: {customer.Age}");
            //Console.WriteLine($"Is KYC Valid: {customer.IsKYCValid}");
        }

        [Command("validateCustomer")]
        public static void ValidateJsonParser()
        {
            var customer = TestData.GetCustomer();
            Console.WriteLine(JsonConvert.SerializeObject(customer));
            JsonRuleParser<IRuleApplicable> parser = new();
            var rules = parser.Parse("customer_rules.json");
            foreach (var rule in rules)
            {
                if (rule is LsRule<Customer> lsRule)
                {
                    lsRule.ApplyRules(customer);
                }
                //rule.ApplyRules(customer);
            }
            Console.WriteLine(JsonConvert.SerializeObject(customer));

        }

        /// <summary>
        ///  the customer will be eligible (IsEligible will be set to true) under the following conditions:
        ///
        ///        The customer's age is greater than or equal to 18.
        ///  Either:
        ///         The customer's income is greater than 50,000.
        ///  And either:
        ///         The customer is employed(IsEmployed is true).
        ///         The customer has a valid credit history(HasValidCreditHistory is true).
        /// </summary>
        /// <returns></returns>
        [Command("isCustomerEligible")]
        public void IsEligible()
        {
            FinancialCustomer financialCustomer = TestData.GetFinancialCustomer();
            Console.WriteLine(JsonConvert.SerializeObject(financialCustomer));
            JsonRuleParser<IRuleApplicable> parser = new();
            var rules = parser.Parse("customer_subcondition.json");
            foreach (var rule in rules)
            {
                if (rule is LsRule<FinancialCustomer> lsRule)
                {
                    lsRule.ApplyRules(financialCustomer);
                }
            }
            Console.WriteLine(JsonConvert.SerializeObject(financialCustomer));
        }
        [Command("plainText")]
        public void ValidateParser()
        {
            var customer = TestData.GetCustomer();

            var data = File.ReadAllText("plain_rules.txt");
            var rules = PlainTextRules<IRuleApplicable>.Parse(data);
            foreach (var rule in rules)
            {
                if (rule is LsRule<Customer> lsRule)
                {
                    lsRule.ApplyRules(customer);
                }
            }
            Console.WriteLine(JsonConvert.SerializeObject(customer));

        }

        [Command("weaklyTyped")]
        public void Weak()
        {
            var customer = TestData.GetCustomer();
            JSchemaGenerator generator = new();
            JSchema schema = generator.Generate(typeof(Customer));
            var json = JsonConvert.SerializeObject(customer);

           var cond1 = RuleEngine.WeaklyTyped.RuleCondition
                .CreateBuilder(_logger)
                .InitCondition("Age", ConditionType.Null, customer.GetType(), 18)
                .Build();
            Int64 age = 19;
            var cond2 = RuleEngine.WeaklyTyped.RuleCondition
                .CreateBuilder(_logger)
                .InitCondition("Age", ConditionType.GreaterThan, customer.GetType(), 18)
                .Build();
            var cond = RuleEngine.WeaklyTyped.RuleCondition
                .CreateBuilder(_logger)
                .InitCondition("Email", ConditionType.NotNull, typeof(string), null)
                .AndCondition("Email", ConditionType.NotEmpty, typeof(string), null)
                .OrCondition("Email", ConditionType.NotEquals, typeof(string), "1")
                .Build();
            // var data = File.ReadAllText("plain_rules.txt");
            // var ruleEngine = new RuleEngine.WeaklyTyped.Rules(_logger);

            //List<RuleCondition> conditions = new List<RuleCondition>();
            //Func<JObject, bool> compiledLambda = ConditionEvaluator.EvaluateConditions(schema, json, cond.Conditions);

            //conditions.Add(cond);
            var r = RuleEngine.WeaklyTyped.Rule
                .CreateBuilder(_logger)
                .ForType(nameof(Customer), schema)
                .AddCondition(cond)
                .AddCondition(cond2)
                .Build();

            r.ApplyRule(json);
        }
    }
}
