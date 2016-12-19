using System;
using System.Threading;
using lexiCONOMICON;
using parSEER.Exceptions;
using parSEER.Exceptions.ExceptionClasses;

namespace parSEER
{
    public static class exceptionMaster
    {

        public static void throwException(int flag, tokenObject currentToken = null, tokenType expectedType = tokenType.NaN)
        {
            switch (flag)
            {
                case 011: throw new Exception("Invalid If Condition.");
                case 010: throw new Exception("Variable has already been previously define.");
                case 009: throw new Exception("Invalid Value assiged to Variable.");
                case 008: throw new ExpectedTokenTypeException("Unexpected Token Found at " + currentToken._coordinates.ToString());
                case 006: throw new EndOfStatementException("End of Statement expected at" + currentToken._coordinates.ToString());
                case 005: throw new ParserRunOutOfTokens("You retracted the tokens too many times.");
                case 004: throw new InvalidFactorFound("Factor was Expected at " + currentToken._coordinates.ToString());
                case 003: throw new ExpectedTokenTypeException("Unexpected Token Found at " + currentToken._coordinates.ToString()  + "  expected Token Was: " + expectedType.ToString());
                case 002: throw new IndexOutOfRangeException("You have reached the upper limit of the list at " + currentToken._coordinates.ToString());
                case 001: throw new ParserRunOutOfTokens("The parser run out of tokens at: " + currentToken._coordinates.ToString());
                case 000: throw new EndOfFileException("End Of File expected at" + currentToken._coordinates.ToString());
                default:
                    throw new Exception("Wrong Exception Flag");
            }
        }
    }
}