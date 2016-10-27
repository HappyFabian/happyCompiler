using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace happyCompiler
{
    public enum tokenType 
    {
        //ID - 1
        ID,
        //SIMPLE - 1
        DIGIT,
        STRINGLITERAL,
        COMMENTCONTENT,
        //arithmetic - 7
        oper_ADDITION,
        oper_INCREASE,
        oper_SUBSTRACTION,
        oper_DECREASE,
        oper_MULTIPLICATION,
        oper_DIVISION,
        oper_MODULUS,
        //relational - 6
        reloper_COMPARE,
        reloper_LESSTHAN,
        reloper_LESSOREQUAL,
        reloper_GREATERTHAN,
        reloper_GREATEROREQUAL,
        reloper_NOTEQUAL,
        //logical - 2
        logicaloper_AND,
        logicaloper_OR,
        //bitoper - 6
        bitoper_AND,
        bitoper_OR,
        bitoper_NOT,
        bitoper_XOR,
        bitoper_LEFTSHIFT,
        bitoper_RIGHTSHIFT,
        //keyword - 9
        keyword_INT,
        keyword_FLOAT,
        keyword_BOOL,
        keyword_CHAR,
        keyword_STRING,
        keyword_DATE,
        keyword_STRUCT,
        keyword_ENUM,
        keyword_VAR,
        //reserved - 11
        resword_IF,
        resword_ELSE,
        resword_WHILE,
        resword_DO,
        resword_SWITCH,
        resword_CASE,
        resword_FOR,
        resword_FOREACH,
        resword_BREAK,
        resword_CONTINUE,
        resword_FUNCTION,
        //flow - 6
        flow_paramOpen,
        flow_paramClose,
        flow_singleQuote,
        flow_doubleQuotes,
        flow_arrayOpen,
        flow_arrayClose,
        flow_blockOpen,
        flow_blockClose,

        //symbol - 7
        symbol_COMMENTLINE,
        symbol_SEPARATOR,
        symbol_DOT,
        symbol_ASSIGN,
        symbol_CARRIAGERETURN,
        symbol_EOS, //END OF STATEMENT
        symbol_EOF, //END OF FILE
        symbol_EOL, //END OF LINE
        NaN //SYMBOL DOES NOT EXIST. ERROR
    }
}
