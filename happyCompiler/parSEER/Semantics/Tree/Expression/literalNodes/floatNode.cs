using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Semantics.Types;
using parSEER.Semantics.Types.literalTypes;

namespace parSEER.Semantics.Tree.Expression.literalNodes
{
    public class floatNode : expressionNode
    {
        public float Value { get; set; }

        public override nodeType EvaluateTypes()
        {
            return new floatType();
        }

        public override nodeValue Interpret()
        {
            return new floatValue {Value = Value};
        }
    }
}
