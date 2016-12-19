using System;
using System.Text;
using lexiCONOMICON;
using parSEER;


namespace happyCompiler
{
    class Program
    {
        public static void Main(string[] args)
        {

            var path = args.Length == 0 ? "lexiTest.txt" : args[0];
            var lexiConomicon = new nlexiEngine(path,Encoding.UTF8);
            lexiConomicon.GenerateAllTokens();
            lexiConomicon.PrintTokens();
            var parSEER = new parserTools(lexiConomicon._generatedTokens);
            parSEER.parse();
            var compileEngy = new compilerEngine
            {
                _generatedStatements = parSEER.holder.generatedStatementNodes
            };

            compileEngy.compile();


            Console.ReadKey();

        }
    }
}
