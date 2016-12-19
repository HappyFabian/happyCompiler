using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parSEER.Semantics.Tree.Expression
{
    public abstract class bOperationNode : expressionNode
    {
        public expressionNode LeftOperand { get; set; }
        public expressionNode RightOperand { get; set; }
    }
}
