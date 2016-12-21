using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Parsing;
using parSEER.Parsing.Metadata;
using parSEER.Semantics.Tree.Expression.literalNodes;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Expression.operationNodes
{
    public class arrayNode : bOperationNode
    {
        public override nodeType EvaluateTypes()
        {
            var lOper = (LeftOperand as idNode);
            var returnedMetadata = contextTable.instance.searchMetadata(lOper.Name);
            if (returnedMetadata is variableArrayMetadata)
            {
                return (returnedMetadata as variableArrayMetadata).type;
            }
            throw new Exception("Woops");
        }

        public override nodeValue Interpret()
        {
            var lOper = (LeftOperand as idNode);
            var rOper = (RightOperand.Interpret());
            var returnedMetadata = contextTable.instance.searchMetadata(lOper.Name);
            if (returnedMetadata is variableArrayMetadata)
            {
                if (rOper is numberValue)
                {

                    return (returnedMetadata as variableArrayMetadata).arryValues[(rOper as numberValue).Value];
                }
            }
            throw new Exception("Wrong array Call.");
            return null;
        }
    }
}
