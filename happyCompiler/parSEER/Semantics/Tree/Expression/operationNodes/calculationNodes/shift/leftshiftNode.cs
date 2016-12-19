using parSEER.Interpretative.Values;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Types;

namespace parSEER
{
    internal class leftshiftNode : bOperationNode
    {
        public override nodeType EvaluateTypes()
        {
            if (LeftOperand.EvaluateTypes().GetType() == typeof(numberNode) && RightOperand.EvaluateTypes().GetType() == typeof(numberNode))
            {
                return LeftOperand.EvaluateTypes();
            }

            exceptionMaster.throwException(009);
            return null;
        }

        public override nodeValue Interpret()
        {
            if (LeftOperand.EvaluateTypes().GetType() == RightOperand.EvaluateTypes().GetType())
            {
                var lOper = LeftOperand.Interpret();
                var rOper = RightOperand.Interpret();

                if (lOper is numberValue)
                {
                    return new numberValue { Value = ((lOper as numberValue).Value << (rOper as numberValue).Value) };
                }
                

            }

            exceptionMaster.throwException(009);
            return null;
        }
    }
}