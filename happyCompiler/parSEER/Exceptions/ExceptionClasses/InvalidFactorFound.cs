using System;
using System.Runtime.Serialization;

namespace parSEER
{
    public class InvalidFactorFound : Exception
    {
 
        public InvalidFactorFound(string message) : base(message)
        {
        }

    }
}