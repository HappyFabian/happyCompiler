using parSEER.Interpretative.Values;

namespace parSEER.Semantics.Types
{
    public class boolType : nodeType
    {
        public override nodeValue GetDefaultValue()
        {
            return new boolValue {Value = false};
        }
    }
}