using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    //SC exclusive
    class Chunk_AreaShape
    {
        public const uint type = 0x0000000E;

        int headerSize = 20;

        int visAreasCount;
        int portalsCount;

        int areaShapesStreamSize;

        public byte[] areaShapesStream = null;

        public Chunk_AreaShape(byte[] content)
        {
            areaShapesStream = new byte[content.Length];
            using (MemoryStream stream = new MemoryStream(content))
            { 
                using (BinaryReader br = new BinaryReader(stream))
                {
                    br.ReadUInt32();
                    areaShapesStreamSize = br.ReadInt32();
                    visAreasCount = br.ReadInt32();
                    portalsCount = br.ReadInt32();
                    br.ReadUInt32();
                    for (int i = 0; i < areaShapesStreamSize-headerSize; i++)
                        areaShapesStream[i] = br.ReadByte();
                }
            }
        }
        public int GetVisAreasCount()
        {
            return visAreasCount;
        }
        public int GetPortalsCount()
        {
            return portalsCount;
        }
    }
}
