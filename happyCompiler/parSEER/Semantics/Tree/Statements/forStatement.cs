using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Parsing;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Sentences
{
    class forStatement : statementNode
    {
        public statementNode declarationStatement { get; set; }
        public expressionNode comparativeExpression { get; set; }
        public statementNode assignationStatement { get; set; }

        public List<statementNode> forStatementsList { get; set; }



        public contextLayer interpretedContextLayer;
        public override nodeType EvaluateSemantics()
        {
            contextTable.instance.addNewContext();
            declarationStatement.EvaluateSemantics();
            assignationStatement.EvaluateSemantics();
            if (comparativeExpression.EvaluateTypes().GetType() == typeof(boolType))
            {
                foreach (var statementNode in forStatementsList)
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
            declarationStatement.compile();
            if (comparativeExpression.EvaluateTypes().GetType() == typeof(boolType))
            {
                var conditionInterpreted = comparativeExpression.Interpret();    
                while ((comparativeExpression.Interpret() as boolValue).Value)
                {
                    foreach (var statementNode in forStatementsList)
                    {
                        statementNode.compile();
                    }                  
                    assignationStatement.compile();
                }
                
            }
            contextTable.instance.removeContext();
        }
    }
}
