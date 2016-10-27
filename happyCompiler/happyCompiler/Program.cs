using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace happyCompiler
{
    class Program
    {
        static void Main(string[] args)
        {

            string path;
            if(args.Length == 0){ path = "lexiTest.txt"; }
            else { path = args[0]; }
            var lexiEngine = new lexiEngine(path,Encoding.UTF8);
            lexiEngine.generateTokens();
            lexiEngine.printTokens();
            Console.ReadKey();

        }
    }
}
