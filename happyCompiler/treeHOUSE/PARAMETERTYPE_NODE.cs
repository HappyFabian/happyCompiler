using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexiCONOMICON;

namespace treeHOUSE
{
    public class PARAMETERTYPE_NODE : baseNODE
    {
        public tokenObject type;
        public idEXPRESSION_NODE idExpressionNode;
        public EXPRESSION_NODE value;
    }
}
