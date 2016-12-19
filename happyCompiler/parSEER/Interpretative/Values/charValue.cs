namespace parSEER.Interpretative.Values
{
    public class charValue : nodeValue
    {
        public char Value { get; set; }
        public override nodeValue Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}