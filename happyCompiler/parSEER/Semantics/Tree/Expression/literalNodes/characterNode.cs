using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Expression.literalNodes
{
    class characterNode : expressionNode
    {
        public char Value { get; set; }
        public override nodeType EvaluateTypes()
        {
            return new characterType();
        }

        public override nodeValue Interpret()
        {
            return new charValue {Value = Value};
        }
    }
}
