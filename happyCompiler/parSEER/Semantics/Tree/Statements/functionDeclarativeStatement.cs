using System.Collections.Generic;
using parSEER.Parsing;
using parSEER.Parsing.Metadata;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Tree.Sentences;
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
            throw new System.NotImplementedException();
        }
    }
}