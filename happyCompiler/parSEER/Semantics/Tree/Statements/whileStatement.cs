using System;
using System.Collections.Generic;
using parSEER.Interpretative.Values;
using parSEER.Parsing;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Tree.Sentences;
using parSEER.Semantics.Types;

namespace parSEER
{
    public class whileStatement : statementNode
    {
        public expressionNode conditionalExpression { get; set; }
        public List<statementNode> whileStatementList { get; set; }

        public contextLayer interpretedContextLayer;

        public override nodeType EvaluateSemantics()
        {
            if (conditionalExpression.EvaluateTypes().GetType() == typeof(boolType))
            {

                contextTable.instance.addNewContext();
                foreach (var statementNode in whileStatementList)
                {
                    statementNode.EvaluateSemantics();
                }

                interpretedContextLayer = contextTable.instance.getCurrentDepthLayer();
                contextTable.instance.removeContext();

                return null;
            }

            exceptionMaster.throwException(009);
            return null;
        }

        public override void compile()
        {
            contextTable.instance.addNewContext();
          
            if (conditionalExpression.EvaluateTypes().GetType() == typeof(boolType))
            {
                var conditionInterpreted = conditionalExpression.Interpret();
                while ((conditionInterpreted as boolValue).Value)
                {
                    foreach (var statementNode in whileStatementList)
                    {
                        statementNode.compile();
                    }
                    conditionInterpreted = conditionalExpression.Interpret();
                }

            }
            else
            {
                throw new Exception("Invalid type of Conditional Expression");
            }
            contextTable.instance.removeContext();
        }
    }
}