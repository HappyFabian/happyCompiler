using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Parsing;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Tree.Sentences;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Statements
{
    class returnStatement : statementNode
    {
        public expressionNode Value { get; set; }

        public override nodeType EvaluateSemantics()
        {
            if (Value == null)
            {
                return new voidType();
            }
            return Value.EvaluateTypes();
        }

        public override void compile()
        {
            contextTable.instance.returnValueIs(Value.Interpret());
        }
    }
}
