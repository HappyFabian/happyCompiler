using parSEER.Parsing;
using parSEER.Semantics.Tree.Expression.literalNodes;
using parSEER.Semantics.Tree.Sentences;
using parSEER.Semantics.Types;

namespace parSEER
{
    public class hmtlStatement : statementNode
    {
        public htmlNode value { get; set; }

        public override nodeType EvaluateSemantics()
        {
            return value.EvaluateTypes();
        }

        public override void compile()
        {
            contextTable.instance.AddHTMLStatement(value);
        }
    }
}