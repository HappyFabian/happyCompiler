using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parSEER.Exceptions
{
    public class EndOfFileException : Exception
    {
        public EndOfFileException(string message) : base(message)
        {
            
        }
    }
}
