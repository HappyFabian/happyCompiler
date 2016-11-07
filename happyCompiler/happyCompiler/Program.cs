using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexiCONOMICON;

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
            Console.ReadKey();

        }
    }
}
