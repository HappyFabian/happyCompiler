using lexiCONOMICON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using treeHOUSE;

namespace parSEER
{

    public class parserEngine
    {
       List<baseSTATEMENT> _generatedCodeNodes = new List<baseSTATEMENT>();

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
            _generatedCodeNodes = Code();
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

        private List<baseSTATEMENT> Code()
        {
            var statementList = new List<baseSTATEMENT>();
            StatementList(statementList);
            return statementList;
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

        private void StatementList(List<baseSTATEMENT> list)
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
                && currentTokenType() != tokenType.symbol_HASHTAG
                && currentTokenType() != tokenType.HTML_TOKEN
                && currentTokenType() != tokenType.ID
                && currentTokenType() != tokenType.symbol_EndOfStatement) return;
            var statement = Statement();
            list.Add(statement);
            StatementList(list);
        }
        private baseSTATEMENT Statement()
        {
            if (currentTokenType() == tokenType.resword_CONST)
            {
                CONSTANT_DECLARATION();
                EOS();
            }
            if (_currentToken._type == tokenType.HTML_TOKEN)
            {
              advanceToken();
            }
            if (_currentToken._type == tokenType.symbol_HASHTAG)
            {
                IncludeStatement();
            }
            else if (currentTokenType() == tokenType.resword_STRUCT)
            {
                STRUCT_DECLARATION();
            }
            else if (currentTokenType() == tokenType.resword_ENUM)
            {
                ENUM_DECLARATION();
            }
            else if (checkALLtypes())
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
                EOS();
               
            }
            else if (currentTokenType() == tokenType.resword_IF)
            {
                IF_statement(); 
            }
            else if (currentTokenType() == tokenType.resword_SWITCH)
            {
                SWITCH_statement();
            }
            else if (currentTokenType() == tokenType.resword_WHILE)
            {
                WHILE_statement();
            }
            else if (currentTokenType() == tokenType.resword_DO)
            {
                DO_WHILE_statement();
                EOS();
            }
            else if (currentTokenType() == tokenType.resword_FOR)
            {
                FOR_statement();
            }
            else if (currentTokenType() == tokenType.resword_FOREACH)
            {
                FOREACH_STATEMENT();
            }
            else if (currentTokenType() == tokenType.ID || isUnaryToken() )
            {
                try
                {
                    ASSIGNATION_statement();
                }
                catch (Exception)
                {
                    try
                    {
                        ID_EXPRESSIONP();
                    }
                    catch (Exception)
                    {
                        throwException();
                    }
                }
                EOS();
            }
            else if (currentTokenType() == tokenType.symbol_EndOfStatement)
            {
                EOS();
            }

        }

        private void FOREACH_STATEMENT()
        {
            if (currentTokenType() != tokenType.resword_FOREACH){return;}
            advanceToken();
            if (currentTokenType() != tokenType.symbol_openParenthesis)
            {
                throwException();
            }
            advanceToken();
            if(!checkALLtypes()){ throwException();}
            advanceToken();
            ID_Simple();
            if (currentTokenType() != tokenType.symbol_doublePoints) { throwException();}
            advanceToken();
            ID_Simple();
            if (currentTokenType() != tokenType.symbol_closeParenthesis)
            {
                throwException();
            }
            advanceToken();
            SCOPE();
        }
        private void FOR_statement()
        {
            if (currentTokenType() != tokenType.resword_FOR) {return; }
            advanceToken();
            if (currentTokenType() != tokenType.symbol_openParenthesis)
            {
                throwException();
            }
            advanceToken();
            FOR_EXPRESSION();
            FOR_EXPRESSION();
            FOR_EXPRESSION();
            if (currentTokenType() != tokenType.symbol_closeParenthesis)
            {
                throwException();
            }
            advanceToken();
            SCOPE();
        }
        private void FOR_EXPRESSION()
        {
            if (checkStandardAndVar()) { advanceToken(); DECLARATION_variable(); EOS();}
            else if (currentTokenType() == tokenType.ID || isUnaryToken())
            {
                ASSIGNATION_statement();
                EOS();
            }
            else
            {
                EXPRESSION(); EOS();
            }
        }
        private void DO_WHILE_statement()
        {
            if (currentTokenType() != tokenType.resword_DO) { return; }
            advanceToken();
            SCOPE();
            if (currentTokenType() != tokenType.resword_WHILE) { return; }
            advanceToken();

            if (currentTokenType() != tokenType.symbol_openParenthesis)
            {
                throwException();
            }
            advanceToken();
            EXPRESSION();
            if (currentTokenType() != tokenType.symbol_closeParenthesis)
            {
                throwException();
            }
            advanceToken();
        }
        private void WHILE_statement()
        {
            if (currentTokenType() != tokenType.resword_WHILE){ return;}
            advanceToken();
            if (currentTokenType() != tokenType.symbol_openParenthesis){ throwException();}
            advanceToken();
            EXPRESSION();
            if (currentTokenType() != tokenType.symbol_closeParenthesis) { throwException();}
            advanceToken();
            SCOPE();
        }
        private void ASSIGNATION_statement()
        {
            ID_Simple();
            if (currentTokenType() != tokenType.symbol_Assignator &&
                currentTokenType() != tokenType.symbol_operAssignator)
            {
                throwException();
            }
            advanceToken();
            EXPRESSION();

        }
        private void SWITCH_statement()
        {
            if(currentTokenType() != tokenType.resword_SWITCH) { return;}
            advanceToken();
            if (currentTokenType() != tokenType.symbol_openParenthesis)
            {
                throwException();
            }
            advanceToken();
            ID_Simple();
            if (currentTokenType() != tokenType.symbol_closeParenthesis)
            {
                throwException();
            }
           
            advanceToken();

            SWITCH_SCOPE();
        }
        private void SWITCH_SCOPE()
        {
            if(currentTokenType() != tokenType.symbol_openCurlyBraces) { throwException();}
            advanceToken();
            LISTCASE();
            DEFAULTCASE();
            if (currentTokenType() != tokenType.symbol_closeCurlyBraces) { throwException();}
            advanceToken();


        }
        private void DEFAULTCASE()
        {
            if(currentTokenType() != tokenType.resword_DEFAULT) { return;}
                advanceToken();
            if (currentTokenType() != tokenType.symbol_doublePoints) { throwException(); }
                advanceToken();
            if (currentTokenType() != tokenType.symbol_openCurlyBraces) { throwException(); }
            advanceToken();
            ListBasicStatements();
            if (currentTokenType() != tokenType.symbol_closeCurlyBraces) { throwException(); }
            advanceToken();

        }
        private void LISTCASE()
        {
             CASE();
        }
        private void CASE()
        {
            if(currentTokenType() != tokenType.resword_CASE) { return;}
            advanceToken();
            EXPRESSION();
            if(currentTokenType() != tokenType.symbol_doublePoints) { throwException();}
            advanceToken();

            if (currentTokenType() != tokenType.symbol_openCurlyBraces) { throwException(); }
            advanceToken();
            ListBasicStatements();

            if (currentTokenType() != tokenType.symbol_closeCurlyBraces) { throwException(); }
            advanceToken();


            LISTCASE();
        }
        private void IF_statement()
        {
            if (currentTokenType() != tokenType.resword_IF)
            {
                return;
            }
            advanceToken();
            if (currentTokenType() != tokenType.symbol_openParenthesis)
            {
                throwException();
            }
            advanceToken();
            EXPRESSION();
            if (currentTokenType() != tokenType.symbol_closeParenthesis)
            {
                throwException();
            }
            advanceToken();
            SCOPE();
            ELSE_statement();
        }
        private void ELSE_statement()
        {
            if (currentTokenType() != tokenType.resword_ELSE)
            {
                return;
            }
            advanceToken();
            SCOPE();
        }
        private void ENUM_DECLARATION()
        {
            if (currentTokenType() != tokenType.resword_ENUM) return;

            advanceToken();
            if (currentTokenType() != tokenType.ID)
            {
                throwException();
            }
            advanceToken();

            if (currentTokenType() != tokenType.symbol_openCurlyBraces)
            {
                throwException();
            }
            advanceToken();
            ListEnums();

            if (currentTokenType() != tokenType.symbol_closeCurlyBraces)
            {
                throwException();
            }
            advanceToken();
        }
        private void ListEnums()
        {
            if (currentTokenType() != tokenType.ID) return;
            advanceToken();
            if (currentTokenType() == tokenType.symbol_Assignator)
            {
                advanceToken();
                EXPRESSION();
            }
            if (currentTokenType() == tokenType.symbol_SEPARATOR)
            {
                advanceToken();
                ListEnums();
            }

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
                    advanceToken();
                    DECLARATION_variable();
                    EOS();
            }
            else if (_currentToken._type == tokenType.HTML_TOKEN)
            {
                advanceToken();
            }
            else if (currentTokenType() == tokenType.resword_IF)
            {
                IF_statement();
            }
            else if (currentTokenType() == tokenType.resword_SWITCH)
            {
                SWITCH_statement();
            }
            else if (currentTokenType() == tokenType.resword_WHILE)
            {
                WHILE_statement();
            }
            else if (currentTokenType() == tokenType.resword_DO)
            {
                DO_WHILE_statement();
                EOS();
            }
            else if (currentTokenType() == tokenType.resword_FOR)
            {
                FOR_statement();
            }
            else if (currentTokenType() == tokenType.resword_FOREACH)
            {
                FOREACH_STATEMENT();
            }
            else if (isUnaryToken() || currentTokenType() == tokenType.ID)
            {
                try
                {
                    ASSIGNATION_statement();
                }
                catch (Exception)
                {
                    try
                    {
                        ID_EXPRESSIONP();
                    }
                    catch (Exception)
                    {
                        throwException();
                    }
                }
                EOS();
            }
            else if (currentTokenType() == tokenType.resword_RETURN)
            {
                RETURN_statement();
                EOS();
            }
            else if (currentTokenType() == tokenType.resword_CONTINUE)
            {
                CONTINUE_statement();
                EOS();
            }
            else if (currentTokenType() == tokenType.resword_BREAK)
            {
                BREAK_statement(); EOS();
            }
            else if (currentTokenType() == tokenType.symbol_EndOfStatement)
            {
                EOS();
            }
        }
        private void RETURN_statement()
        {
            if (currentTokenType() != tokenType.resword_RETURN){ return;}
            advanceToken();

            if (currentTokenType() != tokenType.symbol_EndOfStatement)
            {
                EXPRESSION();
            }
        }
        private void BREAK_statement()
        {
            if (currentTokenType() != tokenType.resword_BREAK)
            {
                return;
            }
            advanceToken();
        }
        private void CONTINUE_statement()
        {
            if (currentTokenType() != tokenType.resword_CONTINUE)
            {
                return;
            }
            advanceToken();
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
 

        private tokenObject ID()
        {
            if (currentTokenType() == tokenType.ID)
            {
                var id = _currentToken;
                advanceToken();
                return id;
            }
            return null;
        }
        private void ID_Simple(idEXPRESSION_NODE node)
        {
            unaryList(node);
            var idResult = ID();
            if (idResult == null)
            {
                throwException();
            }

            accesorList(node);
        }
        private void accesorList(idEXPRESSION_NODE node)
        {

            var result = accesor(node);
            if (result)
            {
                accesorList(node);
            }
        }
        private bool accesor(idEXPRESSION_NODE node)
        {
            if (currentTokenType() == tokenType.symbol_Accessor || currentTokenType() == tokenType.symbol_structAccessor)
            {
                var symbol = _currentToken._type;
                advanceToken();
                var whatId = ID();
                if (whatId == null) { throwException(); }
                var accesorNode = new ACCESSOR_NODE();
                accesorNode.id = whatId;
                accesorNode.type = symbol;
                node.accessorList.Add(accesorNode);

                return true;
            }
            else if (currentTokenType() == tokenType.symbol_arrayOpen)
            {
                advanceToken();
                var arrayNode = new ARRAY_NODE();
                var whatExpression = EXPRESSION();
                arrayNode.expressions.Add(whatExpression);
                ARRAYCONTENT(arrayNode);
                if (currentTokenType() == tokenType.symbol_arrayClose)
                {
                    node.accessorList.Add(arrayNode);
                    advanceToken();
                    return true;
                }

            }
            return false;
        }
        private void ARRAYCONTENT(ARRAY_NODE arrayNode)
        {
            if (_currentToken._type == tokenType.symbol_SEPARATOR)
            {
                SEPARATOR();
                var whatExpression = EXPRESSION();
                arrayNode.expressions.Add(whatExpression);
                ARRAYCONTENT(arrayNode);
            }

        }
        private void unaryList(idEXPRESSION_NODE node)
        {
            var result = unary(node);
            if (result)
            {
                unaryList(node);
            }
        }
        private bool unary(idEXPRESSION_NODE node)
        {
            if (isUnaryToken())
            {
                var unaryNode = new UNARY_NODE();
                unaryNode.value = _currentToken;
                node.unaryList.Add(unaryNode);
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
        private EXPRESSION_NODE EXPRESSION()
        {
            var ExpressionNode = new EXPRESSION_NODE();
            ExpressionNode.termOR = TERM_OR();
            return ExpressionNode;
        }
        private OR_NODE TERM_OR()
        {
            var OrNode = new OR_NODE();
            OrNode.addAnd(TERM_AND());
            TERM_ORP(OrNode);
            return OrNode;
        }
        private void TERM_ORP(OR_NODE node)
        {
            if (currentTokenType() != tokenType.logicaloper_OR)
            {
                return;
            }
            advanceToken();
            node.addAnd(TERM_AND());
            TERM_ORP(node);

        }
        private AND_NODE TERM_AND()
        {
            var AndNode = new AND_NODE();
            AndNode.addbOR(TERM_bOR());
            TERM_ANDP(AndNode);
            return AndNode;
        }
        private void TERM_ANDP(AND_NODE node)
        {
            if (currentTokenType() != tokenType.logicaloper_AND)
            {
                return;
            }
            advanceToken();
            node.addbOR(TERM_bOR());
            TERM_ANDP(node);

        }
        private bOR_NODE TERM_bOR()
        {
            var bOrNode = new bOR_NODE();
            bOrNode.addbXOR(TERM_bXOR());
            TERM_bORP(bOrNode);
            return bOrNode;
        }
        private void TERM_bORP(bOR_NODE node)
        {
            if (currentTokenType() != tokenType.bitoper_OR)
            {
                return;
            }
            advanceToken();
            node.addbXOR(TERM_bXOR());
            TERM_bORP(node);
        }
        private bXOR_NODE TERM_bXOR()
        {
            var bXOrNode = new bXOR_NODE();
            bXOrNode.addbAND(TERM_bAND());
            TERM_bXORP(bXOrNode);
            return bXOrNode;
        }
        private void TERM_bXORP(bXOR_NODE node)
        {

            if (currentTokenType() != tokenType.bitoper_XOR)
            {
                return;
            }
            advanceToken();
            node.addbAND(TERM_bAND());
            TERM_bXORP(node);

        }
        private bAND_NODE TERM_bAND()
        {
            var bAndNode = new bAND_NODE();
            bAndNode.addEqual(TERM_EQUALITY());
            TERM_bANDP(bAndNode);
            return bAndNode;

        }
        private void TERM_bANDP(bAND_NODE node)
        {
            if (currentTokenType() != tokenType.bitoper_AND)
            {
                return;
            }
            advanceToken();
            node.addEqual(TERM_EQUALITY());
            TERM_bANDP(node);
        }
        private EQUAL_NODE TERM_EQUALITY()
        {
            var equalNode = new EQUAL_NODE();
            equalNode.addRelation(TERM_RELATIONAL());
            TERM_EQUALITYP(equalNode);
            return equalNode;
        }
        private void TERM_EQUALITYP(EQUAL_NODE node)
        {

            if (currentTokenType() != tokenType.reloper_COMPARE
                && currentTokenType() != tokenType.reloper_NOTEQUAL)
            {
                return;
            }
            var symbol = _currentToken;
            advanceToken();
            node.addRelation(TERM_RELATIONAL(), symbol._type);
            TERM_EQUALITYP(node);
        }
        private RELATION_NODE TERM_RELATIONAL()
        {
            var relationalNode = new RELATION_NODE();
            relationalNode.addShift(TERM_SHIFT());
            TERM_RELATIONALP(relationalNode);
            return relationalNode;
        }
        private void TERM_RELATIONALP(RELATION_NODE node)
        {
            if (currentTokenType() != tokenType.reloper_LESSTHAN
                && currentTokenType() != tokenType.reloper_LESSOREQUAL
                && currentTokenType() != tokenType.reloper_GREATERTHAN
                && currentTokenType() != tokenType.reloper_GREATEROREQUAL)
            {
                return;
            }
            var symbol = _currentToken;
            advanceToken();
            node.addShift(TERM_SHIFT(), symbol._type);
            TERM_RELATIONALP(node);
        }
        private SHIFT_NODE TERM_SHIFT()
        {
            var shiftNode = new SHIFT_NODE();
            shiftNode.addArithmetica(ARITHMETIC());
            TERM_SHIFTP(shiftNode);
            return shiftNode;
        }
        private void TERM_SHIFTP(SHIFT_NODE node)
        {
            if (currentTokenType() != tokenType.bitoper_LEFTSHIFT
                && currentTokenType() != tokenType.bitoper_RIGHTSHIFT)
            {
                return;
            }
            var symbol = _currentToken;
            advanceToken();
            node.addArithmetica(ARITHMETIC(), symbol._type);
            TERM_SHIFTP(node);
        }
        private ARITHMETIC_NODE ARITHMETIC()
        {
            var arithmeticNode = new ARITHMETIC_NODE();
            arithmeticNode.addTerm(TERM());
            ARITHMETICP(arithmeticNode);
            return arithmeticNode;
        }
        private void ARITHMETICP(ARITHMETIC_NODE node)
        {
            if (currentTokenType() != tokenType.oper_ADDITION
                && currentTokenType() != tokenType.oper_SUBSTRACTION)
            {
                return;
            }
            var symbol = _currentToken;
            advanceToken();
            node.addTerm(TERM());
            ARITHMETICP(node);
        }
        private TERM_NODE TERM()
        {
            var termNode = new TERM_NODE();
            termNode.addFactor(FACTOR());
            TERMP(termNode);
            return termNode;
        }
        private void TERMP(TERM_NODE node)
        {
            if (currentTokenType() != tokenType.oper_MULTIPLICATION
                && currentTokenType() != tokenType.oper_DIVISION
                && currentTokenType() != tokenType.oper_MODULUS)
            {
                return;
            }
            var symbol = _currentToken;
            advanceToken();
            node.addFactor(FACTOR(), symbol._type);
            TERMP(node);
        }
        private FACTOR_NODE FACTOR()
        {
            var factorNode = new FACTOR_NODE();
            if (isLITERAL())
            {
                var literalNode = new LITERAL_NODE();
                literalNode.value = _currentToken;
                factorNode.valueNode = literalNode;
                advanceToken();
                return factorNode;
            }
            else if (isUnaryToken() || currentTokenType() == tokenType.ID)
            {
                var idExpressionNode = ID_EXPRESSION();
                factorNode.valueNode = idExpressionNode;
                return factorNode;
            }
            else if (currentTokenType() == tokenType.symbol_openParenthesis)
            {
                var expressionNode = EXPRESSION();
                if (currentTokenType() == tokenType.symbol_closeParenthesis)
                {
                    factorNode.valueNode = expressionNode;
                    return factorNode;
                }
            }
            throwException();
            return factorNode;
        }
        private idEXPRESSION_NODE ID_EXPRESSION()
        {
            var iDExpressionNode = new idEXPRESSION_NODE();
            ID_Simple(iDExpressionNode);
            ID_EXPRESSIONP(iDExpressionNode);
            return iDExpressionNode;
        }
        private void ID_EXPRESSIONP(idEXPRESSION_NODE node)
        {
            if (currentTokenType() != tokenType.symbol_openParenthesis)
            {
                return;
            }
            advanceToken();
            ListParameters(node);
            if (currentTokenType() != tokenType.symbol_closeParenthesis) { throwException(); }
            advanceToken();
        }
        private void ListParameters(idEXPRESSION_NODE node)
        {
            try
            {
                var expression = EXPRESSION();
                node.parameters.Add(expression);
                ListParametersP(node);
            }
            catch (Exception)
            {
                Console.Write("Parameterless FUNCTIONCALL AT" + _currentToken._coordinates + " ");
                return;
            }
        }
        private void ListParametersP(idEXPRESSION_NODE node)
        {
            if (currentTokenType() != tokenType.symbol_SEPARATOR) { return; }
            advanceToken();
            var expression = EXPRESSION();
            node.parameters.Add(expression);
            ListParametersP(node);
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
