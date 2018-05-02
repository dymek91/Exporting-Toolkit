using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
    class DataStream_Colors
    {
        public static uint elementSize = 4;
        public List<DataStream_Colors_Elem> color = new List<DataStream_Colors_Elem>();
    }
    class DataStream_Colors_Elem
    {
        public byte[] rgba = new byte[4];
        public static uint elementSize = 4;
    }
}
