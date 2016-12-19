using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Semantics.Tree.Expression.literalNodes;
using parSEER.Semantics.Tree.Sentences;
using parSEER.Semantics.Types;

namespace parSEER.Parsing
{
    public class contextTable
    {
        private static contextTable _instance;
        public static contextTable instance => _instance ?? (_instance = new contextTable());


        public static List<contextLayer> contexts;

        public static List<contextLayer> savedFunctions;

        public static List<htmlNode> compiledList;
        public contextTable()
        {
            contexts = new List<contextLayer> {new contextLayer()};
            savedFunctions = new List<contextLayer>();
            compiledList = new List<htmlNode>();
        }

        public metadataNode searchMetadata(string name)
        {
            for (var currentDepth = maxDepth(); currentDepth >= 0; currentDepth--)
            {
                if(contexts[currentDepth].contextMetadata.Count > 0)
                { 
                    var returnable = contexts[currentDepth].contextMetadata.Find(n => n.variableName == name);
                    if (returnable != null)
                    {
                        return returnable;
                    }
                }

            }

            return null;
        }

        public void addVariableDefinitonToCurrentContext(declarativeStatement  statement)
        {

            var variableMD = new variableMetadata();
            if (searchMetadata((statement.ID as idNode).Name) != null)
            {
                // dummy
                exceptionMaster.throwException(010);
            }

            variableMD.variableName = (statement.ID as idNode).Name;
            variableMD.type = statement.Type;
            variableMD.value = statement.Type.GetDefaultValue();
            contexts[maxDepth()].addVariable(variableMD);
        }

        public contextLayer getCurrentDepthLayer()
        {
            return contexts[maxDepth()];
        }

        public void addNewContext()
        {
            contexts.Add(new contextLayer());           
        }

        public void removeContext()
        {
            if(maxDepth() > 0)
            contexts.RemoveAt(maxDepth());
        }

        public int maxDepth()
        {
            return  contexts.Count - 1;
        }

        public void changeVariableValueOnCurrentContext(string name,nodeValue interpret)
        {
            for (var currentDepth = maxDepth(); currentDepth >= 0; currentDepth--)
            {
                if (contexts[currentDepth].contextMetadata.Count > 0)
                {
                    var returnable = contexts[currentDepth].contextMetadata.Find(n => n.variableName == name);
                    if (returnable != null)
                    {
                        if (contexts[currentDepth].contextMetadata.Find(n => n.variableName == name) is variableMetadata)
                        {
                            (contexts[currentDepth].contextMetadata.Find(n => n.variableName == name) as
                                variableMetadata).value = interpret;
                        }
                        else
                        {
                            throw new Exception("You can't define a new value to something different that a variable. Goof.");
                        }
                    }
                }

            }

        }

        public void AddHTMLStatement(htmlNode value)
        {
            compiledList.Add(value);
        }
    }
}
