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
            Console.WriteLine("Code Parsing Finished.");
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

        private List<baseSTATEMENT> Code()
        {
            var statementList = new List<baseSTATEMENT>();
            StatementList(statementList);
            return statementList;
        }
        private baseSTATEMENT IncludeStatement()
        {
            var returnStatement = new INCLUDE_STATEMENT();
            advanceToken();
            if (currentTokenType() != tokenType.resword_INCLUDE)
            {
                throwException();
                return null;
            }
            advanceToken();
            if (currentTokenType() != tokenType.literal_FILENAME && currentTokenType() != tokenType.literal_STRING)
            {
                throwException();
                return null;
            }
            else
            {
                returnStatement.filename = _currentToken;
                advanceToken();
                return returnStatement;
            }
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
            //Done
            if (currentTokenType() == tokenType.resword_CONST)
            {
                var returnStatement = CONSTANT_DECLARATION();
                EOS();
                return returnStatement;
            }
            //Done
            else if (_currentToken._type == tokenType.HTML_TOKEN)
            {
                var returnStatement = HTML_STATEMENT();
                advanceToken();
                return returnStatement;
            }
            //Done
            else if (_currentToken._type == tokenType.symbol_HASHTAG)
            {
               var returnStatement = IncludeStatement();
               return returnStatement;
            }
            //Done
            else if (currentTokenType() == tokenType.resword_STRUCT)
            {
               var returnStatement =  STRUCT_DECLARATION();
               return returnStatement;
            }
            else if (currentTokenType() == tokenType.resword_ENUM)
            {
               return ENUM_DECLARATION();
            }
            //Done
            else if (checkStandardAndVar())
            {

                    var type = _currentToken;
                    advanceToken();
                    var idExpressionNode = new idEXPRESSION_NODE();
                    unaryList(idExpressionNode);
                    idExpressionNode.id = ID();
                    if (currentTokenType() == tokenType.symbol_openParenthesis)
                    {
                        advanceToken();
                        var listparameterstype = ListParameterTypes(new List<baseNODE>());
                        if (currentTokenType() == tokenType.symbol_closeParenthesis)
                        {
                            advanceToken();
                            var scope = SCOPE();
                            var functionDeclaration = new functionDECLARATION_STATEMENT();
                            functionDeclaration.returnType = type;
                            functionDeclaration.idExpressionNode = idExpressionNode;
                            functionDeclaration.listparameterstype = listparameterstype;
                            functionDeclaration.scope = scope;
                            return functionDeclaration;
                        }
                    }
                    else
                        accesorList(idExpressionNode);
                    var expression = new EXPRESSION_NODE();
                    if (currentTokenType() == tokenType.symbol_Assignator)
                    {
                        advanceToken();
                        expression = EXPRESSION();
                    }
                    var declarationNode = new DECLARATION_STATEMENT();
                    declarationNode.type = type;
                    declarationNode.variableList.Add(new PARAMETERTYPE_NODE { idExpressionNode = idExpressionNode, value = expression });
                    DECLARATION_variableP(declarationNode);
                    EOS();
                    return declarationNode;  
            }
            //Done
            else if (currentTokenType() == tokenType.resword_IF)
            {
               return IF_statement(); 
            }
            //Done
            else if (currentTokenType() == tokenType.resword_SWITCH)
            {
               return SWITCH_statement();
            }
            //Done
            else if (currentTokenType() == tokenType.resword_WHILE)
            {
                return WHILE_statement();
            }
            //Done
            else if (currentTokenType() == tokenType.resword_DO)
            {
                var dowhileStatement = DO_WHILE_statement();
                EOS();
                return dowhileStatement;
            }
            //Done
            else if (currentTokenType() == tokenType.resword_FOR)
            {
                return FOR_statement();
            }
            //Done
            else if (currentTokenType() == tokenType.resword_FOREACH)
            {
              return FOREACH_STATEMENT();
            }
            //Done?
            else if (currentTokenType() == tokenType.ID || isUnaryToken() )
            {
                var result = FUNCTIONCALL_OR_ASSIGNATION();
                EOS();
                return result;
               
            }
            //Done
            else if (currentTokenType() == tokenType.symbol_EndOfStatement)
            {
                EOS();
                return new EMPTY_STATEMENT();
            }

            return null;
        }

        private baseSTATEMENT FUNCTIONCALL_OR_ASSIGNATION()
        {
            var idExpresion = new idEXPRESSION_NODE();
            ID_Simple(idExpresion);
            if (currentTokenType() == tokenType.symbol_openParenthesis)
            {
                advanceToken();
                ListParameters(idExpresion);
                if (currentTokenType() == tokenType.symbol_closeParenthesis)
                {
                    advanceToken();
                    var functionCall = new FUNCTIONCALL_STATEMENT();
                    functionCall.value = idExpresion;
                    return functionCall;
                }
            }
            else if (currentTokenType() == tokenType.symbol_Assignator ||
                     currentTokenType() == tokenType.symbol_operAssignator)
            {
                var assignationStatement = new ASSIGNATION_STATEMENT();
                assignationStatement.assignator = _currentToken;
                advanceToken();
                var paramtypenode = new PARAMETERTYPE_NODE();
                paramtypenode.idExpressionNode = idExpresion;
                paramtypenode.value = EXPRESSION();
                assignationStatement.assignation = paramtypenode;
                return assignationStatement;
            }
            return null;
        }

        private baseSTATEMENT HTML_STATEMENT()
        {
            return new HTML_STATEMENT {value = _currentToken};
        }

        private FOREACH_STATEMENT FOREACH_STATEMENT()
        {
            if (currentTokenType() != tokenType.resword_FOREACH){return null;}
            advanceToken();
            if (currentTokenType() != tokenType.symbol_openParenthesis)
            {
                throwException();
            }
            advanceToken();
            if(!checkALLtypes()){ throwException();}
            var type = _currentToken;
            advanceToken();
            var idExpression1 = new idEXPRESSION_NODE();
            ID_Simple(idExpression1);
            if (currentTokenType() != tokenType.symbol_doublePoints) { throwException();}
            advanceToken();
            var idExpression2 = new idEXPRESSION_NODE();
            ID_Simple(idExpression2);
            if (currentTokenType() != tokenType.symbol_closeParenthesis)
            {
                throwException();
            }
            advanceToken();
            var parametertypeNode = new PARAMETERTYPE_NODE();
            parametertypeNode.type = type;
            parametertypeNode.idExpressionNode = idExpression1;
            var foreachStatement = new FOREACH_STATEMENT();
            foreachStatement.typeparameter = parametertypeNode;
            foreachStatement.variable = idExpression2;
            foreachStatement.scope = SCOPE();
            return foreachStatement;
        }
        private FOR_STATEMENT FOR_statement()
        {
            if (currentTokenType() != tokenType.resword_FOR)
            { return null; }
            advanceToken();
            if (currentTokenType() != tokenType.symbol_openParenthesis)
            {
                throwException();
            }
            advanceToken();
            var forStatement = new FOR_STATEMENT();
            forStatement.firstExpression = FOR_EXPRESSION();
            forStatement.secondExpression = FOR_EXPRESSION();
            forStatement.thirdExpression = FOR_EXPRESSION();
            if (currentTokenType() != tokenType.symbol_closeParenthesis)
            {
                throwException();
            }
            advanceToken();
            forStatement.scope = SCOPE();
            return forStatement;
        }
        private FOREXPRESSION_NODE FOR_EXPRESSION()
        {
            var forexpressionNode = new FOREXPRESSION_NODE();
            
            if (checkStandardAndVar())
            {
                var declarationStatement = new DECLARATION_STATEMENT();
                declarationStatement.type = _currentToken;
                advanceToken();
                DECLARATION_variable(declarationStatement);
                EOS();
                forexpressionNode.value = declarationStatement;
            }
            else if (currentTokenType() == tokenType.ID || isUnaryToken())
            {
                forexpressionNode.value = ASSIGNATION_statement();
                EOS();
            }
            else
            {
                forexpressionNode.value = EXPRESSION(); EOS();
            }
            return forexpressionNode;
        }
        private WHILE_STATEMENT DO_WHILE_statement()
        {
            if (currentTokenType() != tokenType.resword_DO) { return null; }
            advanceToken();
            var whileStatement = new WHILE_STATEMENT();
            whileStatement.isDoWhile = true;
            whileStatement.scope = SCOPE();
            if (currentTokenType() != tokenType.resword_WHILE) { throwException(); }
            advanceToken();

            if (currentTokenType() != tokenType.symbol_openParenthesis)
            {
                throwException();
            }
            advanceToken();
           whileStatement.expression =  EXPRESSION();
            if (currentTokenType() != tokenType.symbol_closeParenthesis){throwException();}
            advanceToken();
            return whileStatement;
        }
        private WHILE_STATEMENT WHILE_statement()
        {
            if (currentTokenType() != tokenType.resword_WHILE){ return null;}
            advanceToken();
            if (currentTokenType() != tokenType.symbol_openParenthesis){ throwException();}
            advanceToken();

            var whileStatement = new WHILE_STATEMENT();
             
            whileStatement.expression = EXPRESSION();
            if (currentTokenType() != tokenType.symbol_closeParenthesis) { throwException();}
            advanceToken();
           whileStatement.scope = SCOPE();
            return whileStatement;
        }
        private ASSIGNATION_STATEMENT ASSIGNATION_statement()
        {
            var idExpresion = new idEXPRESSION_NODE();
            ID_Simple(idExpresion);
            if (currentTokenType() != tokenType.symbol_Assignator &&
                currentTokenType() != tokenType.symbol_operAssignator)
            {
                throwException();
            }
            var assignator = _currentToken;
            advanceToken();
            var parametertype = new PARAMETERTYPE_NODE();
            parametertype.idExpressionNode = idExpresion;
            parametertype.value = EXPRESSION();
            var assignationStatement = new ASSIGNATION_STATEMENT();
            assignationStatement.assignator = assignator;
            assignationStatement.assignation = parametertype;
            return assignationStatement;

        }
        private SWITCH_STATEMENT SWITCH_statement()
        {
            if(currentTokenType() != tokenType.resword_SWITCH) { return null; }
            advanceToken();
            if (currentTokenType() != tokenType.symbol_openParenthesis)
            {
                throwException();
            }
            advanceToken();
            var idExpression = new idEXPRESSION_NODE();
            ID_Simple(idExpression);
            if (currentTokenType() != tokenType.symbol_closeParenthesis)
            {
                throwException();
            }
           
            advanceToken();

            var switchStatement = new SWITCH_STATEMENT();
            switchStatement.id = idExpression;
            switchStatement.switchScope = SWITCH_SCOPE();
            return switchStatement;
        }
        private switchSCOPE_NODE SWITCH_SCOPE()
        {
            if(currentTokenType() != tokenType.symbol_openCurlyBraces) { throwException();}
            advanceToken();

            var listCase = new List<CASE_NODE>();
            LISTCASE(listCase);
            var defaultcaseNode = new CASE_NODE();
            DEFAULTCASE(defaultcaseNode);
            var switchscopeNode = new switchSCOPE_NODE();
            switchscopeNode.listCases = listCase;
            switchscopeNode.defaultCase = defaultcaseNode;
            if (currentTokenType() != tokenType.symbol_closeCurlyBraces) { throwException();}
            advanceToken();

            return switchscopeNode;
            
        }
        private void DEFAULTCASE(CASE_NODE node)
        {
            if(currentTokenType() != tokenType.resword_DEFAULT) { return;}
                advanceToken();
            if (currentTokenType() != tokenType.symbol_doublePoints) { throwException(); }
                advanceToken();
            if (currentTokenType() != tokenType.symbol_openCurlyBraces) { throwException(); }
            advanceToken();
            var listBasicStatemnt = new List<baseSTATEMENT>();
            ListBasicStatements(listBasicStatemnt);

            var caseNode = new CASE_NODE();
            caseNode.statements = listBasicStatemnt;
            if (currentTokenType() != tokenType.symbol_closeCurlyBraces) { throwException(); }
            advanceToken();

        }
        private void LISTCASE(List<CASE_NODE> list)
        {
            if(currentTokenType() != tokenType.resword_CASE) { return;}
            advanceToken();
            var expression = EXPRESSION();
            if(currentTokenType() != tokenType.symbol_doublePoints) { throwException();}
            advanceToken();

            if (currentTokenType() != tokenType.symbol_openCurlyBraces) { throwException(); }
            advanceToken();
            var listBasicStatemnt = new List<baseSTATEMENT>();
            ListBasicStatements(listBasicStatemnt);

            var caseNode = new CASE_NODE();
            caseNode.expression = expression; 
            caseNode.statements = listBasicStatemnt;

            if (currentTokenType() != tokenType.symbol_closeCurlyBraces) { throwException(); }
            advanceToken();

            list.Add(caseNode);
            LISTCASE(list);
        }
        private baseSTATEMENT IF_statement()
        {
            if (currentTokenType() != tokenType.resword_IF)
            {
                return null;
            }
            advanceToken();
            if (currentTokenType() != tokenType.symbol_openParenthesis)
            {
                throwException();
                return null;
            }
            var ifStatement = new IF_STATEMENT();
            
            advanceToken();
            ifStatement.expression = EXPRESSION();
            if (currentTokenType() != tokenType.symbol_closeParenthesis)
            {
                throwException();
            }
            advanceToken();
            ifStatement.scope = SCOPE();
            ifStatement.elseNode = ELSE_statement();
            return ifStatement;
        }
        private ELSE_NODE ELSE_statement()
        {
            if (currentTokenType() != tokenType.resword_ELSE)
            {
                return null;
            }
            advanceToken();
            var elseNode = new ELSE_NODE();
            elseNode.scope = SCOPE();
            return elseNode;
        }
        private ENUM_STATEMENT ENUM_DECLARATION()
        {
            if (currentTokenType() != tokenType.resword_ENUM) return null;

            advanceToken();
            if (currentTokenType() != tokenType.ID)
            {
                throwException();
            }
            var id = _currentToken;
            advanceToken();

            if (currentTokenType() != tokenType.symbol_openCurlyBraces)
            {
                throwException();
            }
            advanceToken();
            var enumStatement = new ENUM_STATEMENT();
            enumStatement.id = id;
           
            var list = new List<PARAMETERTYPE_NODE>();
            ListEnums(list);
            enumStatement.list = list;
            if (currentTokenType() != tokenType.symbol_closeCurlyBraces)
            {
                throwException();
            }
            advanceToken();
            return enumStatement;
            
        }
        private void ListEnums(List<PARAMETERTYPE_NODE> list)
        {
            if (currentTokenType() != tokenType.ID) return;
            var paramtypeNode = new PARAMETERTYPE_NODE();
            paramtypeNode.idExpressionNode = new idEXPRESSION_NODE {id = _currentToken};
            advanceToken();
            if (currentTokenType() == tokenType.symbol_Assignator)
            {
                advanceToken();
                paramtypeNode.value = EXPRESSION();
                list.Add(paramtypeNode);
            }
            if (currentTokenType() == tokenType.symbol_SEPARATOR)
            {
                advanceToken();
                ListEnums(list);
            }

        }     
        private RETURN_STATEMENT RETURN_statement()
        {
            if (currentTokenType() != tokenType.resword_RETURN){ return null;}
            advanceToken();
            var returnStatement = new RETURN_STATEMENT();
            if (currentTokenType() != tokenType.symbol_EndOfStatement)
            {
                returnStatement.expression = EXPRESSION();
            }
            return returnStatement;
        }
        private BREAK_STATEMENT BREAK_statement()
        {
            if (currentTokenType() != tokenType.resword_BREAK)
            {
                return null;
            }
            advanceToken();
            return new BREAK_STATEMENT();
        }
        private CONTINUE_STATEMENT CONTINUE_statement()
        {
            if (currentTokenType() != tokenType.resword_CONTINUE)
            {
                return null;
            }
            advanceToken();
            return new CONTINUE_STATEMENT();
        }
        private void ListBasicStatements(List<baseSTATEMENT> list)
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
                list.Add(BasicStatement());
                ListBasicStatements(list);
        }
        private baseSTATEMENT BasicStatement()
        {
            //Done
            if (checkStandardAndVar())
            {
                var type = _currentToken;
                advanceToken();
                var idExpressionNode = new idEXPRESSION_NODE();
                unaryList(idExpressionNode);
                idExpressionNode.id = ID();
                accesorList(idExpressionNode);
                var expression = new EXPRESSION_NODE();
                if (currentTokenType() == tokenType.symbol_Assignator)
                {
                    advanceToken();
                    expression = EXPRESSION();
                }
                var declarationNode = new DECLARATION_STATEMENT();
                declarationNode.type = type;
                declarationNode.variableList.Add(new PARAMETERTYPE_NODE { idExpressionNode = idExpressionNode, value = expression });
                DECLARATION_variableP(declarationNode);
                EOS();
                return declarationNode;
            }
            //Done
            else if (_currentToken._type == tokenType.HTML_TOKEN)
            {
                var returnStatement = HTML_STATEMENT();
                advanceToken();
                return returnStatement;
            }
            //Done      
            else if (currentTokenType() == tokenType.resword_IF)
            {
               return  IF_statement();
            }
            //Done
            else if (currentTokenType() == tokenType.resword_SWITCH)
            {
                return SWITCH_statement();
            }
            //Done
            else if (currentTokenType() == tokenType.resword_WHILE)
            {
              return WHILE_statement();
            }
            //Done
            else if (currentTokenType() == tokenType.resword_DO)
            {
                var dowhileStatement = DO_WHILE_statement();
                EOS();
                return dowhileStatement;
            }
            //Done
            else if (currentTokenType() == tokenType.resword_FOR)
            {
              return FOR_statement();
            }
            //Done
            else if (currentTokenType() == tokenType.resword_FOREACH)
            {
               return FOREACH_STATEMENT();
            }
            //Done?
            else if (isUnaryToken() || currentTokenType() == tokenType.ID)
            {
                var result = FUNCTIONCALL_OR_ASSIGNATION();
                EOS();
                return result;
            }
            //Done
            else if (currentTokenType() == tokenType.resword_RETURN)
            {
                var result = RETURN_statement();
                EOS();
                return result;
            }
            //Done
            else if (currentTokenType() == tokenType.resword_CONTINUE)
            {
                var result =  CONTINUE_statement();
                EOS();
                return result;
            }
            //Done
            else if (currentTokenType() == tokenType.resword_BREAK)
            {
                var result = BREAK_statement();
                EOS();
                return result;
            }
            //Done
            else if (currentTokenType() == tokenType.symbol_EndOfStatement)
            {
                EOS();
                return new EMPTY_STATEMENT();
            }
            return null;
        }
        private List<baseNODE> ListParameterTypes(List<baseNODE> list)
        {
            if (!checkALLtypes()) return list;

            var parametertypeNode = new PARAMETERTYPE_NODE();
            var type = _currentToken;
            parametertypeNode.type = type;
            advanceToken();
            var idExpressionNode = new idEXPRESSION_NODE();
            Console.WriteLine("DEFINITION OF PARAMETERS");
            ID_Simple(idExpressionNode);
            parametertypeNode.idExpressionNode = idExpressionNode;
            if (currentTokenType() == tokenType.symbol_Assignator)
            {
                advanceToken();
                var expressionNode = EXPRESSION();
                parametertypeNode.value = expressionNode;
            }
            list.Add(parametertypeNode);
            return ListParametersTypesP(list);
        }
        private List<baseNODE> ListParametersTypesP(List<baseNODE> list)
        {
            if (currentTokenType() != tokenType.symbol_SEPARATOR) { return list; }
            advanceToken();
            return ListParameterTypes(list);

        }
        private baseSTATEMENT STRUCT_DECLARATION()
        {
            var structNode = new STRUCT_STATEMENT();
            if (currentTokenType() != tokenType.resword_STRUCT) throwException();
            advanceToken();
            var idExpresionNode = new idEXPRESSION_NODE();
            unaryList(idExpresionNode);
            idExpresionNode.id = ID();

            var structScope = STRUCT_SCOPE();
            structNode.idExpression = idExpresionNode;
            structNode.scope = structScope;
            return structNode;
        }
        private baseNODE STRUCT_SCOPE()
        {
            if (currentTokenType() != tokenType.symbol_openCurlyBraces) { throwException();}
                advanceToken();
            var structNode = new structSCOPE__NODE();
            structNode.listOfDeclarations = ListDeclarations(new List<baseSTATEMENT>());

            if (currentTokenType() != tokenType.symbol_closeCurlyBraces) { throwException();}
                advanceToken();
            return structNode;
        }
        private List<baseSTATEMENT> ListDeclarations(List<baseSTATEMENT> node)
        {
            if (checkALLtypes())
            {
                var declaration =  Declaration();
                node.Add(declaration);
                ListDeclarations(node);
            }
            return node;

        }
        private baseSTATEMENT Declaration()
        {
            if (checkStandardAndVar())
            {

                var type = _currentToken;
                advanceToken();
                var idExpressionNode = new idEXPRESSION_NODE();
                unaryList(idExpressionNode);
                idExpressionNode.id = ID();
                if (currentTokenType() == tokenType.symbol_openParenthesis)
                {
                    advanceToken();
                    var listparameterstype = ListParameterTypes(new List<baseNODE>());
                    if (currentTokenType() == tokenType.symbol_closeParenthesis)
                    {
                        advanceToken();
                        var scope = SCOPE();
                        var functionDeclaration = new functionDECLARATION_STATEMENT();
                        functionDeclaration.returnType = type;
                        functionDeclaration.idExpressionNode = idExpressionNode;
                        functionDeclaration.listparameterstype = listparameterstype;
                        functionDeclaration.scope = scope;
                        return functionDeclaration;
                    }
                }
                else
                accesorList(idExpressionNode);
                var expression = new EXPRESSION_NODE();
                if (currentTokenType() == tokenType.symbol_Assignator)
                {
                    advanceToken();
                    expression = EXPRESSION();
                }
                var declarationNode = new DECLARATION_STATEMENT();
                declarationNode.type = type;
                declarationNode.variableList.Add(new  PARAMETERTYPE_NODE {idExpressionNode = idExpressionNode,value = expression});
                DECLARATION_variableP(declarationNode);
                EOS();
                return declarationNode;

            }
            else if (currentTokenType() == tokenType.resword_STRUCT)
            {
               return STRUCT_DECLARATION();
            }
            throwException();
            return null;
        }
        private SCOPE_NODE SCOPE()
        {
            var scopeNode = new SCOPE_NODE();
            var basicStatementList = new List<baseSTATEMENT>();
            if (currentTokenType() == tokenType.symbol_openCurlyBraces)
            {
                advanceToken();
                ListBasicStatements(basicStatementList);
                if (currentTokenType() == tokenType.symbol_closeCurlyBraces)
                {
                    advanceToken();
                    scopeNode.basicStatementList = basicStatementList;
                    return scopeNode;
                }
                throwException();
            }
            else
            {
               basicStatementList.Add(BasicStatement());
               scopeNode.basicStatementList = basicStatementList;
               return scopeNode;
            }
            return null;
        }
        private baseSTATEMENT CONSTANT_DECLARATION()
        {

            if (currentTokenType() == tokenType.resword_CONST)
                advanceToken();

            if (checkStandardAndVar())
                advanceToken();
            var declarationStatement = new DECLARATION_STATEMENT();
            declarationStatement.isFirstConstant = true;
            DECLARATION_variable(declarationStatement);
            return declarationStatement;
        }
        private void DECLARATION_variable(DECLARATION_STATEMENT statement)
        {
            var paramtypeNode = new PARAMETERTYPE_NODE();
            var idExpressionNode = new idEXPRESSION_NODE();
            ID_Simple(idExpressionNode);
            paramtypeNode.idExpressionNode = idExpressionNode;
            if (currentTokenType() == tokenType.symbol_Assignator)
            {
                EQUAL();
                var expression = EXPRESSION();
                paramtypeNode.value = expression;
                statement.variableList.Add(paramtypeNode);
            }
            DECLARATION_variableP(statement);
        }
        private void DECLARATION_variableP(DECLARATION_STATEMENT statement)
        {

            if (currentTokenType() == tokenType.symbol_SEPARATOR)
            {
                SEPARATOR();
                DECLARATION_variable(statement);
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
