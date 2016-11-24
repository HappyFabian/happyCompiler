using System;
using System.Collections.Generic;
using lexiCONOMICON;

namespace treeHOUSE
{
    public class SHIFT_NODE
    {
        List<Tuple<tokenType,ARITHMETIC_NODE>> termARITMETHIC = new List<Tuple<tokenType, ARITHMETIC_NODE>>();

        public void addArithmetica(ARITHMETIC_NODE node, tokenType type = tokenType.NaN)
        {
            termARITMETHIC.Add(new Tuple<tokenType, ARITHMETIC_NODE>(type,node));
        }
    }
}