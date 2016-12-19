using parSEER.Interpretative.Values;

namespace parSEER.Semantics.Types
{
    internal class stringValue : nodeValue
    {
        public string Value { get; set; }
        public override nodeValue Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}