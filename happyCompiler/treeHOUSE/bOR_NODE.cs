using System.Collections.Generic;

namespace treeHOUSE
{
    public class bOR_NODE : baseNODE
    {
        List<bXOR_NODE> termbXOR = new List<bXOR_NODE>();

        public void addbXOR(bXOR_NODE node)
        {
            termbXOR.Add(node);
        }
    }
}