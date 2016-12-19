namespace parSEER.Interpretative.Values
{
    public class floatValue : nodeValue
    {
        public float Value { get; set; }
        public override nodeValue Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}