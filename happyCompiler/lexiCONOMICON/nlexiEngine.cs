using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using happyCompiler;

namespace lexiCONOMICON
{
    public class nlexiEngine
    {
        private bool _reachedEndOfFile = false;

        private string _path;
        private Encoding _encoding;
        private StreamReader _reader;
        private lexiDictionary _dictionary;

        private readonly tokenCoordinates _currentLocation;
        private tokenCoordinates _lastLocation;

        private List<layerObject> _layerStack;
        private List<tokenObject> _generatedTokens;

        private string _currentLexeme;
        private char _lastCharacterRead;
        private char _currentCharacterRead;
        private char _nextCharacterRead;

        public nlexiEngine(string PATH, Encoding ENCODING)
        {
            //Set up the Streamer
            _path = PATH;
            _encoding = ENCODING ?? Encoding.Default;
            _reader = new StreamReader(_path, _encoding);
            //Set up the Locations.
            _currentLocation = new tokenCoordinates();
            _lastLocation = _currentLocation.DeepCopy();
            //Set up the Stacks.
            _layerStack = new List<layerObject>();
            //Set up the Dictionary.
            _dictionary = new lexiDictionary();
            //Set up the Generated Token Repository.
            _generatedTokens = new List<tokenObject>();
            //Set up the Lexeme.
            _currentLexeme = "";
            _lastCharacterRead = '\0';
            _currentCharacterRead = '\0';
            _nextCharacterRead = (char)_reader.Peek();
            //Set up the Character Streamer.
            AddLayer("DEPTHZERO");

        }

        public void CloseReader() { _reader.Dispose(); _reader.Close(); }

        private void PrintCurrentLocation() { Console.WriteLine("Current Location -" + _currentLocation.ToString()); }
        private void AdvanceColumn() { _currentLocation.IncreaseColumn(); }
        private void AdvanceRow() { _currentLocation.IncreaseRow(); }
        private void AdvanceCoordinateCursor()
        {
            if (IsCurrentCharNewLine()) { AdvanceRow(); }
            else { AdvanceColumn(); }
        }
        private void AdvanceReaderCursor(bool IgnoreSpecialCharacters =false)
        {
            _lastCharacterRead = _currentCharacterRead;
            _currentCharacterRead = (char)_reader.Read();
            _nextCharacterRead = (char)_reader.Peek();
        }

        private void AdvanceCursors(){ AdvanceCoordinateCursor();AdvanceReaderCursor();}

        private layerObject LastAddedLayer() { return _layerStack.Last(); }
        private void AddLayer(string value) { _layerStack.Add(new layerObject(value, _currentLocation.DeepCopy())); }
        private void PopLastLayer(){_layerStack.Remove(_layerStack.Last());}
        private void LogLastLayer(){Console.WriteLine(LastAddedLayer().ToString());}

        private void AddContext(string Lexeme){ _currentLexeme += Lexeme; }
        private void ClearContent(){ _currentLexeme = ""; }

        private void CurrentTokenBeginning(){_lastLocation = _currentLocation.DeepCopy();}
        private bool IsCurrentCharCarriageReturn(){ return _currentCharacterRead == '\r'; }
        private bool IsCurrentCharNewLine(){ return _currentCharacterRead == '\n'; }

        private bool IsWhiteSpaceMinusNewLine()
        {
            return (char.IsWhiteSpace(_currentCharacterRead) && !IsCurrentCharNewLine());
        }

        public bool CheckReaderIsAtEndOfStream(){ return _reader.EndOfStream;}
        private tokenObject EndOfFileToken()
        {
            CurrentTokenBeginning();
            return new tokenObject(tokenType.system_EndOfFile,"[END OF FILE]", _lastLocation);
        }

        private bool HaveIReachedEndOfFileTwice()
        {
            if (_reachedEndOfFile) return true;
            _reachedEndOfFile = true;
            return false;
        }

        private bool IsCharacterRecognizable(char input)
        {
            return char.IsLetterOrDigit(input) | char.IsSymbol(input) | char.IsPunctuation(input);
        }

        private string IdentifyCharacter(char input)
        {
            var returnValue = "NONE";
            if (char.IsLetter(input)){ returnValue = "LETTER";}
            if (char.IsDigit(input)){ returnValue = "DIGIT";}
            if (char.IsPunctuation(input)){ returnValue = "PUNCTUATION";}
            if (char.IsSymbol(input)){returnValue = "SYMBOL";}
            return returnValue;
        }

