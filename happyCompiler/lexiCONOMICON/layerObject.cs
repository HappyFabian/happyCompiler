using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexiCONOMICON
{
    class layerObject
    {
        public string _layername;
        public tokenCoordinates _coordinates;

        public layerObject(string value, tokenCoordinates coordinates)
        {
            _layername = value;
            _coordinates = coordinates;
        }

        public override string ToString()
        {
            return "Layername: " + _layername + " , Coordinates: " + _coordinates.ToString();
        }
    }
}
