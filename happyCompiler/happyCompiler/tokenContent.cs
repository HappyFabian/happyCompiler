using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace happyCompiler
{
    public class tokenContent
    {
       private string Value;
        
       public void setValue(string VALUE)
        {
            Value = VALUE;
        } 

        public string getValue()
        {
            return Value;
        }
    }
}
