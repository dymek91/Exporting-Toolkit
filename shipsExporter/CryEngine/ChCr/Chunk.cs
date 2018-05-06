using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
    class Chunk
    {
        public ushort type;
        public ushort version;
        public uint chunkId;
        public uint size;
        public uint pos;
        public byte[] content;

        public Chunk()
        {

        }
        public Chunk(uint id)
        {
            chunkId = id;
        }
    }
}
