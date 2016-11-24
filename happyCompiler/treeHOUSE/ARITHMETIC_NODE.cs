using System;
using System.Collections.Generic;
using lexiCONOMICON;

namespace treeHOUSE
{
    public class ARITHMETIC_NODE
    {
        List<Tuple<tokenType,TERM_NODE>> term = new List<Tuple<tokenType, TERM_NODE>>();

        public void addTerm(TERM_NODE node, tokenType type = tokenType.NaN)
        {
            term.Add(new Tuple<tokenType, TERM_NODE>(type,node));
        }
    }
}