using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace happyCompiler
{
    class tokenCoordinates
    {
        int _column;
        int _row;

        public tokenCoordinates(int column, int row)
        {
            _column = column;
            _row = row;
        }

        public void setCoordinates(int column, int row)
        {
            _column = column;
            _row = row;
        }

        
        public int getColumn() { return _column; }
        public int getRow() { return _row; }
        

    }
}
