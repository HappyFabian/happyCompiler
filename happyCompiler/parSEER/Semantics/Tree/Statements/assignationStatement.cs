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
    public class assignationStatement :statementNode
    {
        public expressionNode Id { get; set; }

        public expressionNode Value { get; set; }
        public override nodeType EvaluateSemantics()
        {
            var returnable = contextTable.instance.searchMetadata((Id as idNode).Name);
            if (returnable != null)
            {
                if (returnable.GetType() == typeof(variableMetadata))
                {
                    if ((returnable as variableMetadata).type.GetType() == Value.EvaluateTypes().GetType())
                    {
                        return null;
                    }
                }
            }


            exceptionMaster.throwException(009);
            return null;
        }

        public override void compile()
        {
            var returnable = contextTable.instance.searchMetadata((Id as idNode).Name);
            if (returnable != null)
            {
                if (returnable.GetType() == typeof(variableMetadata))
                {
                    if ((returnable as variableMetadata).type.GetType() == Value.EvaluateTypes().GetType())
                    {
                        contextTable.instance.changeVariableValueOnCurrentContext((Id as idNode).Name,Value.Interpret());
                        return;
                    }
                }
            }
            exceptionMaster.throwException(009);
            return;
        }
    }
}
