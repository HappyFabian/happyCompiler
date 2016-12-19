using parSEER.Interpretative.Values;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Types;

namespace parSEER
{
    public class numberNode : expressionNode
    {
        public int Value { get; set; }

        public override nodeType EvaluateTypes()
        {
            return new numberType();
        }

        public override nodeValue Interpret()
        {
            return new numberValue {Value = Value};
        }
    }
}