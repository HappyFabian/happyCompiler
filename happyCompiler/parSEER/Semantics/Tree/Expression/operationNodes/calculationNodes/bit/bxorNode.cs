using parSEER.Interpretative.Values;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Types;

namespace parSEER
{
    internal class bxorNode : bOperationNode
    {
        public override nodeType EvaluateTypes()
        {
            if (LeftOperand.EvaluateTypes().GetType() == RightOperand.EvaluateTypes().GetType())
            {
                if (LeftOperand.EvaluateTypes().GetType() == typeof(boolType))
                {
                    return LeftOperand.EvaluateTypes();
                }
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

               
                if (lOper is boolValue)
                {
                    return new boolValue { Value = ((lOper as boolValue).Value ^ (rOper as boolValue).Value) };
                }

            }

            exceptionMaster.throwException(009);
            return null;
        }
    }
}