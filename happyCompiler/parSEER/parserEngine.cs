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
            {
                throw new Exception("INVALID TOKEN AT: " + _currentToken._coordinates);
            }
        }

        private void throwException()
        {
            throw new Exception("Statement is invalid at Location: " + _currentToken._coordinates);
        }

        private void advanceToken()
        {
            counter++;
            _currentToken = _generatedTokens[counter];
        }

        private tokenType currentTokenType()
        {
            return _currentToken._type;
        }

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
            if (currentTokenType() == tokenType.literal_FILENAME || currentTokenType() == tokenType.literal_STRING)
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
            if (checkStandardTypes() || checkVAR() || checkSTRUCT())
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
                || currentTokenType() == tokenType.logicaloper_NOT
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
            if (currentTokenType() != tokenType.resword_CONST
                && currentTokenType() != tokenType.resword_STRUCT
                && currentTokenType() != tokenType.resword_ENUM
                && !checkALLtypes()
                && currentTokenType() != tokenType.resword_IF
                && currentTokenType() != tokenType.resword_SWITCH
                && currentTokenType() != tokenType.resword_WHILE
                && currentTokenType() != tokenType.resword_DO
                && currentTokenType() != tokenType.resword_FOR
                && currentTokenType() != tokenType.resword_FOREACH
                && currentTokenType() != tokenType.resword_SWITCH
                && !isUnaryToken()
                && currentTokenType() != tokenType.ID
                && currentTokenType() != tokenType.symbol_EndOfStatement) return;
            Statement();
            StatementList();
        }

        private void Statement()
        {
            if (currentTokenType() == tokenType.resword_CONST)
            {
                Console.Write("CONSTANT  ");
                CONSTANT_DECLARATION();
                EOS();
                Console.WriteLine("EOS");
            }
            else if (currentTokenType() == tokenType.resword_STRUCT)
            {
                Console.Write("STRUCT  ");
                STRUCT_DECLARATION();
            }
            else if (currentTokenType() == tokenType.resword_ENUM)
            {
                ENUM_DECLARATION();
            }
            else if (checkALLtypes())
            {
                Console.Write("DECLARATION ");
                if (checkALLtypes())
                {
                    advanceToken();
                    unaryList();
                    ID();
                    if (currentTokenType() == tokenType.symbol_openParenthesis)
                    {
                        advanceToken();
                        ListParameterTypes();
                        if (currentTokenType() == tokenType.symbol_closeParenthesis)
                        {
                            advanceToken();
                            SCOPE();

                        }
                    }
                    else
                        accesorList();
                    DECLARATION_variableP();
                    EOS();
                }
                EOS();

                Console.WriteLine("EOS");
            }
            else if (currentTokenType() == tokenType.resword_IF)
            {
            }
            else if (currentTokenType() == tokenType.resword_SWITCH)
            {
            }
            else if (currentTokenType() == tokenType.resword_WHILE)
            {
            }
            else if (currentTokenType() == tokenType.resword_DO)
            {
            }
            else if (currentTokenType() == tokenType.resword_FOR)
            {
            }
            else if (currentTokenType() == tokenType.resword_FOREACH)
            {
            }
            else if (currentTokenType() == tokenType.resword_SWITCH)
            {
            }
            else if (isUnaryToken())
            {
            }
            else if (currentTokenType() == tokenType.ID)
            {
            }
            else if (currentTokenType() == tokenType.symbol_EndOfStatement)
            {
                EOS();
            }

        }

        private void ENUM_DECLARATION()
        {
            throw new NotImplementedException();
        }

        private void SCOPE()
        {
            if (currentTokenType() == tokenType.symbol_openCurlyBraces)
            {
                advanceToken();
                ListBasicStatements();
                if (currentTokenType() == tokenType.symbol_closeCurlyBraces)
                {
                    advanceToken();
                    return;
                }
                throwException();
            }
            else
            {
                BasicStatement();
            }
        }

        private void BasicStatement()
        {
            if (checkStandardAndVar())
            {
                Console.Write("DECLARATION VARIABLE");
                    advanceToken();
                    ID_Simple();
                    accesorList();
                    DECLARATION_variableP();
                    EOS();
                Console.WriteLine("EOS");
            }
            else if (currentTokenType() == tokenType.resword_IF)
            {
            }
            else if (currentTokenType() == tokenType.resword_SWITCH)
            {
            }
            else if (currentTokenType() == tokenType.resword_WHILE)
            {
            }
            else if (currentTokenType() == tokenType.resword_DO)
            {
            }
            else if (currentTokenType() == tokenType.resword_FOR)
            {
            }
            else if (currentTokenType() == tokenType.resword_FOREACH)
            {
            }
            else if (currentTokenType() == tokenType.resword_SWITCH)
            {
            }
            else if (isUnaryToken())
            {
            }
            else if (currentTokenType() == tokenType.ID)
            {
            }
            else if (currentTokenType() == tokenType.symbol_EndOfStatement)
            {
                EOS();
            }
        }

        private void ListBasicStatements()
        {
                if (!checkStandardAndVar()
                && currentTokenType() != tokenType.resword_IF
                && currentTokenType() != tokenType.resword_SWITCH
                && currentTokenType() != tokenType.resword_WHILE
                && currentTokenType() != tokenType.resword_DO
                && currentTokenType() != tokenType.resword_FOR
                && currentTokenType() != tokenType.resword_FOREACH
                && currentTokenType() != tokenType.resword_SWITCH
                && !isUnaryToken()
                && currentTokenType() != tokenType.resword_RETURN
                && currentTokenType() != tokenType.resword_CONTINUE
                && currentTokenType() != tokenType.resword_BREAK
                && currentTokenType() != tokenType.ID
                && currentTokenType() != tokenType.symbol_EndOfStatement) return;
                BasicStatement();
                ListBasicStatements();
        }


        private void ListParameterTypes()
        {
            if (!checkALLtypes()) return;
            advanceToken();
            Console.WriteLine("DEFINITION OF PARAMETERS");
            ID_Simple();
            if (currentTokenType() == tokenType.symbol_Assignator)
            {
                advanceToken();
                EXPRESSION();
            }
            ListParametersTypesP();
        }

        private void ListParametersTypesP()
        {
            if (currentTokenType() != tokenType.symbol_SEPARATOR) { return; }
            advanceToken();
            ListParameterTypes();

        }

        private void STRUCT_DECLARATION()
        {
            if (currentTokenType() != tokenType.resword_STRUCT) throwException();
            advanceToken();
            ID();
            STRUCT_SCOPE();
        }

        private void STRUCT_SCOPE()
        {
            if (currentTokenType() != tokenType.symbol_openCurlyBraces) { throwException();}
                advanceToken();

            ListDeclarations();

            if (currentTokenType() != tokenType.symbol_closeCurlyBraces) { throwException();}
                advanceToken();
        }

        private void ListDeclarations()
        {
            if (checkALLtypes() || currentTokenType() == tokenType.resword_STRUCT)
            {
                Declaration();
            }

        }

        private void Declaration()
        {
            if (checkALLtypes())
            {
                advanceToken();
                unaryList();
                ID();
                if (currentTokenType() == tokenType.symbol_openParenthesis)
                {
                    advanceToken();
                    ListParameterTypes();
                    if (currentTokenType() == tokenType.symbol_closeParenthesis)
                    {
                        advanceToken();
                        SCOPE();
                    }
                }
                else
                    accesorList();
                DECLARATION_variableP();
                EOS();
            }
            else if (currentTokenType() == tokenType.resword_STRUCT)
            {
                STRUCT_DECLARATION();
            }
        }

        private void CONSTANT_DECLARATION()
        {

            if (currentTokenType() == tokenType.resword_CONST)
                advanceToken();

            if (checkStandardAndVar())
                advanceToken();

            DECLARATION_variable();


        }

        private void DECLARATION_variable()
        {
            ID_Simple();
            DECLARATION_variableP();
        }

        private void DECLARATION_variableP()
        {

            if (currentTokenType() == tokenType.symbol_Assignator)
            {
                EQUAL();
                EXPRESSION();
                SEPARATOR();
                DECLARATION_variable();
            }
            else if (currentTokenType() == tokenType.symbol_SEPARATOR)
            {
                SEPARATOR();
                DECLARATION_variable();
            }

        }

        private void SEPARATOR()
        {
            if (currentTokenType() == tokenType.symbol_SEPARATOR)
            {
                advanceToken();
            }
        }

        private void EQUAL()
        {
            if (currentTokenType() == tokenType.symbol_Assignator)
            {
                advanceToken();
            }
        }

        private void ID()
        {
            if (currentTokenType() == tokenType.ID)
            {
                advanceToken();
            }
        }

        private void ID_Simple()
        {
            unaryList();
            ID();
            accesorList();
        }

        private void accesorList()
        {

            var result = accesor();
            if (result)
            {
                accesorList();
            }
        }
        private bool accesor()
        {
            if (currentTokenType() == tokenType.symbol_Accessor)
            {
                advanceToken();
                ID();
                return true;
            }
            else if (currentTokenType() == tokenType.symbol_arrayOpen)
            {
                advanceToken();
                SEPARATOR();
                if (currentTokenType() == tokenType.symbol_arrayOpen)
                {
                    advanceToken();
                    return true;
                }

            }
            return false;
        }

        private void unaryList()
        {
            var result = unary();
            if (result)
            {
                unaryList();
            }
        }
        private bool unary()
        {
            if (isUnaryToken())
            {
                advanceToken();
                return true;
            }
            return false;
        }

        private void EOS()
        {
            if (currentTokenType() == tokenType.symbol_EndOfStatement)
            {
                advanceToken();
            }
        }

        private void EXPRESSION()
        {
            TERM_OR();
        }

        private void TERM_OR()
        {
            TERM_AND();
            TERM_ORP();
        }
        private void TERM_ORP()
        {
            if (currentTokenType() != tokenType.logicaloper_OR)
            {
                return;
            }
            advanceToken();
            TERM_AND();
            TERM_ORP();

        }

        private void TERM_AND()
        {
            TERM_bOR();
            TERM_ANDP();
        }
        private void TERM_ANDP()
        {
            if (currentTokenType() != tokenType.logicaloper_AND)
            {
                return;
            }
            advanceToken();
            TERM_bOR();
            TERM_ANDP();

        }

        private void TERM_bOR()
        {
            TERM_bXOR();
            TERM_bORP();
        }
        private void TERM_bORP()
        {
            if (currentTokenType() != tokenType.bitoper_OR)
            {
                return;
            }
            advanceToken();
            TERM_bXOR();
            TERM_bORP();
        }

        private void TERM_bXOR()
        {
            TERM_bAND();
            TERM_bXORP();
        }
        private void TERM_bXORP()
        {

            if (currentTokenType() != tokenType.bitoper_XOR)
            {
                return;
            }
            advanceToken();
            TERM_bAND();
            TERM_bXORP();

        }

        private void TERM_bAND()
        {
            TERM_EQUALITY();
            TERM_bANDP();

        }
        private void TERM_bANDP()
        {
            if (currentTokenType() != tokenType.bitoper_AND)
            {
                return;
            }
            advanceToken();
            TERM_EQUALITY();
            TERM_bANDP();
        }

        private void TERM_EQUALITY()
        {
            TERM_RELATIONAL();
            TERM_EQUALITYP();
        }
        private void TERM_EQUALITYP()
        {

            if (currentTokenType() != tokenType.reloper_COMPARE
                && currentTokenType() != tokenType.reloper_NOTEQUAL)
            {
                return;
            }
            advanceToken();
            TERM_RELATIONAL();
            TERM_EQUALITYP();
        }

        private void TERM_RELATIONAL()
        {
            TERM_SHIFT();
            TERM_RELATIONALP();
        }
        private void TERM_RELATIONALP()
        {
            if (currentTokenType() != tokenType.reloper_LESSTHAN
                && currentTokenType() != tokenType.reloper_LESSOREQUAL
                && currentTokenType() != tokenType.reloper_GREATERTHAN
                && currentTokenType() != tokenType.reloper_GREATEROREQUAL)
            {
                return;
            }
            advanceToken();
            TERM_SHIFT();
            TERM_RELATIONALP();
        }

        private void TERM_SHIFT()
        {
            ARITHMETIC();
            TERM_SHIFTP();
        }
        private void TERM_SHIFTP()
        {
            if (currentTokenType() != tokenType.bitoper_LEFTSHIFT
                && currentTokenType() != tokenType.bitoper_RIGHTSHIFT)
            {
                return;
            }
            advanceToken();
            ARITHMETIC();
            TERM_SHIFTP();
        }

        private void ARITHMETIC()
        {
            TERM();
            ARITHMETICP();
        }
        private void ARITHMETICP()
        {
            if (currentTokenType() != tokenType.oper_ADDITION
                && currentTokenType() != tokenType.oper_SUBSTRACTION)
            {
                return;
            }
            advanceToken();
            TERM();
            ARITHMETICP();
        }

        private void TERM()
        {
            FACTOR();
            TERMP();
        }
        private void TERMP()
        {
            if (currentTokenType() != tokenType.oper_MULTIPLICATION
                && currentTokenType() != tokenType.oper_DIVISION
                && currentTokenType() != tokenType.oper_MODULUS)
            {
                return;
            }
            advanceToken();
            FACTOR();
            TERMP();
        }

        private void FACTOR()
        {
            if (isLITERAL())
            {
                advanceToken();
                return;
            }
            if (isUnaryToken() || currentTokenType() == tokenType.ID)
            {
                ID_EXPRESSION();
                return;
            }
            if (currentTokenType() == tokenType.symbol_openParenthesis)
            {
                EXPRESSION();
                if (currentTokenType() == tokenType.symbol_closeParenthesis)
                {
                    return;
                }
            }
            throwException();
        }

        private void ID_EXPRESSION()
        {
            ID_Simple();
            ID_EXPRESSIONP();
        }
        private void ID_EXPRESSIONP()
        {
            if (currentTokenType() != tokenType.symbol_openParenthesis)
            {
                return;
            }
            advanceToken();
            ListParameters();
            if (currentTokenType() != tokenType.symbol_closeParenthesis){   throwException();}
            advanceToken();
        }

        private void ListParameters()
        {
            try
            {
                EXPRESSION();
                ListParametersP();
            }
            catch (Exception)
            {
                Console.Write("Parameterless FUNCTIONCALL AT" + _currentToken._coordinates + " ");
                return;
            }
        }
        private void ListParametersP()
        {
            if (currentTokenType() != tokenType.symbol_SEPARATOR) { return;}
            advanceToken();
            EXPRESSION();
            ListParametersP();
        }


        bool isLITERAL()
        {
            if (currentTokenType() == tokenType.literal_BOOL
                || currentTokenType() == tokenType.literal_CHARACTER
                || currentTokenType() == tokenType.literal_DATE
                || currentTokenType() == tokenType.literal_FLOAT
                || currentTokenType() == tokenType.literal_DATE
                || currentTokenType() == tokenType.literal_HEXADECIMAL
                || currentTokenType() == tokenType.literal_NUMBER
                || currentTokenType() == tokenType.literal_OCTAL
                || currentTokenType() == tokenType.literal_STRING)
            {
                return true;
            }
            return false;
        }
    }
}
