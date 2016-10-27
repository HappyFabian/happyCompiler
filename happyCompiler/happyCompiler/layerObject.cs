using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace happyCompiler
{
    class layerObject
    {
        public string LAYERNAME;
        public tokenCoordinates coordinates;

        public layerObject(string value, int column, int row)
        {
            LAYERNAME = value;
            coordinates = new tokenCoordinates(column, row);
        }
    }
}
