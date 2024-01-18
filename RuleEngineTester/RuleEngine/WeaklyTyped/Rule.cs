﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using RuleEngineTester.RuleEngine.ErrorHandling;

namespace RuleEngineTester.RuleEngine.WeaklyTyped;

public class Rule : IRule
{
    private readonly ILogger _logger;
    private JSchema _typeToApplyRule;
    private string _applyToType;
    private List<RuleCondition> _conditons = new();

    public Rule(ILogger logger)
    {
        _logger = logger;
    }
    public void SetType(string type, JSchema jsonSchema)
    {
        _applyToType = type;
        _typeToApplyRule = jsonSchema;
    }

    public void AddCondition(RuleCondition condition)
    {
        _conditons.Add(condition);
        
    }

    public void ApplyRule(string jsonData)
    {
        JObject jObj = JObject.Parse(jsonData);
        _logger.LogDebug($"Type : {_applyToType} \n Data: \n {jsonData}");
        JsonTextReader reader = new JsonTextReader(new StringReader(jsonData));
        JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(reader);
        validatingReader.Schema = _typeToApplyRule;
        validatingReader.ValidationEventHandler += (sender, args) =>
        {
            throw new RuleEngineException($"An error occurred while validating the JSON.\n{args.Message}");
        };
        // Deserialize using the validating reader
        JsonSerializer serializer = new JsonSerializer();
        var data = serializer.Deserialize<JObject>(validatingReader);
        if (data is null ) 
        {
            throw new RuleEngineException("Invalid Object.");
        }
        foreach ( RuleCondition condition in _conditons )
        {
            List<bool> results = [];
            var res = condition.Evaluate(data);
            results.Add(res);

            if (results.All(w => w == true))
            {
                // Condition is satisfied
                _logger.LogInformation($"Condition '{condition.ExpressionToExecute}' is satisfied.");
            }
            else
            {
                // Condition is not satisfied
                _logger.LogInformation($"Condition '{condition.Conditions.First().PropertyName}' is not satisfied.");
            }
        }
    }

    public JSchema GetApplyToType() => _typeToApplyRule;
    public string GetApplyToTypeName() => _applyToType;



    public static RuleBuilder CreateBuilder(ILogger logger) => new RuleBuilder(logger);
    public class RuleBuilder
    {
        private readonly ILogger _logger;
        private IRule _rule;
        public RuleBuilder(ILogger logger)
        {
            _logger = logger;
            _rule = new Rule(logger);
        }

        public RuleBuilder ForType(string type, JSchema jsonSchema)
        {
            _rule.SetType(type, jsonSchema);
            _logger.LogInformation($"Type: {type}");
            _logger.LogInformation(jsonSchema.ToString());
            return this;
        }

        public RuleBuilder WithName(string name)
        {
            return this;
        }
     
        public RuleBuilder AddCondition(RuleCondition ruleCondition)
        {
            _rule.AddCondition(ruleCondition);
            return this;
        }
        public IRule Build() => _rule;
    }
}
