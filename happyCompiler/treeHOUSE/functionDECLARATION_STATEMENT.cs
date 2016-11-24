using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexiCONOMICON;

namespace treeHOUSE
{
    public class functionDECLARATION_STATEMENT : baseSTATEMENT
    {
        public tokenObject returnType;
        public idEXPRESSION_NODE idExpressionNode;
        public List<baseNODE> listparameterstype;
        public SCOPE_NODE scope;
    }
}