        public tokenObject GenerateToken()
        {
            AdvanceReaderCursor();
            AdvanceCoordinateCursor();

            switch (LastAddedLayer()._layername)
            {
                case "COMMENTLINE":
                    break;
                case "COMMENTBLOCK":
                    break;
                case "DEPTHZERO":
                    
                    goto default;
                default:
                    if(ConsumeWhitespaces()) {ClearContent();}
                    if (IsCharacterRecognizable(_currentCharacterRead))
                    {
                        CurrentTokenBeginning();
                        switch (IdentifyCharacter(_currentCharacterRead))
                        {
                            case "LETTER":
                                return ProcessWord(); break;
                            case "DIGIT":
                                return ProcessNumber();
                                break;
                            case "PUNCTUATION": 
                            case "SYMBOL":
                                return ProcessSymbolsAndPunctuations();
                                break;
                        }   
                    }
                    if (CheckReaderIsAtEndOfStream()){if (HaveIReachedEndOfFileTwice()) { return EndOfFileToken();} }
                    break;
            }

            _currentLexeme = _currentCharacterRead.ToString();
            //Unidentified Character Found.
            return new tokenObject(tokenType.ErrorToken, "UCF. " + _currentLexeme, _lastLocation);
        }

        private bool ConsumeWhitespaces()
        {

            if (!char.IsWhiteSpace(_currentCharacterRead)) return false;
            while (char.IsWhiteSpace(_currentCharacterRead))
            {
                AdvanceCursors();
            }
            return true;
        }

        private tokenObject ProcessSymbolsAndPunctuations()
        {
            _currentLexeme = _currentCharacterRead.ToString();
            tokenType probableTokenType;
            /*
            if (_dictionary.identifySymbolAndPunctuation(_currentLexeme) != tokenType.ErrorToken)
            {
                probableTokenType = _dictionary.identifySymbolAndPunctuation(_currentLexeme);
            }
            else
            {
                throw new Exception("This Symbol Does not Exist. " + _currentLexeme + " At: " + _lastLocation);
            }
            var continueLooping = true;
            while ((char.IsSymbol(_nextCharacterRead) || char.IsPunctuation(_nextCharacterRead)) && continueLooping)
            {                
                if (_dictionary.identifySymbolAndPunctuation(_currentLexeme + _nextCharacterRead) != tokenType.ErrorToken)
                {
                    AdvanceCursors();
                    _currentLexeme += _currentCharacterRead;
                    probableTokenType = _dictionary.identifySymbolAndPunctuation(_currentLexeme);
                }
                else
                {
                    continueLooping = false;
                }

            }
            */
            var newToken = new tokenObject(probableTokenType, _currentLexeme,_lastLocation);
            ClearContent();
            return newToken;
        }

        private tokenObject ProcessWord()
        {
            _currentLexeme = _currentCharacterRead.ToString();
            while (char.IsLetterOrDigit(_nextCharacterRead))
            {
                AdvanceCursors();
                _currentLexeme += _currentCharacterRead;
            }
            var newToken = new tokenObject(_dictionary.identifyString(_currentLexeme), _currentLexeme, _lastLocation);
            ClearContent();
            return newToken;
        }

