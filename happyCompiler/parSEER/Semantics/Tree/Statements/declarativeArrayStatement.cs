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
    public class declarativeArrayStatement : statementNode
    {
        public nodeType Type { get; set; }
        public expressionNode size { get; set; }
        public expressionNode Name { get; set; }
        public bool Constant { get; set; }

        public override nodeType EvaluateSemantics()
        {
            throw new NotImplementedException();
        }

        public override void compile()
        {
            if (Type != null && size.EvaluateTypes().GetType() == typeof(numberType))
            {
                contextTable.instance.addArrayDefinition(this);
                return;
            }
            exceptionMaster.throwException(009);
            return;
        }
    }
}
