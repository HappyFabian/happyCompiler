using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace happyCompiler
{
    public class tokenContent
    {
       private string _value;
        
       public void setValue(string VALUE)
        {
            _value = VALUE;
        } 

        public string getValue()
        {
            return _value;
        }
    }
}
