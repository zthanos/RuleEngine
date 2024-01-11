using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuleEngineTester.RuleEngine;
using RuleEngineTester.RuleEngine.Evaluators;
using System;
using System.Collections.Generic;

namespace RuleEngineTester.Tests
{
    [TestClass]
    public class RuleEngineTests
    {
        [TestMethod]
        public void TestCompositeConditionEvaluator()
        {
            // Arrange
            var customer = new Customer { Age = 25, IsActive = true, Name = "John" };

            var condition1 = new Condition(1, "Age", 21, "GreaterThanOrEquals", "And");
            var condition2 = new Condition(2, "Name", "John", ConditionType.Equals, "And");
            var condition0 = new Condition(2, "Name", "John", "NotNull", "And");
            var subConditions = new List<Condition> { condition0, condition1, condition2 };

            var compositeCondition = new Condition(3, "", "", ConditionType.Composite, "And")
            {
                SubConditions = subConditions
            };

            var evaluatorFactory = new ConditionEvaluatorFactory<Customer>();
            var conditionEvaluator = evaluatorFactory.CreateConditionEvaluator(compositeCondition);

            var compositeConditionEvaluator = (CompositeConditionEvaluator<Customer>)conditionEvaluator;

            // Act
            bool result = compositeConditionEvaluator.Evaluate(customer);

            // Assert
            Assert.IsTrue(result, "Composite condition should evaluate to true for the provided customer.");
        }

        // You can add more tests for other conditions and scenarios
    }

    // Mock class for testing
    public class Customer
    {
        public int Age { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
    }
}
