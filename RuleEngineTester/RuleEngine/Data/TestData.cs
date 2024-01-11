using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineTester.RuleEngine.Data
{
    public class TestData
    {
        public static Customer GetCustomer()
        {
            var customer = new Customer();
            customer.Name = "thanos";
            customer.Address = "Ioanninon";
            customer.HomeAddress = "Ioanninon";
            customer.City = "korydallos";
            customer.City = "korydallos";
            customer.Region = "region";
            customer.PostalCode = "18122";
            customer.Country = "greece";
            customer.Phone = "645455444";
            customer.Email = "asda@asdas.com";
            customer.Age = 48;
            return customer;

        }

        public static FinancialCustomer GetFinancialCustomer()
        {
            var customer = new FinancialCustomer();
            customer.Name = "thanos";
            customer.Phone = "645455444";
            customer.Email = "asda@asdas.com";
            customer.Age = 48;
            customer.Income = 51000;
            customer.HasValidCreditHistory = true;
            customer.IsEmployed = true;
            return customer;


        }
    }
}

