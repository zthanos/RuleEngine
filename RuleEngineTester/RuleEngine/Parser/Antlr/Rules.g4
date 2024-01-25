grammar Rules;

// Parser Rules
ruleFile: prog+ ;
prog: ruleName appliesTo when then;

ruleName: 'Rule Name:' STRING ;
appliesTo: 'Applies to:' STRING ;
when: 'When:' conditionBlock* ;
then: 'Then:'  actions ;

conditionBlock: (TAB logicalExpression)+ ;
conditions: logicalExpression ;
logicalExpression: expression (logicalOperator expression)* ;
expression: basicCondition | '(' logicalExpression ')' ;
basicCondition: ID comparator value | ID unaryOperator value?;
value: ID | NUMBER | STRING ;

unaryOperator: 'notEmpty' | 'Empty' | 'Equals' | 'GreaterThan' | 'LessThan' | 'isNull' | 'isNotNull';
logicalOperator: 'and' | 'or' | '&' | '|' ;

actions: action+ ;
action: 'Set' ID 'to' mathExpression ;
mathExpression: term (mathoperators term)* ;
term: ID | NUMBER | '(' mathExpression ')' | actionText ;
actionText: ID | NUMBER ;

// Lexer Rules
ID: [a-zA-Z_][a-zA-Z0-9_.]* ;
NUMBER: '0' | '-'?[1-9][0-9]* ;
STRING: '"' (~["\r\n])* '"' ;
NEWLINE: '\r'? '\n' ;
COMMENT: '--' ~[\r\n]* -> skip;
TAB: '& ' | '| ' ;
comparator: '>' | '<' | '==' | '!=' | '>=' | '<=' ;
mathoperators: '*' | '/' | '+' | '-'   ;
WS: [ \t\r\n]+ -> skip ; // skip spaces, tabs, newlines