using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;

namespace parSEER.Semantics.Types
{
    public class numberType : nodeType
    {
        public override nodeValue GetDefaultValue()
        {
            return new numberValue {Value = 0};
        }
    }
}
