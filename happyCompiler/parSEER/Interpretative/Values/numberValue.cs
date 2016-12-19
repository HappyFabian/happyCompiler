using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parSEER.Interpretative.Values
{
    class numberValue : nodeValue
    {
        public int Value { get; set; }
    
        public override nodeValue Clone()
        {
            return new numberValue {Value = this.Value};
        }
    }
}
