using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Parsing;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Tree.Expression.literalNodes;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Sentences
{
    public class declarativeStatement : statementNode
    {
        public bool Constant { get; set; }
        public nodeType Type { get; set; }
        public expressionNode ID { get; set; }
        public expressionNode Value { get; set; }

        public override nodeType EvaluateSemantics()
        {
            if (Value == null || Value.EvaluateTypes().GetType() == Type.GetType())
            {
                contextTable.instance.addVariableDefinitonToCurrentContext(this);

                return Type;
            }

            exceptionMaster.throwException(009);
            return null;
        }

        public override void compile()
        {
            if (Value == null || Value.EvaluateTypes().GetType() == Type.GetType())
            {
                contextTable.instance.addVariableDefinitonToCurrentContext(this);
                if (Value != null)
                {
                   contextTable.instance.changeVariableValueOnCurrentContext((ID as idNode).Name,Value.Interpret());
                }

                return;
            }

            exceptionMaster.throwException(009);
            return;
        }
    }
}
