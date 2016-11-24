using System;
using System.Collections.Generic;
using lexiCONOMICON;

namespace treeHOUSE
{
    public class TERM_NODE : baseNODE
    {
        List<Tuple<tokenType,FACTOR_NODE>> factor = new List<Tuple<tokenType, FACTOR_NODE>>();

        public void addFactor(FACTOR_NODE node, tokenType type = tokenType.NaN)
        {
            factor.Add(new Tuple<tokenType, FACTOR_NODE>(type,node));
        }

    }
}