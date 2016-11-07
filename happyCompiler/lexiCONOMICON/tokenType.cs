namespace happyCompiler
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
        oper_ADDITION,
        oper_INCREASE,
        oper_SUBSTRACTION,
        oper_DECREASE,
        oper_DIVISION,
        oper_MODULUS,
        oper_MULTIPLICATION,
        symbol_Accessor,
        symbol_EndOfStatement,
        symbol_Assignator,
        symbol_openParenthesis,
        symbol_closeParenthesis,
        symbol_openCurlyBraces,
        symbol_closeCurlyBraces

    }
}