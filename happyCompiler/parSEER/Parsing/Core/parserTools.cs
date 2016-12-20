using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using lexiCONOMICON;
using parSEER.Interpretative.Values;
using parSEER.Parsing;
using parSEER.Semantics.Tree.Expression;
using parSEER.Semantics.Tree.Expression.literalNodes;
using parSEER.Semantics.Tree.Expression.operationNodes;
using parSEER.Semantics.Tree.Expression.operationNodes.idcalculationNodes;
using parSEER.Semantics.Tree.Sentences;
using parSEER.Semantics.Tree.Statements;
using parSEER.Semantics.Types;
using parSEER.Semantics.Types.literalTypes;

namespace parSEER
{

    public class parserTools
    {
        
        public parserHolder holder;

        public parserTools(List<tokenObject> generatedTokensList)
        {
            holder = new parserHolder(generatedTokensList);
        }

        public void parse()
        {
            holder.generatedStatementNodes = Code();
            if (holder.getCurrentTokenType() != tokenType.system_EndOfFile)
            {
                holder.throwException(000, holder.getCurrentToken());
            }

        }


        public List<statementNode> Code()
        {
            return StatementList();
        }

        private List<statementNode> StatementList()
        {
            var statementNode = Statement();
            if (statementNode.GetType() != typeof(unknownStatement))
            {
                var statementList = StatementList();
                statementList.Insert(0, statementNode);
                return statementList;
            }

            return new List<statementNode>();
        }
        private statementNode Statement()
        {
            var canItBeConstant = true;
            switch (holder.getCurrentTokenType())
            {
                case tokenType.HTML_TOKEN: return HtmlStatement();  
                //IF
                case tokenType.resword_IF: return IfStatement();
                //FOR
                case tokenType.resword_FOR: return ForStatement();
                //WHILE
                case tokenType.resword_WHILE: return WhileStatement();

                case tokenType.resword_DO:
                    return DoWhileStatement();

                case tokenType.resword_PRINT:
                    return PrintStatement();
                //FunctionCall
                case tokenType.ID:
                    var functionCall = functionCallStatement();
                    if (functionCall != null)
                    {
                        return functionCall;
                    }
                    goto case tokenType.resword_INT;
                // Declaration
                case tokenType.resword_CONST:
                case tokenType.resword_INT:
                case tokenType.resword_BOOL:
                case tokenType.resword_CHAR:
                case tokenType.resword_STRING:
                case tokenType.resword_FLOAT:
                    return DeclarationStatement(canItBeConstant);
                //Assignation
                case tokenType.assign:
                    return AssignationStatement();
                case tokenType.resword_FUNCTION: 
                    return functionDeclarationStatement();

            }

            return new unknownStatement();
        }

        private statementNode PrintStatement()
        {
            holder.advanceIndex();
            
             if (holder.getCurrentTokenType() != tokenType.symbol_openCurlyBraces)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_openCurlyBraces);
            }
            holder.advanceIndex();
            var expression = Expresion();

