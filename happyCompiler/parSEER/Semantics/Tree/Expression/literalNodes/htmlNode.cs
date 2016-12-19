using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexiCONOMICON;
using parSEER.Interpretative.Values;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Expression.literalNodes
{
    public class htmlNode : expressionNode
    {
        public string Value { get; set; }


        public override nodeType EvaluateTypes()
        {
            return new htmlType();
        }

        public override nodeValue Interpret()
        {
            return new htmlValue {Value = Value};
        }
    }
}
