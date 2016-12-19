using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexiCONOMICON;
using parSEER.Semantics.Tree.Sentences;

namespace parSEER
{
    public class parserHolder
    {
    

     //   private exceptionMaster _exceptionMaster;
        private List<tokenObject> _lexiconomiconTokenList;

        public List<statementNode> generatedStatementNodes;
        private int _currentIndexOfToken;

        public parserHolder(List<tokenObject> tokenListFromLexiconomicon )
        {
            _lexiconomiconTokenList = tokenListFromLexiconomicon;
            _currentIndexOfToken = 0;
         //   _exceptionMaster = new exceptionMaster();
        }

        public tokenObject getCurrentToken()
        {
            if (_currentIndexOfToken >= _lexiconomiconTokenList.Count)
            {
                exceptionMaster.throwException(001,getCurrentToken());
            }
            return _lexiconomiconTokenList[_currentIndexOfToken];
        }

        public tokenType getCurrentTokenType()
        {
            return getCurrentToken()._type;
        }

        public void advanceIndex()
        {
            if (_currentIndexOfToken >= _lexiconomiconTokenList.Count)
            {
                exceptionMaster.throwException(002, getCurrentToken());
            }
            _currentIndexOfToken++;
        }

        public void retractIndex()
        {
            if (_currentIndexOfToken <= 0)
            {
                exceptionMaster.throwException(005, getCurrentToken());
            }
            _currentIndexOfToken--;

        }

        public void throwException(int flag, tokenObject currenTokenObject, tokenType expected = tokenType.NaN)
        {
           exceptionMaster.throwException(flag, currenTokenObject, expected);
        }

     


    }

}
