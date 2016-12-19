using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parSEER.Exceptions.ExceptionClasses
{
    public class EndOfStatementException : Exception
    {
        public EndOfStatementException(string message):base(message)
        { }
    }
}
