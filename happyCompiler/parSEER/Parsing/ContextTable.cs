using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Parsing.Metadata;
using parSEER.Semantics.Tree.Expression.literalNodes;
using parSEER.Semantics.Tree.Sentences;
using parSEER.Semantics.Tree.Statements;
using parSEER.Semantics.Types;

namespace parSEER.Parsing
{
    public class contextTable
    {
        private static contextTable _instance;
        public static contextTable instance => _instance ?? (_instance = new contextTable());


        public static List<contextLayer> contexts;

        public static Dictionary<string, functionMetadata> savedFunctions;

        public static List<htmlNode> compiledList;
        public contextTable()
        {
            contexts = new List<contextLayer> {new contextLayer()};
            savedFunctions = new Dictionary<string, functionMetadata>();
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
            var smd = searchMetadata((statement.ID as idNode).Name);
            if(smd != null)
            { 
                if ((searchMetadata((statement.ID as idNode).Name) as variableMetadata).isItConstant)
                {
                    exceptionMaster.throwException(010);
                }
            }

            var variableMD = new variableMetadata();
            if (searchMetadataOnCurrentLevel((statement.ID as idNode).Name) != null)
            {
                exceptionMaster.throwException(010);
            }

            variableMD.variableName = (statement.ID as idNode).Name;
            variableMD.type = statement.Type;
            if (statement.Constant || statement.Value != null )
            {
                variableMD.value = statement.Value.Interpret();
            }
            else
            {
                variableMD.value = statement.Type.GetDefaultValue();
            }
            variableMD.isItConstant = statement.Constant;
            contexts[maxDepth()].addVariable(variableMD);
            
        }

        public metadataNode searchMetadataOnCurrentLevel(string name)
        {
            if (contexts[maxDepth()].contextMetadata.Count > 0)
            {
                var returnable = contexts[maxDepth()].contextMetadata.Find(n => n.variableName == name);
                if (returnable != null)
                {
                    return returnable;
                }
            }
            return null;
        }

        public contextLayer getCurrentDepthLayer()
        {
            return contexts[maxDepth()];
        }

        public void addNewContext()
        {
            contexts.Add(new contextLayer());           
        }
        public void addNewContext(contextLayer context)
        {
            contexts.Add(context);
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
                        if (!(returnable is variableMetadata))
                            throw new Exception("You can't define a new value to something different that a variable. Goof.");
                        if ((returnable as variableMetadata).isItConstant)
                            throw new Exception("You can't override a constant variable.");
                        (contexts[currentDepth].contextMetadata.Find(n => n.variableName == name) as
                            variableMetadata).value = interpret;
                        return;
                    }
                }
            }

        }

        public void AddHTMLStatement(htmlNode value)
        {
            compiledList.Add(value);
        }


        public void saveFunction(string name, functionDeclarativeStatement statement)
        {
            var functionMetadata = new functionMetadata();
            functionMetadata.parameters = getCurrentDepthLayer();
            functionMetadata.returnType = statement.returnType;
            functionMetadata.scope = statement.scope;
            
            savedFunctions.Add(name, functionMetadata);
        }

        public nodeType getFunctionType(string name)
        {
            return savedFunctions[name].returnType;
        }

        public functionMetadata getFunction(string name)
        {
            
            functionMetadata foundFunction = savedFunctions[name];
            functionMetadata returnableFunction = new functionMetadata
            {
                parameters = new contextLayer { contextMetadata = new List<metadataNode>(foundFunction.parameters.contextMetadata)},
                returnType = foundFunction.returnType,
                scope = foundFunction.scope,
                type = foundFunction.type,
                variableName = foundFunction.variableName
            };
            return returnableFunction;
        }

        public static nodeValue returnValueWas;
        public void returnValueIs(nodeValue interpret)
        {
            returnValueWas = interpret;
        }

        public void addArrayDefinition(declarativeArrayStatement statement)
        {
            var smd = searchMetadata((statement.Name as idNode).Name);
            if (smd != null)
            {
                if ((searchMetadata((statement.Name as idNode).Name) as variableMetadata).isItConstant)
                {
                    exceptionMaster.throwException(010);
                }
                if ((searchMetadata((statement.Name as idNode).Name) as variableArrayMetadata).isItConstant)
                {
                    exceptionMaster.throwException(010);
                }
            }

            var variableAM = new variableArrayMetadata();
            variableAM.arryValues = new nodeValue[(statement.size.Interpret() as numberValue).Value];
            variableAM.isItConstant = statement.Constant;
            variableAM.type = statement.Type;
            variableAM.variableName = (statement.Name as idNode).Name;
            contexts[maxDepth()].addVariable(variableAM);
        }

        public void changeVariableArrayValueOnCurrentContext(string name, int value, nodeValue interpret)
        {
            for (var currentDepth = maxDepth(); currentDepth >= 0; currentDepth--)
            {
                if (contexts[currentDepth].contextMetadata.Count > 0)
                {
                    var returnable = contexts[currentDepth].contextMetadata.Find(n => n.variableName == name);
                    if (returnable != null)
                    {
                        if (!(returnable is variableArrayMetadata))
                            throw new Exception("You can't define a new value to something different that a variable. Goof.");
                        if ((returnable as variableArrayMetadata).isItConstant)
                            throw new Exception("You can't override a constant variable.");
                        (contexts[currentDepth].contextMetadata.Find(n => n.variableName == name) as
                            variableArrayMetadata).arryValues[value] = interpret;
                        return;
                    }
                }
            }
        }
    }
}
