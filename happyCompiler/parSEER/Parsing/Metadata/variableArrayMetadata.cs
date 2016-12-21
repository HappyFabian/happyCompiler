using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;

namespace parSEER.Parsing.Metadata
{
    class variableArrayMetadata : metadataNode
    {
        public bool isItConstant;
        public nodeValue[] arryValues  = null;
    }
}
