using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexiCONOMICON
{
    public class lexiDictionary
    {
        private Dictionary<string, tokenType> _arithmeticOperators;
        private Dictionary<string, tokenType> _relationalOperators;
        private Dictionary<string, tokenType> _bitOperators;
        private Dictionary<string, tokenType> _logicalOperators;
        private Dictionary<string, tokenType> _reservedWords;
        private Dictionary<string, tokenType> _symbols;
        private Dictionary<string, tokenType> _escape;

        public lexiDictionary()
        {
            defineArithmethics();
            defineRelational();
            defineLogical();
            defineBit();
            defineReservedWords();
            defineSymbols();
            defineEscapes();
        }


        private void defineArithmethics()
        {
            _arithmeticOperators = new Dictionary<string, tokenType>();
            _arithmeticOperators.Add("+", tokenType.oper_ADDITION);
            _arithmeticOperators.Add("++", tokenType.oper_INCREASE);
            _arithmeticOperators.Add("-", tokenType.oper_SUBSTRACTION);
            _arithmeticOperators.Add("--", tokenType.oper_DECREASE);
            _arithmeticOperators.Add("/", tokenType.oper_DIVISION);
            _arithmeticOperators.Add("*", tokenType.oper_MULTIPLICATION);
            _arithmeticOperators.Add("%", tokenType.oper_MODULUS);
        }

        private void defineRelational()
        {
            _relationalOperators = new Dictionary<string, tokenType>();
            _relationalOperators.Add("==", tokenType.reloper_EQUAL);
            _relationalOperators.Add("<", tokenType.reloper_LESSTHAN);
            _relationalOperators.Add("<=", tokenType.reloper_LESSOREQUAL);
            _relationalOperators.Add(">", tokenType.reloper_GREATERTHAN);
            _relationalOperators.Add(">=", tokenType.reloper_GREATEROREQUAL);
            _relationalOperators.Add("!=", tokenType.reloper_NOTEQUAL);

        }

        private void defineLogical()
        {
            _logicalOperators = new Dictionary<string, tokenType>();
           
            _logicalOperators.Add("&&", tokenType.logicaloper_AND);
            _logicalOperators.Add("||", tokenType.logicaloper_OR);
            _logicalOperators.Add("!", tokenType.logicaloper_NOT);
        }

        private void defineBit()
        {
            _bitOperators = new Dictionary<string, tokenType>();
            _bitOperators.Add("&", tokenType.bitoper_AND);
            _bitOperators.Add("|", tokenType.bitoper_OR);
            _bitOperators.Add("~", tokenType.bitoper_NOT);
            _bitOperators.Add("^", tokenType.bitoper_XOR);
            _bitOperators.Add("<<", tokenType.bitoper_LEFTSHIFT);
            _bitOperators.Add(">>", tokenType.bitoper_RIGHTSHIFT);
        }
        private void defineReservedWords() {
            _reservedWords = new Dictionary<string, tokenType>();
            //11
            _reservedWords.Add("int", tokenType.resword_INT);
            _reservedWords.Add("float", tokenType.resword_FLOAT);
            _reservedWords.Add("bool", tokenType.resword_BOOL);
            _reservedWords.Add("char", tokenType.resword_CHAR);
            _reservedWords.Add("string", tokenType.resword_STRING);
            _reservedWords.Add("date", tokenType.resword_DATE);
            _reservedWords.Add("enum", tokenType.resword_ENUM);
            _reservedWords.Add("struct", tokenType.resword_STRUCT);
            _reservedWords.Add("var", tokenType.resword_VAR);
            _reservedWords.Add("void", tokenType.resword_VOID);
            _reservedWords.Add("if", tokenType.resword_IF);
            _reservedWords.Add("else", tokenType.resword_ELSE);
            _reservedWords.Add("for", tokenType.resword_FOR);
            _reservedWords.Add("while", tokenType.resword_WHILE);
            _reservedWords.Add("do", tokenType.resword_DO);
            _reservedWords.Add("switch", tokenType.resword_SWITCH);
            _reservedWords.Add("case", tokenType.resword_CASE);
            _reservedWords.Add("assign", tokenType.assign);

            _reservedWords.Add("def", tokenType.resword_DEF);
            _reservedWords.Add("print", tokenType.resword_PRINT);
            _reservedWords.Add("default", tokenType.resword_DEFAULT);
            _reservedWords.Add("foreach", tokenType.resword_FOREACH);
            _reservedWords.Add("break", tokenType.resword_BREAK);
            _reservedWords.Add("continue", tokenType.resword_CONTINUE);
            _reservedWords.Add("return", tokenType.resword_RETURN);
            _reservedWords.Add("function", tokenType.resword_FUNCTION);
            _reservedWords.Add("include",tokenType.resword_INCLUDE);
            _reservedWords.Add("const", tokenType.resword_CONST);
            _reservedWords.Add("true", tokenType.literal_BOOL);
            _reservedWords.Add("false", tokenType.literal_BOOL);
        }



        private void defineSymbols()
        {
            _symbols = new Dictionary<string, tokenType>();
            //7
            _symbols.Add("->", tokenType.symbol_Accessor);
            _symbols.Add(":", tokenType.symbol_doublePoints);
            _symbols.Add("#", tokenType.symbol_HASHTAG);
            _symbols.Add("//", tokenType.symbol_COMMENTLINE);     
            _symbols.Add("/*", tokenType.symbol_commentOpen);
            _symbols.Add("*/", tokenType.symbol_commentClose);
            _symbols.Add(",", tokenType.symbol_SEPARATOR);
            _symbols.Add("=", tokenType.symbol_Assignator);
            _symbols.Add("+=", tokenType.symbol_operAssignator);
            _symbols.Add("-=", tokenType.symbol_operAssignator);
            _symbols.Add("*=", tokenType.symbol_operAssignator);
            _symbols.Add("/=", tokenType.symbol_operAssignator);
            _symbols.Add("%=", tokenType.symbol_operAssignator);
            _symbols.Add(">>=", tokenType.symbol_operAssignator);
            _symbols.Add("<<=", tokenType.symbol_operAssignator);
            _symbols.Add("&=", tokenType.symbol_operAssignator);
            _symbols.Add("^=", tokenType.symbol_operAssignator);
            _symbols.Add("|=", tokenType.symbol_operAssignator);
            _symbols.Add(".", tokenType.symbol_Accessor);
            _symbols.Add(";", tokenType.symbol_EndOfStatement);
             _symbols.Add("(", tokenType.symbol_openParenthesis);
            _symbols.Add(")", tokenType.symbol_closeParenthesis);
            _symbols.Add("{", tokenType.symbol_openCurlyBraces);
            _symbols.Add("}", tokenType.symbol_closeCurlyBraces);
            _symbols.Add("[", tokenType.symbol_arrayOpen);
            _symbols.Add("]", tokenType.symbol_arrayClose);
           // _symbols.Add("<",tokenType.symbol_fileOpen);
           // _symbols.Add(">", tokenType.symbol_fileClose);
            _symbols.Add('\''.ToString(), tokenType.symbol_singleQuote);
            _symbols.Add('"'.ToString(), tokenType.symbol_doubleQuotes);

        }

        private void defineEscapes()
        {
            _escape = new Dictionary<string, tokenType>();
        }

        public tokenType identifyString(string input)
        {
            tokenType result = tokenType.ID;
            if (_reservedWords.ContainsKey(input)) { result = _reservedWords[input]; }
            return result;
        }
        public tokenType identifySymbolAndPunctuation(string input)
        {
            var result = tokenType.ErrorToken;
            if(_relationalOperators.ContainsKey(input)) { result = _relationalOperators[input]; }
            if (_bitOperators.ContainsKey(input)) { result = _bitOperators[input]; }
            if (_logicalOperators.ContainsKey(input)) { result = _logicalOperators[input]; }
            if (_arithmeticOperators.ContainsKey(input)) { result = _arithmeticOperators[input]; }
            if (_symbols.ContainsKey(input)) { result = _symbols[input]; }
            if (_escape.ContainsKey(input)) { result = _escape[input]; }
            return result;
        }
    }
}
