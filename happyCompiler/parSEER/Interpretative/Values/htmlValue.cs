using lexiCONOMICON;

namespace parSEER.Interpretative.Values
{
    public class htmlValue : nodeValue
    {
        public tokenObject Value { get; set; }
        public override nodeValue Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}