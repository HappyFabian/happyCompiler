using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Parsing;
using parSEER.Parsing.Metadata;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Tree.Expression.literalNodes;
using parSEER.Semantics.Tree.Sentences;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Statements
{
    class assignmentArrayStatement : statementNode
    {
        public expressionNode Id { get; set; }

        public expressionNode Address { get; set; }

        public expressionNode Value { get; set; }

        public override nodeType EvaluateSemantics()
        {
            throw new NotImplementedException();
        }

        public override void compile()
        {
            if (Address.EvaluateTypes().GetType() == typeof(numberType))
            {
                var returnable = contextTable.instance.searchMetadata((Id as idNode).Name);
                if (returnable != null)
                {
                    if (returnable.GetType() == typeof(variableArrayMetadata))
                    {
                        if ((returnable as variableArrayMetadata).type.GetType() == Value.EvaluateTypes().GetType())
                        {
                            contextTable.instance.changeVariableArrayValueOnCurrentContext((Id as idNode).Name, (Address.Interpret() as numberValue).Value,Value.Interpret());
                            return;
                        }
                    }
                }
            }

           
        }
    }
}
