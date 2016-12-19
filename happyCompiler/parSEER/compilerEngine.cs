using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parSEER.Parsing;
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
    }
}
