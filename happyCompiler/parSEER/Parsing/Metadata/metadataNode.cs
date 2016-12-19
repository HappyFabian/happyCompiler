using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Semantics.Types;

namespace parSEER.Parsing
{
    public abstract class metadataNode 
    {
        public string variableName { get; set; }

        public nodeType type { get; set; }
    }
}
