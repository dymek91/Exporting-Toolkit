using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
    class DataStream_Positions
    {
        public static uint elementSize = 12;
        public List<DataStream_Positions_Elem> position = new List<DataStream_Positions_Elem>();
    }
    class DataStream_Positions_Elem
    {
        public float[] pos = new float[3];
        public static uint elementSize = 12;
    }
}
