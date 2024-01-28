using Antlr4.Runtime;
using LsRuleEngine.Interfaces;
using LsRuleEngine.Parser;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using RuleEngineAPI.Application.Interfaces;
using RuleEngineAPI.Services;



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
    public string  ExecuteRules(string jsonData, IEnumerable<RuleItem> rules)
    {
        if (string.IsNullOrEmpty(jsonData) || rules == null)
        {
            throw new ArgumentException("Invalid input data");
        }

        var rulesExecutor = new LsRuleEngine.Rules(_logger);
        foreach (var rule in rules)
        {
            var schema = JSchema.Parse(rule.JsonSchema);
            var rls = ParseRules(rule.RuleContent, schema);
            rulesExecutor.AddRange(rls.ToList());
        }
        try
        {
            JObject? jobj = rulesExecutor.ExecuteRules(jsonData);
            return jobj?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while Executing rule:{ex}",ex.ToString());
            throw;
        }
    }
}
