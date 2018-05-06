using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
    class DataStream_Indices
    {
        public static uint elementSize = 2;
        public List<DataStream_Indices_Elem> indice = new List<DataStream_Indices_Elem>();
    }
    class DataStream_Indices_Elem
    {
        ushort index;
    }
}
