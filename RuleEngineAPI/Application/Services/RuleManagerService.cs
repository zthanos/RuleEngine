using Antlr4.Runtime;
using LsRuleEngine.Interfaces;
using LsRuleEngine.Parser;
using Newtonsoft.Json.Schema;
using RuleEngineAPI.Application.Interfaces;



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
}
