using parSEER.Interpretative.Values;
using parSEER.Semantics.Tree.Expression;

using parSEER.Semantics.Types;
using parSEER.Semantics.Types.literalTypes;

namespace parSEER
{
    internal class substractionNode : bOperationNode
    {
        public override nodeType EvaluateTypes()
        {
            if (LeftOperand.EvaluateTypes().GetType() == RightOperand.EvaluateTypes().GetType())
            {
                if (
                    LeftOperand.EvaluateTypes().GetType() == typeof(numberType)
                    || LeftOperand.EvaluateTypes().GetType() == typeof(floatType)
                  )
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

                if (lOper is numberValue)
                {
                    return new numberValue { Value = (lOper as numberValue).Value - (rOper as numberValue).Value };
                }
                if (lOper is floatValue)
                {
                    return new floatValue { Value = (lOper as floatValue).Value - (rOper as floatValue).Value };
                }

            }

            exceptionMaster.throwException(009);
            return null;
        }
    }
}