using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Types;

namespace parSEER.Parsing
{
    class variableMetadata : metadataNode
    {
        public bool isItConstant;
        public nodeValue value { get; set; }
    }
}
