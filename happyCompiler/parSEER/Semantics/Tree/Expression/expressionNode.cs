using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Expression
{
    public abstract class expressionNode
    {
        public abstract nodeType EvaluateTypes();
        public abstract nodeValue Interpret();

     
    }
}
