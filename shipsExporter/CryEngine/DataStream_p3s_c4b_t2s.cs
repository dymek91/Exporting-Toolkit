using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    class DataStream_p3s_c4b_t2s
    {
        public static uint elementSize = 16;
        public List<DataStream_p3s_c4b_t2s_Elem> p3s_c4b_t2sElem = new List<DataStream_p3s_c4b_t2s_Elem>();

        public byte[] serialized;

        public DataStream_p3s_c4b_t2s()
        {

        }

        public DataStream_p3s_c4b_t2s(uint nCount, byte[] dataStream)
        {
            //Stream stream = new MemoryStream(content);
            using (MemoryStream stream = new MemoryStream(dataStream))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    p3s_c4b_t2sElem = new List<DataStream_p3s_c4b_t2s_Elem>();

                    for (int i=0;i<nCount;i++)
                    {
                        DataStream_p3s_c4b_t2s_Elem elem = new DataStream_p3s_c4b_t2s_Elem();

                        elem.pos[0] = br.ReadUInt16();
                        elem.pos[1] = br.ReadUInt16();
                        elem.pos[2] = br.ReadUInt16();
                        elem.pos[3] = br.ReadUInt16();

                        elem.bgra[0] = br.ReadByte();
                        elem.bgra[1] = br.ReadByte();
                        elem.bgra[2] = br.ReadByte();
                        elem.bgra[3] = br.ReadByte();

                        elem.uv[0] = br.ReadUInt16();
                        elem.uv[1] = br.ReadUInt16();

                        p3s_c4b_t2sElem.Add(elem);
                    }
                }
            }
        }
        public void Serialize()
        {
            serialized = new byte[elementSize* p3s_c4b_t2sElem.Count];
            using (MemoryStream stream = new MemoryStream(serialized))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    for(int i=0;i<p3s_c4b_t2sElem.Count;i++)
                    {
                        p3s_c4b_t2sElem[i].Serialize();
                        bw.Write(p3s_c4b_t2sElem[i].serialized);
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
            return elementSize * (uint)p3s_c4b_t2sElem.Count;
        }
    }

    class DataStream_p3s_c4b_t2s_Elem
    {
        public static uint size = 16;

        public ushort[] pos = new ushort[4];
        public byte[] bgra = new byte[4];
        public ushort[] uv = new ushort[2];

        public byte[] serialized;

        public void Serialize()
        {
            serialized = new byte[size];
            using (MemoryStream stream = new MemoryStream(serialized))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        bw.Write(pos[i]);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        bw.Write(bgra[i]);
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        bw.Write(uv[i]);
                    }
                }
            }
        }
    }
}
