﻿namespace happyCompiler
{
    public enum tokenType
    {
        //Return this for DEBUGGING purposes.
        NaN,
        //This token should not APPEAR.
        ErrorToken,
        //Literals
        literal_ID,
        literal_NUMBER,
        //System Flags Tokens
        system_EndOfFile,
        literal_FLOAT,
        literal_OCTAL,
        literal_HEXADECIMAL,
        //Aritmetic Operators
        oper_ADDITION,
        oper_INCREASE,
        oper_SUBSTRACTION,
        oper_DECREASE,
        oper_DIVISION,
        oper_MODULUS,
        oper_MULTIPLICATION,
        logicaloper_NOT,
        logicaloper_OR,
        logicaloper_AND,
        reloper_COMPARE,
        reloper_LESSTHAN,
        reloper_LESSOREQUAL,
        reloper_GREATERTHAN,
        reloper_GREATEROREQUAL,
        reloper_NOTEQUAL,
        bitoper_AND,
        bitoper_OR,
        bitoper_NOT,
        bitoper_XOR,
        bitoper_LEFTSHIFT,
        bitoper_RIGHTSHIFT,
        //Class Words
        resword_INT,
        resword_FLOAT,
        resword_BOOL,
        resword_CHAR,
        resword_DATE,
        resword_ENUM,
        resword_STRUCT,
        resword_VAR,
        //Reserved Words
        resword_IF,
        resword_ELSE,
        resword_FOR,
        resword_WHILE,
        resword_DO,
        resword_SWITCH,
        resword_CASE,
        resword_FOREACH,
        resword_BREAK,
        resword_CONTINUE,
        resword_FUNCTION,
        resword_INCLUDE,
        resword_VOID,
        //Symbols
        symbol_Accessor,
        symbol_EndOfStatement,
        symbol_Assignator,
        symbol_openParenthesis,
        symbol_closeParenthesis,
        symbol_openCurlyBraces,
        symbol_closeCurlyBraces,
        symbol_SEPARATOR,
        symbol_arrayOpen,
        symbol_arrayClose,
        symbol_fileOpen,
        symbol_fileClose,
        symbol_singleQuote,
        symbol_doubleQuotes,
        symbol_commentOpen,
        symbol_commentClose,
        symbol_COMMENTLINE,
        symbol_HASHTAG,
        literal_CHARACTER,
        literal_STRING
    }
}