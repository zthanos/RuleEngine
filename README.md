# Rule Engine

The Rule Engine is a powerful and flexible library designed to simplify the implementation of rule-based systems in C#. Whether you need to enforce business logic, validation rules, or decision-making processes, this rule engine provides a straightforward and extensible solution.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Examples](#examples)
- [Supported Operations](#supported-operations)
- [Contributing](#contributing)
- [License](#license)
## Features 

- **Rule-Based Logic:** Define rules using a simple and expressive syntax to capture complex conditions and actions.
- **Extensibility:** Easily extend the rule engine to accommodate custom operators, functions, and conditions tailored to your specific requirements.
- **Support for Complex Structures:**  Handle intricate rule structures, including composite conditions and nested rules, to model diverse decision-making scenarios.
- **Type-Agnostic:**  The rule engine is designed to be type-agnostic, allowing you to apply rules to various types of objects by implementing the IRuleApplicable interface.

## Getting Started
1. **Installation:** Clone the repository and follow the provided installation steps.

1. **Sample Usage**
```csharp
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
```
3. **Supported Operations:** Explore a variety of supported operations, including comparison operators, null checks, and more. See the Supported Operations section in the README for a detailed list.

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



## Rule format 

```plaintext
Rule Name: KYCValidationRule
Applies to: RuleEngineTester.RuleEngine.Customer

When:
  - Age > 18
  - HomeAddress isNotNull
  - Email isNotNull and Email notEmpty or Email != 1
  - Phone isNotNull and Phone notEmpty

Then:
  - Set IsKYCValid to true

Rule End
```

## Contributing

Feel free to contribute to the project by [forking the repository](https://github.com/yourusername/rule-engine.git) and submitting pull requests.

## Licence

```bash
# Clone the repository
git clone https://github.com/yourusername/rule-engine.git

# Navigate to the project directory
cd rule-engine

# [Additional installation steps, if any]
