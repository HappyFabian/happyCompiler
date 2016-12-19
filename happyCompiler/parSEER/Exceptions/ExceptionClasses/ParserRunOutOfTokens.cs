using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parSEER.Exceptions
{
    class ParserRunOutOfTokens : Exception
    {
        public ParserRunOutOfTokens(string message) : base(message)
        {

        }
    }
}