        private tokenObject ProcessNumber()
        {
            _currentLexeme = _currentCharacterRead.ToString();

            var whatKindOfNumberIsIt = "NUMBER";
            if (_currentCharacterRead == '0')
            {
                switch (_nextCharacterRead.ToString())
                {
                    case "ADVANCE":
                        AdvanceCursors();
                        _currentLexeme += _currentCharacterRead;
                        break;
                    case ".":
                        whatKindOfNumberIsIt = "FLOAT";
                        goto case "ADVANCE";
                    case "0": case "1": case "2": case "3": case "4":
                    case "5": case "6": case "7":case "8": case "9":
                        whatKindOfNumberIsIt = "OCTAL";
                        goto case "ADVANCE";
                    case "x":
                        whatKindOfNumberIsIt = "HEXADECIMAL";
                        goto case "ADVANCE";
                    default:
                        break;
                }
                if (_nextCharacterRead == '.')
                {
                    switch (whatKindOfNumberIsIt)
                    {
                        case "ADVANCE":
                            AdvanceCursors();
                            _currentLexeme += _currentCharacterRead;
                            break;
                        case "NUMBER":
                            whatKindOfNumberIsIt = "FLOAT";
                            goto case "ADVANCE";
                        case "OCTAL":
                            whatKindOfNumberIsIt = "FLOAT";
                            goto case "ADVANCE";
                        case "HEXADECIMAL":
                            throw new Exception("HEXADECIMALS CAN NOT CONTAIN PERIODS. " + _lastLocation);
                            break;
                        case "FLOAT":
                            throw new Exception("FLOATS DO NOT HAVE MULTIPLE DOTS. " + _lastLocation);
                            break;
                    }
                }
                if (char.IsDigit(_nextCharacterRead))
                {
                    while (char.IsDigit(_nextCharacterRead))
                    {
                        AdvanceCursors();
                        _currentLexeme += _currentCharacterRead;

                        if (_nextCharacterRead == '.')
                        {
                            switch (whatKindOfNumberIsIt)
                            {
                                case "ADVANCE":
                                    AdvanceCursors();
                                    _currentLexeme += _currentCharacterRead;
                                    break;
                                case "NUMBER":
                                    whatKindOfNumberIsIt = "FLOAT";
                                    goto case "ADVANCE";
                                case "OCTAL":
                                    whatKindOfNumberIsIt = "FLOAT";
                                    goto case "ADVANCE";
                                case "HEXADECIMAL":
                                    throw new Exception("HEXADECIMALS CAN NOT CONTAIN PERIODS. " + _lastLocation);
                                    break;
                                case "FLOAT":
                                    throw new Exception("FLOATS DO NOT HAVE MULTIPLE DOTS. " + _lastLocation);
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    if (whatKindOfNumberIsIt != "OCTAL")
                    {
                        throw new Exception("WRONG NUMBER DEFINITION AT: " + _lastLocation);
                    }
                }
            }
            else
            {
                while (char.IsDigit(_nextCharacterRead))
                {
                    AdvanceCursors();
                    _currentLexeme += _currentCharacterRead;
                    if (_nextCharacterRead == '.')
                    {
                        switch (whatKindOfNumberIsIt)
                        {
                            case "ADVANCE":
                                AdvanceCursors();
                                _currentLexeme += _currentCharacterRead;
                                break;
                            case "NUMBER":
                                whatKindOfNumberIsIt = "FLOAT";
                                goto case "ADVANCE";
                            case "OCTAL":
                                whatKindOfNumberIsIt = "FLOAT";
                                goto case "ADVANCE";
                            case "HEXADECIMAL":
                                throw new Exception("HEXADECIMALS CAN NOT CONTAIN PERIODS. " + _lastLocation);
                                break;
                            case "FLOAT":
                                throw new Exception("FLOATS DO NOT HAVE MULTIPLE DOTS. " + _lastLocation);
                                break;
                        }
                    }
                }
            }

            var whatKindOfTokenIsIt = tokenType.NaN;
            switch (whatKindOfNumberIsIt)
            {
                case "NUMBER":
                    whatKindOfTokenIsIt = tokenType.literal_NUMBER;
                    break;
                case "OCTAL":
                    whatKindOfTokenIsIt = tokenType.literal_OCTAL;
                    break;
                case "HEXADECIMAL":
                    whatKindOfTokenIsIt = tokenType.literal_HEXADECIMAL;
                    break;
                case "FLOAT":
                    whatKindOfTokenIsIt = tokenType.literal_FLOAT;
                    break;
            }
            

            var newToken = new tokenObject(whatKindOfTokenIsIt, _currentLexeme, _lastLocation);
            ClearContent();
            return newToken;
        }

        public void GenerateAllTokens()
        {
            tokenObject currentToken;
            do
            {
                currentToken = GenerateToken();
                _generatedTokens.Add(currentToken);
            } while (currentToken._type != tokenType.system_EndOfFile);
            CloseReader();
        }

        public void PrintTokens()
        {
            foreach (var generatedToken in _generatedTokens)
            {
                Console.WriteLine(generatedToken.ToString());
            }
        }
    }
}
