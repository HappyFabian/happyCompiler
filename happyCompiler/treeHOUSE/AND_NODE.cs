using System.Collections.Generic;

namespace treeHOUSE
{
    public class AND_NODE : baseNODE
    {
        List<bOR_NODE> termbOR = new List<bOR_NODE>();

        public void addbOR(bOR_NODE node)
        {
            termbOR.Add(node);
        }
    }
}