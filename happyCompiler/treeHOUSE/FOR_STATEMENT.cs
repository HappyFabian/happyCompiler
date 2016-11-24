using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace treeHOUSE
{
    public class FOR_STATEMENT : baseSTATEMENT
    {
        public FOREXPRESSION_NODE firstExpression;
        public FOREXPRESSION_NODE secondExpression;
        public FOREXPRESSION_NODE thirdExpression;
        public SCOPE_NODE scope;
    }
}
