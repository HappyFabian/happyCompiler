using System.Collections.Generic;
using System.Linq.Expressions;
using parSEER.Interpretative.Values;
using parSEER.Parsing;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Tree.Sentences;
using parSEER.Semantics.Tree.Statements;
using parSEER.Semantics.Types;

namespace parSEER
{
    public class ifStatement : statementNode
    {
        public expressionNode ifCondition;
        public List<statementNode> TrueStatementNodes;
        public List<statementNode> FalseStatementNodes;

        public override nodeType EvaluateSemantics()
        {

            if (ifCondition.EvaluateTypes() is boolType)
            {
                contextTable.instance.addNewContext();
                foreach (var trueStatementNode in TrueStatementNodes)
                {
                    trueStatementNode.EvaluateSemantics();
                }

                contextTable.instance.removeContext();

                contextTable.instance.addNewContext();
                foreach (var falseStatementNode in FalseStatementNodes)
                {
                    falseStatementNode.EvaluateSemantics();
                }

                contextTable.instance.removeContext();

                return null;

            }

            exceptionMaster.throwException(011);
            return null;
        }

        public override void compile()
        {
            if (ifCondition.EvaluateTypes() is boolType)
            {
                var conditionInterpreted = ifCondition.Interpret();

                if ((conditionInterpreted as boolValue).Value)
                {
                    contextTable.instance.addNewContext();
                    foreach (var trueStatementNode in TrueStatementNodes)
                    {
                        trueStatementNode.compile();
                    }
                    contextTable.instance.removeContext();
                }
                else
                {
                    contextTable.instance.addNewContext();
                    foreach (var falseStatementNode in FalseStatementNodes)
                    {
                        falseStatementNode.compile();
                    }
                    contextTable.instance.removeContext();
                }
            }
        }

    }
}