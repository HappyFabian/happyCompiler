using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexiCONOMICON;

namespace treeHOUSE
{
    public class idEXPRESSION_NODE : baseNODE
    {
        public List<UNARY_NODE> unaryList = new List<UNARY_NODE>();
        public tokenObject id;
        public List<baseNODE> accessorList;
        public List<EXPRESSION_NODE> parameters;
    }
}
