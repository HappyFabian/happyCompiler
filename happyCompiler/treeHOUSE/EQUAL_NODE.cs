using System;
using System.Collections.Generic;
using lexiCONOMICON;

namespace treeHOUSE
{
    public class EQUAL_NODE : baseNODE
    {
        List<Tuple<tokenType,RELATION_NODE>> termRELATION = new List<Tuple<tokenType, RELATION_NODE>>();

        public void addRelation(RELATION_NODE node, tokenType type = tokenType.NaN)
        {
            termRELATION.Add(new Tuple<tokenType, RELATION_NODE>(type, node));
        }
    }
}