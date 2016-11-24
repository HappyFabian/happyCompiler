using System.Collections.Generic;

namespace treeHOUSE
{
    public class OR_NODE : baseNODE
    {
        List<AND_NODE> termAND = new List<AND_NODE>();

        public void addAnd(AND_NODE node)
        {
            termAND.Add(node);
        }
    }
}