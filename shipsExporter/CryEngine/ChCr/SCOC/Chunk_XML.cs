using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    //SC exclusive
    class Chunk_XML
    {
        public const uint type = 0x00000004;

        public byte[] xmlStream = null;

        public Chunk_XML(byte[] content)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    xmlStream = new byte[content.Length];
                    for (int i = 0; i < content.Length; i++)
                        xmlStream[i] = br.ReadByte();
                }
            }
        }
    }
}
