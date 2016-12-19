using parSEER.Interpretative.Values;
using parSEER.Semantics.Types;

namespace parSEER
{
    public class voidType : nodeType
    {
        public override nodeValue GetDefaultValue()
        {
            return null;
            
        }
    }
}