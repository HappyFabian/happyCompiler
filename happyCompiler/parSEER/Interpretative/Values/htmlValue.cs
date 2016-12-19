using lexiCONOMICON;

namespace parSEER.Interpretative.Values
{
    public class htmlValue : nodeValue
    {
        public string Value { get; set; }
        public override nodeValue Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}