using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    class DataStream_Tangents
    {
        public static uint elementSize = 16;
        public List<DataStream_Tangents_Elem> tangent = new List<DataStream_Tangents_Elem>();

        public byte[] serialized;

        public void Serialize()
        {
            serialized = new byte[elementSize * tangent.Count];
            using (MemoryStream stream = new MemoryStream(serialized))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    for (int i = 0; i < tangent.Count; i++)
                    {
                        tangent[i].Serialize();
                        bw.Write(tangent[i].serialized);
                    }
                }
            }
        }
        public uint GetElementSize()
        {
            return elementSize;
        }
        public uint GetSize()
        {
            return elementSize* (uint)tangent.Count;
        }
    }
    class DataStream_Tangents_Elem
    {
        public ushort[] tangent_binormal = new ushort[8];
        public static uint size = 16;

        public byte[] serialized;

        public void Serialize()
        {
            serialized = new byte[size];
            using (MemoryStream stream = new MemoryStream(serialized))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        bw.Write(tangent_binormal[i]);
                    } 
                }
            }
        }
    }
}