            if (holder.getCurrentTokenType() != tokenType.symbol_closeCurlyBraces)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_closeCurlyBraces);
            }
            holder.advanceIndex();

            return new printStatement {Value = expression};
        }


        private statementNode DoWhileStatement()
        {
            //do
            holder.advanceIndex();
            //{
            if (holder.getCurrentTokenType() != tokenType.symbol_openCurlyBraces)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_openCurlyBraces);
            }
            holder.advanceIndex();

            //ListOfStatements
            var dowhileStatements = scopeStatementList();
            //}
            if (holder.getCurrentTokenType() != tokenType.symbol_closeCurlyBraces)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_closeCurlyBraces);
            }
            holder.advanceIndex();

            if (holder.getCurrentTokenType() != tokenType.resword_WHILE)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.resword_WHILE);
            }
            holder.advanceIndex();

            //(
            if (holder.getCurrentTokenType() != tokenType.symbol_openParenthesis)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_openParenthesis);
            }
            holder.advanceIndex();

            var conditionalExpression = Expresion();

            //)
            if (holder.getCurrentTokenType() != tokenType.symbol_closeParenthesis)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_closeParenthesis);
            }
            holder.advanceIndex();
            //;
            EndOfStatement();
            return new doWhileStatement {dowhileStatementList =  dowhileStatements, conditionalExpression = conditionalExpression};
        }

        private statementNode functionDeclarationStatement()
        {
            //function
            holder.advanceIndex();

            //TYPE
            nodeType whatType = null;
            switch (holder.getCurrentTokenType())
            {
                case tokenType.resword_VOID: whatType = new voidType(); break;
                case tokenType.resword_INT: whatType = new numberType(); break;
                case tokenType.resword_BOOL: whatType = new boolType(); break;
                case tokenType.resword_CHAR: whatType = new characterType(); break;
                case tokenType.resword_STRING: whatType = new stringType(); break;
                case tokenType.resword_FLOAT: whatType = new floatType(); break;
                case tokenType.ID: whatType = new structType(); break;
                default: holder.throwException(008,holder.getCurrentToken());
                    break;
            }
            holder.advanceIndex();

            var id = idValue();
            //(
            if (holder.getCurrentTokenType() != tokenType.symbol_openParenthesis)
            {
                holder.throwException(003,holder.getCurrentToken(), tokenType.symbol_openParenthesis);
            }
            holder.advanceIndex();

            //parameters
            var parameters = new List<statementNode>();
            if (holder.getCurrentTokenType() != tokenType.symbol_closeParenthesis)
            {
                 parameters = parameterStatementList();
            }

            //)
            if (holder.getCurrentTokenType() != tokenType.symbol_closeParenthesis)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_closeParenthesis);
            }
            holder.advanceIndex();
            //{
            if (holder.getCurrentTokenType() != tokenType.symbol_openCurlyBraces)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_openCurlyBraces);
            }
            holder.advanceIndex();
            var scope = scopeStatementList();


            if (holder.getCurrentTokenType() != tokenType.symbol_closeCurlyBraces)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_closeCurlyBraces);
            }
            holder.advanceIndex();


            return new functionDeclarativeStatement
            {
                returnType = whatType,
                functionName = id,
                parametersDefined = parameters,
                scope = scope
            };
        }

        private List<statementNode> parameterStatementList()
        {
            var statementNode = parameterStatement();
            if (statementNode.GetType() != typeof(unknownStatement))
            {
                var statementList = new List<statementNode>();
                if(holder.getCurrentTokenType() == tokenType.symbol_SEPARATOR) { 
                    statementList = parameterStatementList();
                }
                statementList.Insert(0, statementNode);
                return statementList;
                   
            }

            return new List<statementNode>();
        }

        private statementNode parameterStatement()
        {
            switch (holder.getCurrentTokenType())
            {
                case tokenType.ID:
                case tokenType.resword_INT:
                case tokenType.resword_BOOL:
                case tokenType.resword_CHAR:
                case tokenType.resword_STRING:
                case tokenType.resword_FLOAT:
                    return DeclarationStatement(false,false);

            }

            return new unknownStatement();
        }

        private List<statementNode> scopeStatementList()
        {
            var statementNode = scopeStatement();
            if (statementNode.GetType() != typeof(unknownStatement))
            {
                var statementList = scopeStatementList();
                statementList.Insert(0, statementNode);
                return statementList;
            }

            return new List<statementNode>();
        }
        private statementNode scopeStatement()
        {
            var canItBeConstant = false;
            switch (holder.getCurrentTokenType())
            {
                case tokenType.HTML_TOKEN: return HtmlStatement();
                //IF
                case tokenType.resword_IF: return IfStatement();
                //FOR
                case tokenType.resword_FOR: return ForStatement();
                //WHILE
                case tokenType.resword_WHILE: return WhileStatement();

                case tokenType.resword_DO:
                    return DoWhileStatement();

                case tokenType.resword_PRINT:
                    return PrintStatement();
                //FunctionCall
                case tokenType.ID:
                    var functionCall = functionCallStatement();
                    if (functionCall != null)
                    {
                        return functionCall;
                    }
                    goto case tokenType.resword_INT;
                // Declaration
                case tokenType.resword_INT:
                case tokenType.resword_BOOL:
                case tokenType.resword_CHAR:
                case tokenType.resword_STRING:
                case tokenType.resword_FLOAT:
                    return DeclarationStatement(canItBeConstant);
                //Assignation
                case tokenType.assign:
                    return AssignationStatement();

                //Return
                case tokenType.resword_RETURN:
                    return ReturnStatement();
            }

            return new unknownStatement();
        }

        private statementNode ReturnStatement()
        {
            holder.advanceIndex();
            expressionNode expression = null;
            if (holder.getCurrentTokenType() != tokenType.symbol_EndOfStatement)
            {
                expression = Expresion();
            }
            EndOfStatement();

            return new returnStatement {Value = expression};
        }


        private statementNode functionCallStatement()
        {
            var ID = idValue();
            var functionCall = idTermPP(ID);
            if (functionCall.GetType() != typeof(functionCallNode))
            {
                holder.retractIndex();
                return null;
            }

            EndOfStatement();
            return new functionCallStatement {functionCalled = functionCall};
        }
        private statementNode AssignationStatement()
        {
            holder.advanceIndex();
            var idNode = idValue();
            if (holder.getCurrentTokenType() != tokenType.symbol_Assignator)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_Assignator);
            }
            holder.advanceIndex();
            var value = Expresion();


            EndOfStatement();
            return new assignationStatement {Id = idNode, Value = value};
        }
        private statementNode DeclarationStatement(bool canItBeConstant = true, bool withEOS = true)
        {
            //const variableType id = value;
            // variableType id = value;
            // variableType id;
            var isItConstant = false;
            if (canItBeConstant) { 
             isItConstant = holder.getCurrentTokenType() == tokenType.resword_CONST;
                if (isItConstant) { 
                    holder.advanceIndex();
                }
            }
            nodeType whatType = null;
            switch (holder.getCurrentTokenType())
            {
                case tokenType.resword_INT: whatType = new numberType();  break;
                case tokenType.resword_BOOL: whatType = new boolType(); break;
                case tokenType.resword_CHAR: whatType = new characterType(); break;
                case tokenType.resword_STRING: whatType = new stringType(); break;
                case tokenType.resword_FLOAT: whatType = new floatType(); break;
                case tokenType.ID: whatType = new structType(); break;
                default:
                    holder.throwException(008, holder.getCurrentToken());
                    break;
            }
            holder.advanceIndex();
            var idNode = idValue();
            expressionNode value = null;
            if ( isItConstant )
            {
                if (holder.getCurrentTokenType() != tokenType.symbol_Assignator)
                {
                    exceptionMaster.throwException(003,holder.getCurrentToken(), tokenType.symbol_Assignator);
                }
                holder.advanceIndex();
                value = Expresion();
            }
            else
            {
                if (holder.getCurrentTokenType() == tokenType.symbol_Assignator)
                {
                    holder.advanceIndex();
                    value = Expresion();
                }
            }
            if (withEOS)
            {
                EndOfStatement();
            }

            return new declarativeStatement {Constant = isItConstant, ID = idNode, Type = whatType, Value = value};
        }


        private statementNode WhileStatement()
        {
            holder.advanceIndex();
            if (holder.getCurrentTokenType() != tokenType.symbol_openParenthesis)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_openParenthesis);
            }
            holder.advanceIndex();

            var expression = Expresion();

            if (holder.getCurrentTokenType() != tokenType.symbol_closeParenthesis)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_closeParenthesis);
            }
            holder.advanceIndex();

            if (holder.getCurrentTokenType() != tokenType.symbol_openCurlyBraces)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_openCurlyBraces);
            }
            holder.advanceIndex();

            var whileStatementList = scopeStatementList();

            if (holder.getCurrentTokenType() != tokenType.symbol_closeCurlyBraces)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_closeCurlyBraces);
            }
            holder.advanceIndex();



            return new whileStatement {conditionalExpression = expression, whileStatementList = whileStatementList};
        } 

        //for(Declaration; Expression; Asignation){scope}
        private statementNode ForStatement()
        {
            //for
            holder.advanceIndex();
            //(
            if (holder.getCurrentTokenType() != tokenType.symbol_openParenthesis)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_openParenthesis);
            }
            holder.advanceIndex();

            //Declaration;
            var Declaration = DeclarationStatement(false);

            //Expression;
            var Expression = Expresion();

            EndOfStatement();
            //Assignation;
            var Assignation = AssignationStatement();
            //)
            if (holder.getCurrentTokenType() != tokenType.symbol_closeParenthesis)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_closeParenthesis);
            }
            holder.advanceIndex();
            //{
            if (holder.getCurrentTokenType() != tokenType.symbol_openCurlyBraces)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_openCurlyBraces);
            }
            holder.advanceIndex();

            //ListOfStatements
            var forStatements = scopeStatementList();
            //}
            if (holder.getCurrentTokenType() != tokenType.symbol_closeCurlyBraces)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_closeCurlyBraces);
            }
            holder.advanceIndex();
            return new forStatement
            {
                declarationStatement = Declaration,
                comparativeExpression = Expression,
                assignationStatement = Assignation,
                forStatementsList =  forStatements
            };
        }

        //if(Expression){scopeList}
        //if(Expression){scopeList} else {scopeList} 
        //if(Expression){scopeList} else scopeStatement 
        private statementNode IfStatement()
        {
            //if
            holder.advanceIndex();
            //(
            if (holder.getCurrentTokenType() != tokenType.symbol_openParenthesis)
            {
                 holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_openParenthesis);
            }
            holder.advanceIndex();
            //Expression
            var condition = Expresion();
            //)
            if (holder.getCurrentTokenType() != tokenType.symbol_closeParenthesis)
            {
                holder.throwException(003,holder.getCurrentToken(), tokenType.symbol_closeParenthesis);
            }
            holder.advanceIndex();
            //{
            if (holder.getCurrentTokenType() != tokenType.symbol_openCurlyBraces)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_openCurlyBraces);
            }
            holder.advanceIndex();
            //scopeList
            var trueStatementList = scopeStatementList();
            //}
            if (holder.getCurrentTokenType() != tokenType.symbol_closeCurlyBraces)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_closeCurlyBraces);
            }
            holder.advanceIndex();
            //else 
            var falseStatementList = new List<statementNode>();
            if (holder.getCurrentTokenType() == tokenType.resword_ELSE)
            {
                holder.advanceIndex();
                //{
                if (holder.getCurrentTokenType() == tokenType.symbol_openCurlyBraces)
                {
                    holder.advanceIndex();
                    //scopeList
                    falseStatementList = scopeStatementList();
                    //}
                    if (holder.getCurrentTokenType() != tokenType.symbol_closeCurlyBraces)
                    {
                        holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_closeCurlyBraces);
                    }
                    holder.advanceIndex();
                }
                else
                {
                    //scopeStatement
                    falseStatementList.Add(scopeStatement());
                }
            }

            return new ifStatement
            {
                ifCondition = condition,
                TrueStatementNodes = trueStatementList,
                FalseStatementNodes = falseStatementList
            };
        }



        //Don't mind this one either.
        private void EndOfStatement()
        {
            if (holder.getCurrentTokenType() != tokenType.symbol_EndOfStatement)
            {
                holder.throwException(006, holder.getCurrentToken());
            }
            holder.advanceIndex();
        }


        // HTML Stuff. Not important.
        private statementNode HtmlStatement()
        {
            var htmlNode = HtmlNode();

            return new hmtlStatement { value = htmlNode };
        }
        private htmlNode HtmlNode()
        {
            var htmlValue = holder.getCurrentToken();
            holder.advanceIndex();
            var htmlNode = new htmlNode { Value = htmlValue._content.getValue() };
            return htmlNode;
        }

        //Below exists only Expressions. Do not lurk.
        private expressionNode Expresion()
        {    
            return orTerm();
        }
        private expressionNode orTerm()
        {
            var orTerm = andTerm();
            return orTermP(orTerm);
        }
        private expressionNode orTermP(expressionNode param)
        {
            if (holder.getCurrentTokenType() == tokenType.logicaloper_OR)
            {
                holder.advanceIndex();
                var valueTerm = andTerm();
                return orTermP(new orNode { LeftOperand = param, RightOperand = valueTerm });
            }
            else
            {
                return param;
            }
        }

        private expressionNode andTerm()
        {
            var andTerm = borTerm();
            return andTermP(andTerm);

        }
        private expressionNode andTermP(expressionNode param)
        {
            if (holder.getCurrentTokenType() == tokenType.logicaloper_AND)
            {
                holder.advanceIndex();
                var valueTerm = borTerm();
                return andTermP(new andNode {LeftOperand = param, RightOperand = valueTerm});
            }
            return param;

        }

        private expressionNode borTerm()
        {
            var borTerm = bxorTerm();
            return borTermP(borTerm);
        }
        private expressionNode borTermP(expressionNode param)
        {
            if (holder.getCurrentTokenType() == tokenType.bitoper_OR)
            {
                holder.advanceIndex();
                var valueTerm = bxorTerm();
                return borTermP(new borNode {LeftOperand = param, RightOperand = valueTerm});
            }
            return param;
        }

        private expressionNode bxorTerm()
        {
            var bxorTerm = bandTerm();
            return bxorTermP(bxorTerm);
        }
        private expressionNode bxorTermP(expressionNode param)
        {
            if (holder.getCurrentTokenType() == tokenType.bitoper_XOR)
            {
                holder.advanceIndex();
                var valueTerm = bandTerm();
                return bxorTermP(new bxorNode {LeftOperand = param, RightOperand = valueTerm});
            }
            return param;
        }

        private expressionNode bandTerm()
        {
            var bandTerm = equalityTerm();
            return bandTermP(bandTerm);
        }
        private expressionNode bandTermP(expressionNode param)
        {
            if (holder.getCurrentTokenType() == tokenType.bitoper_AND)
            {
                holder.advanceIndex();
                var valueTerm = equalityTerm();
                return bandTermP(new bandNode { LeftOperand = param, RightOperand = valueTerm });
            }
            return param;
        }

        private expressionNode equalityTerm()
        {
            var equalityTerm = relationalTerm();
            return equalityTermP(equalityTerm);
        }
        private expressionNode equalityTermP(expressionNode param)
        {
            if (holder.getCurrentTokenType() == tokenType.reloper_EQUAL)
            {
                holder.advanceIndex();
                var valueTerm = relationalTerm();
                return bandTermP(new equalNode { LeftOperand = param, RightOperand = valueTerm });
            }
            if (holder.getCurrentTokenType() == tokenType.reloper_NOTEQUAL)
            {
                holder.advanceIndex();
                var valueTerm = relationalTerm();
                return bandTermP(new notequalNode { LeftOperand = param, RightOperand = valueTerm });
            }
            return param;
        }

        private expressionNode relationalTerm()
        {
            var relationalTerm = shiftTerm();
            return relationalTermP(relationalTerm);
        }
        private expressionNode relationalTermP(expressionNode param)
        {
            if (holder.getCurrentTokenType() == tokenType.reloper_LESSTHAN)
            {
                holder.advanceIndex();
                var valueTerm = shiftTerm();
                return relationalTermP(new lessthanNode {LeftOperand = param, RightOperand = valueTerm});
            }
            if (holder.getCurrentTokenType() == tokenType.reloper_LESSOREQUAL)
            {
                holder.advanceIndex();
                var valueTerm = shiftTerm();
                return relationalTermP(new lessorequalNode { LeftOperand = param, RightOperand = valueTerm });

            }
            if (holder.getCurrentTokenType() == tokenType.reloper_GREATERTHAN)
            {
                holder.advanceIndex();
                var valueTerm = shiftTerm();
                return relationalTermP(new greaterthanNode { LeftOperand = param, RightOperand = valueTerm });
                
            }
            if (holder.getCurrentTokenType() == tokenType.reloper_GREATEROREQUAL)
            {
                holder.advanceIndex();
                var valueTerm = shiftTerm();
                return relationalTermP(new greaterorequalNode { LeftOperand = param, RightOperand = valueTerm });
            }
            return param;
        }

        private expressionNode shiftTerm()
        {
            var shiftTerm = arithmeticTerm();
            return shiftTermP(shiftTerm);
        }
        private expressionNode shiftTermP(expressionNode param)
        {
            if (holder.getCurrentTokenType() == tokenType.bitoper_RIGHTSHIFT)
            {
                holder.advanceIndex();
                var valueTerm = arithmeticTerm();
                return new rightshiftNode { LeftOperand = param, RightOperand = valueTerm };
            }
            if (holder.getCurrentTokenType() == tokenType.bitoper_LEFTSHIFT)
            {
                holder.advanceIndex();
                var valueTerm = arithmeticTerm();
                return new leftshiftNode { LeftOperand = param, RightOperand = valueTerm };
            }
            return param;
        }

        private expressionNode arithmeticTerm()
        {
            var arithmeticTerm = magnitudeTerm();
            return arithmeticTermP(arithmeticTerm);
        }
        private expressionNode arithmeticTermP(expressionNode param)
        {
            if (holder.getCurrentTokenType() == tokenType.oper_MULTIPLICATION)
            {
                holder.advanceIndex();
                var valueTerm = magnitudeTerm();
                return arithmeticTermP(new multiplicationNode {LeftOperand = param, RightOperand = valueTerm});
            }
            if (holder.getCurrentTokenType() == tokenType.oper_DIVISION)
            {
                holder.advanceIndex();
                var valueTerm = magnitudeTerm();
                return arithmeticTermP(new divisionNode {LeftOperand = param, RightOperand = valueTerm});
            }
            if (holder.getCurrentTokenType() == tokenType.oper_MODULUS)
            {
                holder.advanceIndex();
                var valueTerm = magnitudeTerm();
                return arithmeticTermP(new modulusNode {LeftOperand = param, RightOperand = valueTerm});
            }
            return param;
        }

        private expressionNode magnitudeTerm()
        {
            var magnitudeTerm = factorTerm();
            return magnitudeTermP(magnitudeTerm);
        }
        private expressionNode magnitudeTermP(expressionNode param)
        {
            if (holder.getCurrentTokenType() == tokenType.oper_ADDITION)
            {
                holder.advanceIndex();
                var valueTerm = factorTerm();
                return magnitudeTermP(new additionNode {LeftOperand = param, RightOperand = valueTerm});
            }
            if (holder.getCurrentTokenType() == tokenType.oper_SUBSTRACTION)
            {
                holder.advanceIndex();
                var valueTerm = factorTerm();
                return magnitudeTermP(new substractionNode {LeftOperand = param, RightOperand = valueTerm});
            }
            return param;
        }

        private expressionNode factorTerm()
        {
            //(Expression)
            if (holder.getCurrentTokenType() == tokenType.resword_DEF)
            {
                holder.advanceIndex();
                return new defNode();
            }
            if (holder.getCurrentTokenType() == tokenType.symbol_openParenthesis)
            {
                holder.advanceIndex();
                var valueTerm = Expresion();
                if (holder.getCurrentTokenType() != tokenType.symbol_closeParenthesis)
                {
                    holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_closeParenthesis);
                }
                holder.advanceIndex();
                return valueTerm;
            }
            //Int
            if (holder.getCurrentTokenType() == tokenType.literal_NUMBER)
            {
                var numberValue = int.Parse(holder.getCurrentToken()._content.getValue());   
                holder.advanceIndex();
                return new numberNode {Value =  numberValue};
            }
            //Bool
            if (holder.getCurrentTokenType() == tokenType.literal_BOOL)
            {
                var boolValue = bool.Parse(holder.getCurrentToken()._content.getValue());
                holder.advanceIndex();
                return new boolNode {Value = boolValue};
            }
            //String
            if (holder.getCurrentTokenType() == tokenType.literal_STRING)
            {
                var stringValue = holder.getCurrentToken()._content.getValue();
                holder.advanceIndex();
                return new stringNode {Value = stringValue};
            }
            //Character
            if (holder.getCurrentTokenType() == tokenType.literal_CHARACTER)
            {
                var characterValue = char.Parse(holder.getCurrentToken()._content.getValue());
                holder.advanceIndex();
                return new characterNode { Value = characterValue };
            }
            //Float
            if (holder.getCurrentTokenType() == tokenType.literal_FLOAT)
            {
                var floatValue = float.Parse(holder.getCurrentToken()._content.getValue());
                holder.advanceIndex();
                return new floatNode { Value = floatValue};
            }

            //This might need some refactoring along the way to adapt to Accessors and Pointers.
            //ID....
            if (holder.getCurrentTokenType() == tokenType.ID)
            {

                var idTerm = idValue();
                return idTermP(idTerm);
            }
            
            //Epsilon, which essentially it should fail.
            holder.throwException(004,holder.getCurrentToken());
            return null;
        }

        private expressionNode idValue()
        {
            if (holder.getCurrentTokenType() != tokenType.ID) { holder.throwException(003,holder.getCurrentToken(), tokenType.ID);}
            
            var idValue = holder.getCurrentToken()._content.getValue();
            holder.advanceIndex();
            return new idNode {Name = idValue};
        }
        private expressionNode idTermP(expressionNode param)
        {
            //ID.ID....
            /*
            if (holder.getCurrentTokenType() == tokenType.symbol_Accessor)
            {
                holder.advanceIndex();
                var valueTerm = idValue();
                return idTermP(new structAccessor {LeftOperand = param, RightOperand = valueTerm});
            }*/
            ;
            //...ID[expression]
            /*
            if (holder.getCurrentTokenType() == tokenType.symbol_arrayOpen)
            {
                holder.advanceIndex();
                var valueTerm = Expresion();
                if (holder.getCurrentTokenType() != tokenType.symbol_arrayClose)
                {
                    holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_arrayClose);
                }
                holder.advanceIndex();
                return new arrayNode {LeftOperand = param, RightOperand = valueTerm};
            }
            */
            ;
            //...ID()
            //...ID(Expression)
            //...ID(Expression,....,Expression)
            if (holder.getCurrentTokenType() == tokenType.symbol_openParenthesis)
            {
                holder.advanceIndex();
                List<expressionNode> parameterCalled = null;
                if (holder.getCurrentTokenType() != tokenType.symbol_closeParenthesis)
                {
                    parameterCalled = parameterCallList();
                }                     
                if (holder.getCurrentTokenType() != tokenType.symbol_closeParenthesis)
                {
                    holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_arrayClose);
                }
                holder.advanceIndex();
                return new functionCallNode{ID = param, parameters = parameterCalled};
            }
    
            return param;
        }

        private expressionNode idTermPP(expressionNode param)
        {
            if (holder.getCurrentTokenType() != tokenType.symbol_openParenthesis) return param;

            holder.advanceIndex();
            List<expressionNode> parameterCalled = null;
            if (holder.getCurrentTokenType() != tokenType.symbol_closeParenthesis)
            {
                parameterCalled = parameterCallList();
            }


            if (holder.getCurrentTokenType() != tokenType.symbol_closeParenthesis)
            {
                holder.throwException(003, holder.getCurrentToken(), tokenType.symbol_arrayClose);
            }
            holder.advanceIndex();
            return new functionCallNode { ID = param, parameters = parameterCalled };
        }

        private List<expressionNode> parameterCallList()
        {
            List<expressionNode> list = new List<expressionNode>();
            if (holder.getCurrentTokenType() == tokenType.symbol_closeParenthesis)
            {
                return list;
            }
            else
            {
                list.Add(Expresion());
            }
            while (holder.getCurrentTokenType() == tokenType.symbol_SEPARATOR)
            {
                holder.advanceIndex();
                list.Add(Expresion());
            }
            return list;
        }


    }


}