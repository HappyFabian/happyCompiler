using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Interpretative.Values;
using parSEER.Parsing;
using parSEER.Semantics.Tree.Expression.literalNodes;
using parSEER.Semantics.Tree.Statements;
using parSEER.Semantics.Types;

namespace parSEER.Semantics.Tree.Expression.operationNodes.idcalculationNodes
{
    public class functionCallNode : expressionNode
    {
        public expressionNode ID { get; set; }
        public List<expressionNode> parameters { get; set; }

        public override nodeType EvaluateTypes()
        {
            return contextTable.instance.getFunctionType((ID as idNode).Name);
        }

        public override nodeValue Interpret()
        {
            var functionCalled  =  contextTable.instance.getFunction((ID as idNode).Name);
            contextTable.instance.addNewContext(functionCalled.parameters);
            var parametersExpected =  functionCalled.parameters.contextMetadata;
            if (parameters.Count == parametersExpected.Count)
            {
                int i = 0;
                foreach (variableMetadata metadataNode in parametersExpected)
                {
                    var parameterToProcess = parameters[i];
                    if (parameterToProcess.GetType() == typeof(defNode) && metadataNode.value != null)
                    {
                        i++;
                    }
                    else
                    {
                        if (parameterToProcess.GetType() == typeof(defNode) && metadataNode.value == null)
                        {
                            throw new Exception("Default parameter value not defined.");
                        }
 
                        if (parameterToProcess.EvaluateTypes().GetType() == metadataNode.type.GetType())
                        {
                            contextTable.instance.changeVariableValueOnCurrentContext(metadataNode.variableName, parameters[i].Interpret());
                            i++;
                        }
                        else
                        {
                            throw new Exception("Invalid parameter passed.");
                        }
                        
                    }
  
                }
            }
            else
            {
                throw new Exception("Invalid amount of Parameters found. Expected: " + parametersExpected.Count + " Found: " + parameters.Count);
            }
            foreach (var statementNode in functionCalled.scope)
            {
                if (contextTable.returnValueWas == null)
                {
                    statementNode.compile();
                }
            }
            contextTable.instance.removeContext();
            if (functionCalled.returnType.GetDefaultValue().GetType() == contextTable.returnValueWas.GetType())
            {
                var returnValue = contextTable.returnValueWas;
                contextTable.returnValueWas = null;
                return returnValue;
            }

            /*
            contextTable.instance.addNewContext(functionCalled.parameters);
            var currentDepth = contextTable.instance.getCurrentDepthLayer();
            for (int i = 0; i < currentDepth.contextMetadata.Count; i++)
            {
                if (parameters[i].EvaluateTypes().GetType() == currentDepth.contextMetadata[i].type.GetType())
                {
                    contextTable.instance.changeVariableValueOnCurrentContext(currentDepth.contextMetadata[i].variableName,parameters[i].Interpret());
                }
            }
            for (int i = 0; i <  (functionCalled.scope.Count - 1); i++)
            {
                functionCalled.scope[i].compile();
            }
            var lastStatement = functionCalled.scope[functionCalled.scope.Count - 1];
            if (lastStatement is returnStatement && !(lastStatement.EvaluateSemantics() is  voidType))
            {
                if (lastStatement.EvaluateSemantics().GetType() == functionCalled.returnType.GetType())
                {
                    var returnValue = (lastStatement as returnStatement).Value.Interpret();
                    contextTable.instance.removeContext();
                    return returnValue;
                }
            }
            if (lastStatement.EvaluateSemantics() is voidType && lastStatement is returnStatement)
            {
                contextTable.instance.removeContext();
                return null;
            }
            
            */
            throw new Exception("Function called failed.");
            
        }
    }
}
