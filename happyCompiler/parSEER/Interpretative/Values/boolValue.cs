namespace parSEER.Interpretative.Values
{
    public class boolValue : nodeValue
    {
        public bool Value { get; set; }

        public override nodeValue Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}