using lexiCONOMICON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parSEER
{

    public class parserEngine
    {
        private List<tokenObject> _generatedTokens;

        private tokenObject _currentToken;
        private int counter = 0;

        public parserEngine(List<tokenObject> generatedTokens)
        {
            _generatedTokens = generatedTokens;
            _currentToken = _generatedTokens[counter];
        }

        public void parse()
        {
            Code();
            if (_currentToken._type != tokenType.system_EndOfFile)
            { throw new Exception("INVALID TOKEN AT: " + _currentToken._coordinates); }
        }

        private void advanceToken() { counter++; _currentToken = _generatedTokens[counter]; }
        private tokenType currentTokenType() { return _currentToken._type; }

        private void Code()
        {
            IncludeList();
            StatementList();
        }

        private void IncludeList()
        {
            if (_currentToken._type == tokenType.symbol_HASHTAG)
            {
                IncludeStatement();
                IncludeList();
            }
        }
        private void IncludeStatement()
        {
            advanceToken();
            if (currentTokenType() == tokenType.resword_INCLUDE)
            {
                advanceToken();
            }
            if (currentTokenType() == tokenType.literal_FILENAME || currentTokenType()  == tokenType.literal_STRING)
            {
                advanceToken();
            }

        }


        bool checkStandardTypes()
        {
            if (currentTokenType() == tokenType.resword_INT
                || currentTokenType() == tokenType.resword_FLOAT
                || currentTokenType() == tokenType.resword_BOOL
                || currentTokenType() == tokenType.resword_CHAR
                || currentTokenType() == tokenType.resword_STRING
                || currentTokenType() == tokenType.resword_DATE)
            {
                return true;
            }
            return false;
        }
        bool checkVAR()
        {
            if (currentTokenType() == tokenType.resword_VAR)
            {
                return true;
            }
            return false;

        }
        bool checkSTRUCT()
        {
            if (currentTokenType() == tokenType.resword_STRUCT)
            {
                return true;
            }
            return false;

        }
        bool checkALLtypes()
        {
            if(checkStandardTypes() || checkVAR() || checkSTRUCT())
            {
                return true;
            }
            return false;

        }
        bool checkStandardAndStruct()
        {
            if (checkStandardTypes() || checkSTRUCT())
            {
                return true;
            }
            return false;
        }
        bool checkStandardAndVar()
        {
            if (checkStandardTypes() || checkVAR())
            {
                return true;
            }
            return false;
        } 
        bool isUnaryToken()
        {
            if (currentTokenType() == tokenType.oper_MULTIPLICATION 
                ||currentTokenType() == tokenType.logicaloper_NOT
                || currentTokenType() == tokenType.bitoper_NOT
                || currentTokenType() == tokenType.bitoper_AND
                || currentTokenType() == tokenType.oper_ADDITION
                || currentTokenType() == tokenType.oper_SUBSTRACTION
                || currentTokenType() == tokenType.oper_INCREASE
                || currentTokenType() == tokenType.oper_DECREASE)
            {
                return true;
            }
            return false;
        }


        private void StatementList()
        {
            if (currentTokenType() == tokenType.resword_CONST
                 || currentTokenType() == tokenType.resword_STRUCT
                 || currentTokenType() == tokenType.resword_ENUM
                 || checkALLtypes()
                || currentTokenType() == tokenType.resword_IF
                || currentTokenType() == tokenType.resword_SWITCH
                || currentTokenType() == tokenType.resword_WHILE
                || currentTokenType() == tokenType.resword_DO
                || currentTokenType() == tokenType.resword_FOR
                || currentTokenType() == tokenType.resword_FOREACH
                || currentTokenType() == tokenType.resword_SWITCH
                || isUnaryToken() 
                || currentTokenType() == tokenType.ID
                || currentTokenType() == tokenType.symbol_EndOfStatement)
            {
                Statement();
                StatementList();
            }   

        }

        private void Statement()
        {
            if (currentTokenType() == tokenType.resword_CONST) { }
            else if(currentTokenType() == tokenType.resword_STRUCT) { }
            else if (currentTokenType() == tokenType.resword_ENUM) { }
            else if ( checkALLtypes()) { }
            else if ( currentTokenType() == tokenType.resword_IF) { }
            else if ( currentTokenType() == tokenType.resword_SWITCH) { }
            else if ( currentTokenType() == tokenType.resword_WHILE) { }
            else if ( currentTokenType() == tokenType.resword_DO) { }
            else if ( currentTokenType() == tokenType.resword_FOR) { }
            else if ( currentTokenType() == tokenType.resword_FOREACH) { }
            else if ( currentTokenType() == tokenType.resword_SWITCH) { }
            else if ( isUnaryToken()) { }
            else if ( currentTokenType() == tokenType.ID) { }
            else if ( currentTokenType() == tokenType.symbol_EndOfStatement) { }

            throw new Exception("Statement is invalid at Location: " + _currentToken._coordinates);
        }
    }
}
