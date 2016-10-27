using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace happyCompiler
{
    public class lexiDictionary
    {
        private Dictionary<string, tokenType> _arithmeticOperators;
        private Dictionary<string, tokenType> _relationalOperators;
        private Dictionary<string, tokenType> _bitOperators;
        private Dictionary<string, tokenType> _logicalOperators;
        private Dictionary<string, tokenType> _keyWords;
        private Dictionary<string, tokenType> _reservedWords;
        private Dictionary<string, tokenType> _flowSymbols;
        private Dictionary<string, tokenType> _symbols;


        public lexiDictionary()
        {
            defineArithmethics();
            defineRelational();
            defineLogical();
            defineBit();
            defineKeywords();
            defineReservedWords();
            defineFlow();
            defineSymbols();
        }


        private void defineArithmethics()
        {
            _arithmeticOperators = new Dictionary<string, tokenType>();
            //7
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
            //6
            _relationalOperators.Add("==", tokenType.reloper_COMPARE);
            _relationalOperators.Add("<", tokenType.reloper_LESSTHAN);
            _relationalOperators.Add("<=", tokenType.reloper_LESSOREQUAL);
            _relationalOperators.Add(">", tokenType.reloper_GREATERTHAN);
            _relationalOperators.Add(">=", tokenType.reloper_GREATEROREQUAL);
            _relationalOperators.Add("!=", tokenType.reloper_NOTEQUAL);
        }

        private void defineLogical()
        {
            _logicalOperators = new Dictionary<string, tokenType>();
            //2
            _logicalOperators.Add("&&", tokenType.logicaloper_AND);
            _logicalOperators.Add("||", tokenType.logicaloper_AND);
        }

        private void defineBit()
        {
            _bitOperators = new Dictionary<string, tokenType>();
            //6
            _bitOperators.Add("&", tokenType.bitoper_AND);
            _bitOperators.Add("|", tokenType.bitoper_OR);
            _bitOperators.Add("!", tokenType.bitoper_NOT);
            _bitOperators.Add("^", tokenType.bitoper_XOR);
            _bitOperators.Add("<<", tokenType.bitoper_LEFTSHIFT);
            _bitOperators.Add(">>", tokenType.bitoper_RIGHTSHIFT);
        }

        private void defineKeywords()
        {
            _keyWords = new Dictionary<string, tokenType>();
            //9
            _keyWords.Add("int", tokenType.keyword_INT);
            _keyWords.Add("float", tokenType.keyword_FLOAT);
            _keyWords.Add("bool", tokenType.keyword_BOOL);
            _keyWords.Add("char", tokenType.keyword_CHAR);
            _keyWords.Add("string", tokenType.keyword_DATE);
            _keyWords.Add("date", tokenType.keyword_DATE); 
            _keyWords.Add("enum", tokenType.keyword_ENUM);
            _keyWords.Add("struct", tokenType.keyword_STRUCT);
            _keyWords.Add("var", tokenType.keyword_VAR);

        }

        private void defineReservedWords() {
            _reservedWords = new Dictionary<string, tokenType>();
            //11
            _reservedWords.Add("if", tokenType.resword_IF);
            _reservedWords.Add("else", tokenType.resword_ELSE);
            _reservedWords.Add("for", tokenType.resword_FOR);
            _reservedWords.Add("while", tokenType.resword_WHILE);
            _reservedWords.Add("do", tokenType.resword_DO);
            _reservedWords.Add("switch", tokenType.resword_SWITCH);
            _reservedWords.Add("case", tokenType.resword_CASE);
            _reservedWords.Add("foreach", tokenType.resword_FOREACH);
            _reservedWords.Add("break", tokenType.resword_IF);
            _reservedWords.Add("continue", tokenType.resword_CONTINUE);
            _reservedWords.Add("function", tokenType.resword_FUNCTION);
        }

        private void defineFlow()
        {
            _flowSymbols = new Dictionary<string, tokenType>();
            //6
            _flowSymbols.Add("(", tokenType.flow_paramOpen);
            _flowSymbols.Add(")", tokenType.flow_paramClose);
            _flowSymbols.Add("{", tokenType.flow_blockOpen);
            _flowSymbols.Add("}", tokenType.flow_blockClose);
            _flowSymbols.Add("[", tokenType.flow_arrayOpen);
            _flowSymbols.Add("'", tokenType.flow_singleQuote);
            _flowSymbols.Add('"'.ToString(), tokenType.flow_doubleQuotes);
        }
        
        private void defineSymbols()
        {
            _symbols = new Dictionary<string, tokenType>();
            //7
            _symbols.Add("//", tokenType.symbol_COMMENTLINE);
            _symbols.Add(",", tokenType.symbol_SEPARATOR);
            _symbols.Add("=", tokenType.symbol_ASSIGN);
            _symbols.Add(".", tokenType.symbol_DOT);
            _symbols.Add('\r'.ToString(), tokenType.symbol_CARRIAGERETURN);
            _symbols.Add('\n'.ToString(), tokenType.symbol_EOL);
            _symbols.Add(";", tokenType.symbol_EOS);
            _symbols.Add('\0'.ToString(), tokenType.symbol_EOF);
        }

        public tokenType identifyString(string input)
        {
            tokenType result = tokenType.ID;
            if (_reservedWords.ContainsKey(input)) { result = _reservedWords[input]; }
            if (_keyWords.ContainsKey(input)) { result = _keyWords[input]; }
            return result;
        }

        public tokenType identifySymbol(string input)
        {
            tokenType result = tokenType.NaN;
            if(_relationalOperators.ContainsKey(input)) { result = _relationalOperators[input]; }
            if (_bitOperators.ContainsKey(input)) { result = _bitOperators[input]; }
            if (_logicalOperators.ContainsKey(input)) { result = _logicalOperators[input]; }
            if (_arithmeticOperators.ContainsKey(input)) { result = _arithmeticOperators[input]; }
            if (_symbols.ContainsKey(input)) { result = _symbols[input]; }
          
            return result;
        }
        public tokenType identifyFlowSymbol(string input)
        {
            tokenType result = tokenType.NaN;
            if (_flowSymbols.ContainsKey(input)) { result = _flowSymbols[input]; }
            return result;
        }

    }
}
