using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    class DataStream_p3f_c4b_t2s
    {
        public static uint elementSize = 20;
        public List<DataStream_p3f_c4b_t2s_Elem> p3f_c4b_t2sElem = new List<DataStream_p3f_c4b_t2s_Elem>();

        public byte[] serialized;

        public DataStream_p3f_c4b_t2s()
        {

        }

        public DataStream_p3f_c4b_t2s(uint nCount, byte[] dataStream)
        {
            //Stream stream = new MemoryStream(content);
            using (MemoryStream stream = new MemoryStream(dataStream))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    p3f_c4b_t2sElem = new List<DataStream_p3f_c4b_t2s_Elem>();

                    for (int i = 0; i < nCount; i++)
                    {
                        DataStream_p3f_c4b_t2s_Elem elem = new DataStream_p3f_c4b_t2s_Elem();

                        elem.pos[0] = br.ReadSingle();
                        elem.pos[1] = br.ReadSingle();
                        elem.pos[2] = br.ReadSingle(); 

                        elem.bgra[0] = br.ReadByte();
                        elem.bgra[1] = br.ReadByte();
                        elem.bgra[2] = br.ReadByte();
                        elem.bgra[3] = br.ReadByte();

                        elem.uv[0] = br.ReadUInt16();
                        elem.uv[1] = br.ReadUInt16();

                        p3f_c4b_t2sElem.Add(elem);
                    }
                }
            }
        }
        public void Serialize()
        {
            serialized = new byte[elementSize * p3f_c4b_t2sElem.Count];
            using (MemoryStream stream = new MemoryStream(serialized))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    for (int i = 0; i < p3f_c4b_t2sElem.Count; i++)
                    {
                        p3f_c4b_t2sElem[i].Serialize();
                        bw.Write(p3f_c4b_t2sElem[i].serialized);
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
            return elementSize * (uint)p3f_c4b_t2sElem.Count;
        }
    }
    class DataStream_p3f_c4b_t2s_Elem
    {
        public static uint size = 20;

        public float[] pos = new float[3];
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
                    for (int i = 0; i < 3; i++)
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
