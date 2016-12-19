using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Parsing;
using parSEER.Parsing.Metadata;
using parSEER.Semantics.Tree.Expression.literalNodes;
using parSEER.Semantics.Tree.Sentences;

namespace parSEER
{
    public class compilerEngine
    {

        public static contextTable table = new contextTable();

        public List<statementNode> _generatedStatements { get; set; }

        /*
        public static void checkSemantics()
        {
            foreach (var statement in _generatedStatements)
            {
                statement.EvaluateSemantics();
            }
            Console.WriteLine("Semantic Checking OK.");
        }
        */

        public void compile()
        {
            foreach (var generatedStatement in _generatedStatements)
            {
                generatedStatement.compile();
            }
            Console.WriteLine("Semantic and Interpretation is Done!");
        }

        public string printResponse()
        {
            var returnable = "";
            foreach (var htmlNode in contextTable.compiledList)
            {
                returnable += htmlNode.Value;
            }
            return returnable;
        }

        public void purgeContext()
        {
            contextTable.compiledList = new List<htmlNode>();
            contextTable.savedFunctions = new Dictionary<string, functionMetadata>();
            contextTable.contexts = new List<contextLayer> { new contextLayer()};
        }
    }
}
