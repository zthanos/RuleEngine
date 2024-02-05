using Antlr4.Runtime;
using LsRuleEngine;
using LsRuleEngine.Interfaces;
using LsRuleEngine.Parser;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using RuleEngineAPI.Domain.Aggregates;
using RuleEngineAPI.Domain.Interfaces;



namespace RuleEngineAPI.Application.Services;

public class RuleManagerService(ILogger<RuleManagerService> logger) : IRuleManagerService
{
    private readonly ILogger<RuleManagerService> _logger = logger;

    public IEnumerable<IRule> ParseRules(string ruleContent, JSchema schema)
    {
        _logger.LogInformation("Parsing rules");

        var inputStream = new AntlrInputStream(ruleContent);
        var lexer = new RulesLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);
        var parser = new RulesParser(tokenStream);

        var tree = parser.ruleFile(); // Start rule
        var visitor = new LsRuleVisitor(schema, _logger);
        visitor.Visit(tree);

        return visitor.ParsedRules;
    }

    // Implementations for add and execute methods
    public RuleExecutionResults ExecuteRules(RuleSet ruleset, string jsonData)
    {
        //if (string.IsNullOrEmpty(jsonData) || Rulserules == null)
        //{
        //    throw new ArgumentException("Invalid input data");
        //}
        JSchema jSchema = JSchema.Parse(ruleset.Schema);
        var rulesExecutor = new Rules(_logger);
        var parsedRules = ParseRules(ruleset.Rules, jSchema);
        rulesExecutor.AddRange(parsedRules.ToList());
        string data = string.Copy(jsonData);
        var appliedRuleData = rulesExecutor.ExecuteRules(data);
        data = JsonConvert.SerializeObject(appliedRuleData);

        var executionResults = rulesExecutor.GetRuleExecutionResults();
        var conditionResults = executionResults.SelectMany(sm=> sm.ConditionResults).ToList();


        return  new(jsonData, data, rulesExecutor.RuleApplied, conditionResults);
        
    }


}
