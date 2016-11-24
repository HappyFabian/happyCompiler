using System.Collections.Generic;

namespace treeHOUSE
{
    public class bXOR_NODE : baseNODE
    {
        List<bAND_NODE> termbAND = new List<bAND_NODE>();

        public void addbAND(bAND_NODE node)
        {
            termbAND.Add(node);
        }
    }
}