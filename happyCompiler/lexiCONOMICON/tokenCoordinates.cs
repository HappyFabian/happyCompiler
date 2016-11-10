using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexiCONOMICON
{
    public class tokenCoordinates
    {
        private int _column;
        private int _row;

        public tokenCoordinates(){ _column = 0; _row = 0; }
        public tokenCoordinates(int column, int row){ _column = column; _row = row; }

        public override string ToString()
        {
            return " | Column: " + _column + " Row: " + _row;
        }

        public tokenCoordinates DeepCopy()
        {
            return new tokenCoordinates(_column,_row);
        }

        public void setCoordinates(int column, int row){ _column = column; _row = row; }

        public void IncreaseColumn(){ _column++; }
        public void ResetColumn(){ _column = 0; }
        public void IncreaseRow(){ _row++; ResetColumn();}
        public void ResetRow(){ _row = 0; }
        public void resetEverything() { ResetColumn(); ResetRow();}
        
        public int getColumn() { return _column; }
        public int getRow() { return _row; }
        

    }
}
