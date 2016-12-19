using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Sentences
{
    class unknownStatement : statementNode
    {
        public override nodeType EvaluateSemantics()
        {
            throw new NotImplementedException();
        }

        public override void compile()
        {
            throw new NotImplementedException();
        }
    }
}
