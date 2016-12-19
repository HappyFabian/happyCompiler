using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Sentences
{
    public abstract class statementNode
    {
        public abstract nodeType EvaluateSemantics();

        public abstract void compile();

    }
}
