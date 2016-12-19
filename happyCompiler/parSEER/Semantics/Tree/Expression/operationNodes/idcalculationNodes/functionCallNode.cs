using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Parsing;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Expression.operationNodes.idcalculationNodes
{
    public class functionCallNode : expressionNode
    {
        public expressionNode ID { get; set; }
        public List<expressionNode> parameters { get; set; }

        public override nodeType EvaluateTypes()
        {
            return ID.EvaluateTypes();           
        }

        public override nodeValue Interpret()
        {
            throw new NotImplementedException();
        }
    }
}
