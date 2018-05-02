using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
    class DataStream_Normals
    {
        public static uint elementSize = 12;
        public List<DataStream_Normals_Elem> normal = new List<DataStream_Normals_Elem>();
    }
    class DataStream_Normals_Elem
    {
        float[] normal = new float[3];
        public static uint elementSize = 12;
    }
}
