using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    class DataStream_Tangents_SC
    {
        public static uint elementSize = 8;
        public List<DataStream_Tangents_SC_Elem> tangent = new List<DataStream_Tangents_SC_Elem>();

        public byte[] serialized;

        public DataStream_Tangents_SC()
        {

        }
        public DataStream_Tangents_SC(uint nCount, byte[] dataStream)
        {
            using (MemoryStream stream = new MemoryStream(dataStream))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    tangent = new List<DataStream_Tangents_SC_Elem>();

                    for (int i = 0; i < nCount; i++)
                    {
                        DataStream_Tangents_SC_Elem elem = new DataStream_Tangents_SC_Elem();

                        elem.tangent = br.ReadUInt32();
                        elem.bitangent = br.ReadUInt32();

                        tangent.Add(elem);
                    }
                }
            }
        }
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
    }
    class DataStream_Tangents_SC_Elem
    {
        public static uint size = 8;

        public uint tangent;
        public uint bitangent; 

        public byte[] serialized;

        public void Serialize()
        {
            serialized = new byte[size];
            using (MemoryStream stream = new MemoryStream(serialized))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(tangent);
                    bw.Write(bitangent);
                }
            }
        }
    }
}
