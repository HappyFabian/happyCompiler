using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexiCONOMICON;
using parSEER.Interpretative.Values;
using parSEER.Parsing;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Expression.literalNodes
{
    public class idNode : expressionNode 
    {
        //Maybe change it to tokenObject?
        public string Name { get; set; }

        public override nodeType EvaluateTypes()
        {
            return contextTable.instance.searchMetadata(Name).type;
        }

        public override nodeValue Interpret()
        {
            var returnedMetadata = contextTable.instance.searchMetadata(Name);
            if (returnedMetadata is variableMetadata)
            {
                return (returnedMetadata as variableMetadata).value;
            }
            return null;
        }
    }

}
