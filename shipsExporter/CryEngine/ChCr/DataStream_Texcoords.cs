using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
    class DataStream_Texcoords
    {
        public static uint elementSize = 8;
        public List<DataStream_Texcoords_Elem> texcoord = new List<DataStream_Texcoords_Elem>();
    }
    class DataStream_Texcoords_Elem
    {
        public float[] uv = new float[2];
        public static uint elementSize = 8;
    }
}
