using Cocona;
using Newtonsoft.Json;
using RuleEngineTester.RuleEngine;
using RuleEngineTester.RuleEngine.Data;
using RuleEngineTester.RuleEngine.Parser;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RuleEngineTester
{

    public class TestCommands
    {
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
            JsonRuleParser parser = new();
            var rules = parser.Parse("customer_rules.json");
            foreach (var rule in rules)
            {
                rule.ApplyRules(customer);
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
            var customer = TestData.GetFinancialCustomer();
            Console.WriteLine(JsonConvert.SerializeObject(customer));
            JsonRuleParser parser = new();
            var rules = parser.Parse("customer_subcondition.json");
            foreach (var rule in rules)
            {
                rule.ApplyRules(customer);
            }
            Console.WriteLine(JsonConvert.SerializeObject(customer));
        }
        [Ignore]
        public void ValidateParser()
        {
            var data = File.ReadAllText("plain_rules.txt");
            var raw = new PlainTextRules(data);


        }
    }
}
