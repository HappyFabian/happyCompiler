using System;
using System.Collections.Generic;
using lexiCONOMICON;

namespace treeHOUSE
{
    public class RELATION_NODE
    {
        List<Tuple<tokenType,SHIFT_NODE>> termSHIFT = new List<Tuple<tokenType, SHIFT_NODE>>();

        public void addShift(SHIFT_NODE node, tokenType type = tokenType.NaN)
        {
            termSHIFT.Add(new Tuple<tokenType, SHIFT_NODE>(type,node));
        }
    }
}