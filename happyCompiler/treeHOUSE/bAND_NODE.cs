using System.Collections.Generic;

namespace treeHOUSE
{
    public class bAND_NODE : baseNODE
    {
        List<EQUAL_NODE> termEQUAL = new List<EQUAL_NODE>();

        public void addEqual(EQUAL_NODE node)
        {
            termEQUAL.Add(node);
        }
    }
}