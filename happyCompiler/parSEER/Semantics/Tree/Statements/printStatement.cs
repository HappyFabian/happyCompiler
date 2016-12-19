using parSEER.Interpretative.Values;
using parSEER.Parsing;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Tree.Expression.literalNodes;
using parSEER.Semantics.Tree.Sentences;
using parSEER.Semantics.Types;

namespace parSEER
{
    internal class printStatement : statementNode
    {

        public expressionNode Value { get; set; }

        public override nodeType EvaluateSemantics()
        {
            throw new System.NotImplementedException();
        }

        public override void compile()
        {
            var node = new htmlNode();
            var interpreted = Value.Interpret();
            var stringInterpreted = IdentifyValue(interpreted);


            node.Value = "<div>" + stringInterpreted + "</div>";
            contextTable.instance.AddHTMLStatement(node);
        }


        private string IdentifyValue(nodeValue interpret)
        {
            if (interpret is boolValue)
            {
                return ((boolValue)interpret).Value.ToString();
            }
            if (interpret is charValue)
            {
                return ((charValue)interpret).Value.ToString();
            }
            if (interpret is numberValue)
            {
                return ((numberValue)interpret).Value.ToString();
            }
            if (interpret is stringValue)
            {
                return ((stringValue)interpret).Value.ToString();
            }
            if (interpret is floatValue)
            {
                return ((floatValue)interpret).Value.ToString();
            }
            return null;
        }
    }
}