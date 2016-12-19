using System.Collections.Generic;

namespace parSEER.Parsing
{
    public class contextLayer 
    {
        //Variable, Function or Structure.
        public List<metadataNode> contextMetadata = new List<metadataNode>();


        public void addVariable(metadataNode node)
        {
            if (contextMetadata.Find(n => n.variableName == node.variableName) == null)
            {
                contextMetadata.Add(node);
            }
        }
    }
}