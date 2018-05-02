using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    //SC exclusive
    class Object_Type7
    {
        public const uint type = 0x0000007;
        int size = 148;

        public Object_Type7(BinaryReader br)
        {
            br.BaseStream.Position = br.BaseStream.Position + size;
        }
    }
}
