using parSEER.Interpretative.Values;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Types;

namespace parSEER
{
    internal class notequalNode : bOperationNode
    {
        public override nodeType EvaluateTypes()
        {
            if (LeftOperand.EvaluateTypes().GetType() == RightOperand.EvaluateTypes().GetType())
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
                    return new boolValue { Value = ((lOper as numberValue).Value != (rOper as numberValue).Value) };
                }
                if (lOper is floatValue)
                {
                    return new boolValue { Value = ((lOper as floatValue).Value != (rOper as floatValue).Value) };
                }
                if (lOper is stringValue)
                {
                    return new boolValue { Value = ((lOper as stringValue).Value != (rOper as stringValue).Value) };
                }
                if (lOper is charValue)
                {
                    return new boolValue { Value = ((lOper as charValue).Value != (rOper as charValue).Value) };
                }
                if (lOper is boolValue)
                {
                    return new boolValue { Value = ((lOper as boolValue).Value != (rOper as boolValue).Value) };
                }

            }

            exceptionMaster.throwException(009);
            return null;
        }
    }
}