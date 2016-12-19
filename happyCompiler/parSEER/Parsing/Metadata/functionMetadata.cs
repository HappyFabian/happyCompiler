using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Tree.Sentences;
using parSEER.Semantics.Types;

namespace parSEER.Parsing.Metadata
{
    public class functionMetadata : metadataNode
    {
        public contextLayer parameters;
        public nodeType returnType { get; set; }
        public List<statementNode> scope { get; set; }

    }
}
