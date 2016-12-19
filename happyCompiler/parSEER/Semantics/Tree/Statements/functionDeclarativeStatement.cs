using System;
using System.Collections.Generic;
using parSEER.Parsing;
using parSEER.Parsing.Metadata;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Tree.Expression.literalNodes;
using parSEER.Semantics.Tree.Sentences;
using parSEER.Semantics.Tree.Statements;
using parSEER.Semantics.Types;

namespace parSEER
{
    public class functionDeclarativeStatement : statementNode
    {
        public expressionNode functionName { get; set; }
        public List<statementNode> parametersDefined { get; set; }
        public nodeType returnType { get; set; }
        public List<statementNode> scope { get; set; }
        public override nodeType EvaluateSemantics()
        {
            return null;
        }

        public override void compile()
        {
            contextTable.instance.addNewContext();
            foreach (var statementNode in parametersDefined)
            {
                statementNode.compile();
            }


            contextTable.instance.saveFunction((functionName as idNode).Name, this);
            contextTable.instance.removeContext();
        }
    }
}