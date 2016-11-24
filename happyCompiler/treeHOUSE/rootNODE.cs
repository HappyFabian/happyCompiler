using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace treeHOUSE
{
    public class rootNODE : baseNODE
    {
        List<baseSTATEMENT> statementsInCode = new List<baseSTATEMENT>();

        public void addStatement(baseSTATEMENT newStatement)
        {
            statementsInCode.Add(newStatement);
        }
    }
}
