using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexiCONOMICON;

namespace treeHOUSE
{
   public class ASSIGNATION_NODE : baseSTATEMENT
    {
        public idEXPRESSION_NODE idExpressionNode;
        public tokenType assignator;
        public EXPRESSION_NODE value;
        public bool isConstant;
    }
}
