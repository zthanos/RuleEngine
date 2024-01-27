//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Rules.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="RulesParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
#pragma warning disable CS3021 // Type or member does not need a CLSCompliant attribute because the assembly does not have a CLSCompliant attribute
public interface IRulesVisitor<Result> : IParseTreeVisitor<Result> {
#pragma warning restore CS3021 // Type or member does not need a CLSCompliant attribute because the assembly does not have a CLSCompliant attribute
    /// <summary>
    /// Visit a parse tree produced by <see cref="RulesParser.ruleFile"/>.
    /// </summary>
    /// <param name="context">The parse tree.</param>
    /// <return>The visitor result.</return>
    Result VisitRuleFile([NotNull] RulesParser.RuleFileContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.prog"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProg([NotNull] RulesParser.ProgContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.ruleName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRuleName([NotNull] RulesParser.RuleNameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.appliesTo"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAppliesTo([NotNull] RulesParser.AppliesToContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.when"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhen([NotNull] RulesParser.WhenContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.then"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitThen([NotNull] RulesParser.ThenContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.conditionBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditionBlock([NotNull] RulesParser.ConditionBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.conditions"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditions([NotNull] RulesParser.ConditionsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.logicalExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLogicalExpression([NotNull] RulesParser.LogicalExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] RulesParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.basicCondition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBasicCondition([NotNull] RulesParser.BasicConditionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitValue([NotNull] RulesParser.ValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.unaryOperator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnaryOperator([NotNull] RulesParser.UnaryOperatorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.logicalOperator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLogicalOperator([NotNull] RulesParser.LogicalOperatorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.actions"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitActions([NotNull] RulesParser.ActionsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.action"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAction([NotNull] RulesParser.ActionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.mathExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMathExpression([NotNull] RulesParser.MathExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTerm([NotNull] RulesParser.TermContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.actionText"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitActionText([NotNull] RulesParser.ActionTextContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.comparator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparator([NotNull] RulesParser.ComparatorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="RulesParser.mathoperators"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMathoperators([NotNull] RulesParser.MathoperatorsContext context);
}
