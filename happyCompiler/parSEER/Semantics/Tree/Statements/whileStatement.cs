using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }
    }
}