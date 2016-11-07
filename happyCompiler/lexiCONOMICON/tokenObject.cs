using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace happyCompiler
{
    public class tokenObject
    {
        public tokenType _type { get; set; }
        private tokenContent _content { get; set; }
        private tokenCoordinates _coordinates { get; set; }

        public tokenObject(tokenType type, string content, tokenCoordinates coordinates)
        {
            _type = type;
            _content = new tokenContent();
            _content.setValue(content);
            _coordinates = coordinates;
        }

        public tokenObject(tokenType type, string content,int column, int row)
        {
            _type = type;
            _content = new tokenContent();
            _content.setValue(content);
            _coordinates = new tokenCoordinates(column, row);
        }

        public override string ToString()
        {
            return "TokenType: " + _type
                   + " | Content: " + _content.getValue()
                   + _coordinates.ToString();
        }

    }

}
