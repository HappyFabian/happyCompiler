using System;
using System.Collections.Generic;
using parSEER.Interpretative.Values;
using parSEER.Parsing;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Tree.Sentences;
using parSEER.Semantics.Types;

namespace parSEER
{
    internal class doWhileStatement : statementNode
    {

        public expressionNode conditionalExpression { get; set; }
        public List<statementNode> dowhileStatementList { get; set; }

        public override nodeType EvaluateSemantics()
        {
            throw new System.NotImplementedException();
        }

        public override void compile()
        {
            contextTable.instance.addNewContext();

            if (conditionalExpression.EvaluateTypes().GetType() == typeof(boolType))
            {
                var conditionInterpreted = conditionalExpression.Interpret();
                do
                {
                    conditionInterpreted = conditionalExpression.Interpret();
                    foreach (var statementNode in dowhileStatementList)
                    {
                        if (contextTable.returnValueWas == null)
                        {
                            statementNode.compile();
                        }
                    }

                } while ((conditionInterpreted as boolValue).Value);
            }
            else
            {
                throw new Exception("Invalid type of Conditional Expression");
            }
            contextTable.instance.removeContext();
        }
    }
}