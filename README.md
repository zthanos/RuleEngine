Rule Engine
===========

The Rule Engine is a powerful and flexible library designed to simplify the implementation of rule-based systems in C#. Whether you need to enforce business logic, validation rules, or decision-making processes, this rule engine provides a straightforward and extensible solution.

Table of Contents
-----------------

-   [Features](https://chat.openai.com/c/812b32ca-7250-48e7-b51e-3132cf0f36fa#features)
-   [Getting Started](https://chat.openai.com/c/812b32ca-7250-48e7-b51e-3132cf0f36fa#getting-started)
    -   [Prerequisites](https://chat.openai.com/c/812b32ca-7250-48e7-b51e-3132cf0f36fa#prerequisites)
    -   [Installation](https://chat.openai.com/c/812b32ca-7250-48e7-b51e-3132cf0f36fa#installation)
    -   [Usage](https://chat.openai.com/c/812b32ca-7250-48e7-b51e-3132cf0f36fa#usage)
-   [Examples](https://chat.openai.com/c/812b32ca-7250-48e7-b51e-3132cf0f36fa#examples)
-   [Supported Operations](https://chat.openai.com/c/812b32ca-7250-48e7-b51e-3132cf0f36fa#supported-operations)
-   [Advanced Features](https://chat.openai.com/c/812b32ca-7250-48e7-b51e-3132cf0f36fa#advanced-features)
-   [Contributing](https://chat.openai.com/c/812b32ca-7250-48e7-b51e-3132cf0f36fa#contributing)
-   [License](https://chat.openai.com/c/812b32ca-7250-48e7-b51e-3132cf0f36fa#license)

Features
--------

-   Rule-Based Logic: Define rules using a simple and expressive syntax to capture complex conditions and actions.
-   Extensibility: Easily extend the rule engine to accommodate custom operators, functions, and conditions tailored to your specific requirements.
-   Support for Complex Structures: Handle intricate rule structures, including composite conditions and nested rules, to model diverse decision-making scenarios.
-   Type-Agnostic: The rule engine is designed to be type-agnostic, allowing you to apply rules to various types of objects by implementing the IRuleApplicable interface.
-   Forward Chaining: Implement forward chaining logic for dynamic rule evaluation and decision-making processes.
-   Performance Optimization: Optimized for handling complex rules and large datasets efficiently.

Getting Started
---------------

### Prerequisites

-   .NET Core 3.1 or higher
-   Familiarity with C# and object-oriented programming

### Installation

Clone the repository and follow the provided installation steps.

### Usage

#### Sample Usage

```csharp

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
      .InitCondition("CustomerSince", ConditionType.LessThanOrEquals, typeof(DateTime), yearsAsCustomer)
      .Build();

  RuleCondition premiumDiscount = RuleCondition
      .CreateBuilder(logger)
      .InitCondition("IsPremium", ConditionType.Equals, typeof(bool), true)
      .Build();

  RuleCondition amountDiscount = RuleCondition
      .CreateBuilder(logger)
      .InitCondition("PurchaseAmount", ConditionType.GreaterThan, typeof(decimal), 150)
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
```

### Supported Operations

Explore a variety of supported operations, including comparison operators, null checks, and more. See the Supported Operations section in the README for a detailed list.

## Supported Operations

| Operation         | Aliases                    |
| ----------------- | -------------------------- |
| GreaterThanOrEquals | `>=`, `GreaterThanOrEquals`, `Greater Than Or Equals` |
| LessThanOrEquals    | `<=`, `LessThanOrEquals`, `Less Than Or Equals`    |
| LessThan            | `<`, `LessThan`, `Less Than`                        |
| GreaterThan         | `>`, `GreaterThan`, `Greater Than`                   |
| Equals              | `==`, `Equals`                                      |
| NotEquals           | `!=`, `NotEquals`, `Not Equals`                      |
| NotNull             | `isNotNull`, `is not Null`                          |
| isNull              | `is null`, `isNull`                                 |
| NotEmpty            | `notEmpty`, `not Empty`                             |
| Empty               | `Empty`                                             |

Advanced Features
-----------------

-   Custom Operators: Guide on how to implement custom operators.
-   Nested Rules: Examples of implementing nested and composite rules.
-   (Add any other advanced features here)

Contributing
------------

Feel free to contribute to the project by forking the repository and submitting pull requests.

License
-------

MIT License

Copyright (c) 2024 zthanos

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.