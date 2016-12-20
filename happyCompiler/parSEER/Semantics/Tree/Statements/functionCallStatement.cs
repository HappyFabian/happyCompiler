using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Sentences
{
    public class functionCallStatement : statementNode
    {
        public expressionNode functionCalled { get; set; }

        public override nodeType EvaluateSemantics()
        {
            throw new NotImplementedException();
        }

        public override void compile()
        {
            functionCalled.Interpret();
        }
    }
}
