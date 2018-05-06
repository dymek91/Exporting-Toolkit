using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    //SC exclusive
    class Object_Type10
    {
        public const uint type = 0x0000010;
        int size = 132;

        public Object_Type10(BinaryReader br)
        {
            br.BaseStream.Position = br.BaseStream.Position + size;
        }
    }
}
