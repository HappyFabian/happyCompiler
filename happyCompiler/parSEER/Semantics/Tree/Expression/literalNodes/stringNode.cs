using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Expression.literalNodes
{
    class stringNode : expressionNode
    {
        public string Value { get; set; }

        public override nodeType EvaluateTypes()
        {
            return new stringType();
        }

        public override nodeValue Interpret()
        {
            return new stringValue{Value = Value};
        }
    }
}
