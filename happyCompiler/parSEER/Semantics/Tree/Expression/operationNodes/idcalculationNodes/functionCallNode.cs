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

            throw new Exception("Function called failed.");
        }
    }
}
