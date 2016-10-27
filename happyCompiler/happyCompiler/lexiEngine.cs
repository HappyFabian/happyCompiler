using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace happyCompiler
{
    public class lexiEngine
    {
        private string _path;
        private Encoding _encoding;
        private StreamReader _reader;
        private List<tokenObject> generatedTokens;
        private lexiDictionary _dictionary;


        public lexiEngine(string path, Encoding encoding)
        {
            _path = path;
            _encoding = encoding ?? Encoding.Default;
            _reader = new StreamReader(_path, Encoding.UTF8);
            generatedTokens = new List<tokenObject>();
            _dictionary = new lexiDictionary();
            _currentLayer = new List<layerObject>();
        }

        public char advanceCursor()
        {
            var character = (char)_reader.Read();
            currentColumn++;
            return character;
        }



        private int currentColumn = 0;
        private int currentRow = 0;

        private int lastTokenColumn = 0;
        private int lastTokenRow = 0;

        private List<layerObject> _currentLayer;

        private string tokenContent;
        private char _currentChar = ' ';
        public tokenObject getNextToken()
        {

            _currentChar = advanceCursor();
            var currentLayer = _currentLayer.Last();

            switch(currentLayer.LAYERNAME)
            {
                case "DEPTHCOMMENTLINE":

                    while(_currentChar != '\n' && !_reader.EndOfStream)
                    {
                        _currentChar = advanceCursor();
                    }
                    if (_currentChar == '\n')
                    {
                        currentRow++;
                        lastTokenColumn = currentColumn;
                        lastTokenRow = currentRow;
                        _currentLayer.Remove(currentLayer);
                        return new tokenObject(tokenType.COMMENTCONTENT, "IGNORE CONTENT", lastTokenColumn, lastTokenRow);
                    }
                    if(_reader.EndOfStream)
                    {
                        _currentLayer.Remove(currentLayer);
                        lastTokenColumn = currentColumn;
                        lastTokenRow = currentRow;
                        return new tokenObject(tokenType.symbol_EOF, "<EOF>", lastTokenColumn, lastTokenRow);
                    }
                    break;
                case "DEPTHBLOCK":
                case "DEPTHZERO":
                default:



                    if (Char.IsWhiteSpace(_currentChar) && _currentChar != '\n' && _currentChar != '\r')
                    {
                        while (Char.IsWhiteSpace(_currentChar) && _currentChar != '\n')
                        {
                            _currentChar = advanceCursor();
                        }
                        cleanContent();
                    }


                    if (_currentChar == '}')
                    {
                        if (currentLayer.LAYERNAME.Equals("DEPTHBLOCK"))
                        {
                            _currentLayer.Remove(_currentLayer.Last());
                            return new tokenObject(tokenType.flow_blockClose, _currentChar.ToString(), lastTokenColumn, lastTokenRow);
                        }
                    }
                    if (_currentChar == '\r')
                    {
                        while (_currentChar == '\r')
                        {
                            _currentChar = advanceCursor();
                        }
                    }

                    if (_currentChar == '\n')
                    {
                        while (_currentChar == '\n')
                        {
                            _currentChar = advanceCursor();
                        }

                        currentRow++;
                        currentColumn = 0;
                    }
                    if (Char.IsLetter(_currentChar))
                    {
                        lastTokenColumn = currentColumn;
                        lastTokenRow = currentRow;
                        tokenContent += _currentChar;
                        var result = GetWord();
                        cleanContent();
                        return result;
                    }
                    if(Char.IsDigit(_currentChar))
                    {
                        lastTokenColumn = currentColumn;
                        lastTokenRow = currentRow;
                        tokenContent += _currentChar;
                        var result = GetDigit();
                        cleanContent();
                        return result;
                    }
                    if (Char.IsSymbol(_currentChar) || Char.IsPunctuation(_currentChar))
                    {
                        if (_dictionary.identifySymbol(_currentChar.ToString()) != tokenType.NaN)
                        {
                            lastTokenColumn = currentColumn;
                            lastTokenRow = currentRow;
                            tokenContent += _currentChar;
                            var result = evaluateNormalSymbol();
                            cleanContent();
                            return result;
                        }

                        if (_dictionary.identifyFlowSymbol(_currentChar.ToString()) != tokenType.NaN)
                        {
                            lastTokenColumn = currentColumn;
                            lastTokenRow = currentRow;
                            tokenContent += _currentChar;
                            // return evaluateFlowSymbol();
                            var result = evaluateFlowSymbol();
                            cleanContent();
                            return result;
                        }
                    }
                    if (_reader.EndOfStream)
                    {
                        lastTokenColumn = currentColumn;
                        lastTokenRow = currentRow;
                        return new tokenObject(tokenType.symbol_EOF, "<EOF>", lastTokenColumn, lastTokenRow);
                    }
                    break;

                
            }

            throw new Exception("THIS ORC MUST DIE");
        }
        private void cleanContent() { tokenContent = ""; }

        private tokenObject GetWord()
        {

            while(Char.IsLetterOrDigit((char)_reader.Peek()))
            {
                _currentChar = advanceCursor();
                tokenContent += _currentChar;
            }

            return new tokenObject(_dictionary.identifyString(tokenContent), tokenContent, lastTokenColumn, lastTokenRow);
        }

        private tokenObject GetDigit()
        {
            while (Char.IsDigit((char)_reader.Peek()))
            {
                _currentChar = advanceCursor();
                tokenContent += _currentChar;
            }

            return new tokenObject(tokenType.DIGIT, tokenContent, lastTokenColumn, lastTokenRow);
        }


        private tokenObject evaluateNormalSymbol()
        {
            var peekingChar =(char) _reader.Peek();
            string previewSymbol = _currentChar.ToString() + peekingChar.ToString();
            var type = _dictionary.identifySymbol(previewSymbol);
            if ((Char.IsSymbol(peekingChar) || Char.IsPunctuation(peekingChar))&&type != tokenType.NaN)
            {            
                    advanceCursor();
                    if (type == tokenType.symbol_COMMENTLINE)
                    {
                        _currentLayer.Add(new layerObject("DEPTHCOMMENTLINE",lastTokenColumn,lastTokenRow));
                    }
                    return new tokenObject(type, previewSymbol, lastTokenColumn, lastTokenRow);
            }
            else
            {
                type = _dictionary.identifySymbol(_currentChar.ToString());
                if (type != tokenType.NaN)
                {

                    return new tokenObject(type,tokenContent, lastTokenColumn, lastTokenRow);
                }

            }
            throw new Exception("SOMETHING WENT WRONG");
        }

        private tokenObject evaluateFlowSymbol()
        {

            var type = _dictionary.identifyFlowSymbol(_currentChar.ToString());
            if (type != tokenType.NaN)
            {
                if(type == tokenType.flow_doubleQuotes)
                {
                    _currentChar = advanceCursor();
                    var isSTRINGLTIERAL = true;
                    while(isSTRINGLTIERAL == true)
                    {
                        if (_currentChar == '\n')
                        {
                            throw new Exception("BROKEN STRING LITERAL AT COLUMN: " + currentColumn + " - ROW: " + currentRow);
                        }
                        else
                        {
                            tokenContent += _currentChar;
                        }
                         _currentChar = advanceCursor();
                        if(_currentChar == '"')
                        {
                            isSTRINGLTIERAL = false;
                            tokenContent += _currentChar;

                            return new tokenObject(tokenType.STRINGLITERAL, tokenContent, lastTokenColumn, lastTokenRow);
                        }

                        if(_reader.EndOfStream && isSTRINGLTIERAL == true)
                        {
                            throw new Exception("BROKEN STRING LITERAL AT COLUMN: " + lastTokenColumn + " - ROW: " + lastTokenRow);
                        }
                    }

                }

                if(type == tokenType.flow_blockOpen)
                {
                    _currentLayer.Add(new layerObject("DEPTHBLOCK", lastTokenColumn, lastTokenRow));
                }


                return new tokenObject(type, tokenContent, lastTokenColumn, lastTokenRow);
            }
            throw new Exception("SOMETHING IS NOT RIGHT");
        }

        public void generateTokens()
        {
            _currentLayer.Add(new layerObject("DEPTHZERO", 0, 0));

            var parsing = true;
            while(parsing)
            {
                var currentToken = getNextToken();
                generatedTokens.Add(currentToken);
                if (currentToken._type == tokenType.symbol_EOF)
                {
                    parsing = false;
                }
            }
            
            
            if (_currentLayer.Last().LAYERNAME != "DEPTHZERO")
            {
                throw new Exception("You are missing a closing token. ERROR AT" + _currentLayer.Last().coordinates.getColumn() + " - " + _currentLayer.Last().coordinates.getRow());
            }
            
            


            _currentLayer.Remove(_currentLayer.Last());
            _reader.Close();
            _reader.Dispose();
        }

        public void printTokens()
        {
            foreach (var token in generatedTokens)
            {
                Console.WriteLine(token.ToString());
            }
        }
    }
}
