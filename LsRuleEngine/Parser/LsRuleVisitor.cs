using Antlr4.Runtime.Misc;
using LsRuleEngine.Builders;
using LsRuleEngine.Interfaces;
using LsRuleEngine.Resolvers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Schema;

namespace LsRuleEngine.Parser;

public class LsRuleVisitor(JSchema schema, ILogger logger) : RulesBaseVisitor<object>
{
    private readonly JSchema _schema = schema;
    private readonly ILogger _logger = logger;

    public string? AppliesTo { get; private set; }
    public string? RuleName { get; private set; }
    private ConditionBuilder? _active_conditionbuilder;
    private readonly List<RuleBuilder> _rules = [];

    public IEnumerable<IRule> ParsedRules => _rules.Select(s => s.Build());

    private RuleBuilder ActiveRuleBuilder => _rules.Last();

    public override object VisitRuleFile(RulesParser.RuleFileContext context)
    {
        // Your logic for when visiting the ruleFile
        return base.VisitRuleFile(context);
    }


    public override object VisitRuleName([NotNull] RulesParser.RuleNameContext context)
    {
        var ruleName = context.STRING().GetText().Trim('\"');
        _rules.Add(LsRule.CreateBuilder(ruleName, _logger));

        // Your logic here
        RuleName = context.STRING().GetText().Trim('\"');
        RuleName = RuleName.Trim('\"');
        return base.VisitRuleName(context);
    }

    public override object VisitAppliesTo([NotNull] RulesParser.AppliesToContext context)
    {
        ActiveRuleBuilder.ForType(context.STRING().GetText().Trim('\"'), _schema);

        AppliesTo = context.STRING().GetText();
        AppliesTo = AppliesTo.Trim('\"');
        return base.VisitAppliesTo(context);
    }

    public override object VisitBasicCondition([NotNull] RulesParser.BasicConditionContext context)
    {
        // Initialize a variable to hold the condition representation
        string condition = "";

        if (context.ID() != null && context.comparator() != null && context.value() != null)
        {
            // Handle the ID comparator value structure
            string leftOperand = context.ID().GetText();
            string comparator = context.comparator().GetText();
            var rightOperand = Resolver.OperandTypeResolver(_schema, leftOperand, context.value().GetText());

            condition = $"{leftOperand} {comparator} {rightOperand}";
            _logger.LogInformation("Visit Base Condition: {condition}", condition);

            _active_conditionbuilder?.AndCondition(leftOperand, Resolver.ResolveType(comparator), rightOperand);
        }
        else if (context.ID() != null && context.unaryOperator() != null)
        {
            // Handle the ID unaryOperator structure
            string leftOperand = context.ID().GetText();
            string unaryOperator = context.unaryOperator().GetText();
            if (context.value != null)
            {
                var rightOperand = Resolver.OperandTypeResolver(_schema, leftOperand, context.value()?.GetText()!);
                _active_conditionbuilder?.AndCondition(leftOperand, Resolver.ResolveType(unaryOperator), rightOperand);
            }
            else
            {
                _active_conditionbuilder?.AndCondition(leftOperand, Resolver.ResolveType(unaryOperator));
            }
            condition = $"{leftOperand} {unaryOperator}";
            _logger.LogInformation("Visit Base Condition: {condition}", condition);
        }

        return base.VisitBasicCondition(context);
    }

    public override object VisitConditionBlock([NotNull] RulesParser.ConditionBlockContext context)
    {
        foreach (var logicalExpression in context.logicalExpression())
        {
            _active_conditionbuilder = RuleCondition.CreateBuilder(_logger);
            ProcessLogicalExpression(logicalExpression, ref _active_conditionbuilder);
            ActiveRuleBuilder.AddCondition(_active_conditionbuilder.Build());
        }

        return base.VisitConditionBlock(context);
    }

    private void ProcessLogicalExpression(RulesParser.LogicalExpressionContext context, ref ConditionBuilder builder)
    {
        foreach (var expression in context.expression())
        {
            var basicConditionContext = expression.basicCondition();
            if (basicConditionContext != null)
            {
                var conditionResult = VisitBasicCondition(basicConditionContext);
            }
            var logicalExpression = expression.logicalExpression();
            if (logicalExpression != null)
            {
                ProcessLogicalExpression(logicalExpression, ref builder);
            }
        }
    }

    public override object VisitAction(RulesParser.ActionContext context)
    {
        string variableName = context.ID().GetText().Trim('\"');
        //var mathExpressionContext = context.mathExpression();
        string expressionAsString = context.mathExpression().GetText().Trim('\"');
        ActiveRuleBuilder.AddAction(new RuleAction(variableName, expressionAsString));
        _logger.LogInformation("Variable : {variableName}, expression {expressionAsString}", variableName, expressionAsString);

        return base.VisitAction(context);
    }
}
