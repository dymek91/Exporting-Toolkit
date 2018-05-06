using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    class Chunk_DataStream_800
    {
        static uint headerSize = 24;
        public uint nFlags;
        public uint nStreamType;
        public uint nCount;
        public uint nElementSize;
        public uint[] reserved = new uint[2];
        public byte[] dataStream = null;

        public byte[] serialized;

        public Chunk_DataStream_800()
        {

        }
        public Chunk_DataStream_800(byte[] content)
        {
            //Stream stream = new MemoryStream(content);
            using (MemoryStream stream = new MemoryStream(content))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    nFlags = br.ReadUInt32();
                    nStreamType = br.ReadUInt32();
                    nCount = br.ReadUInt32();
                    nElementSize = br.ReadUInt16();
                    br.ReadUInt16();
                    reserved[0] = br.ReadUInt32();
                    reserved[1] = br.ReadUInt32();
                    dataStream = new byte[nCount* nElementSize];
                    for (int i = 0; i < nCount * nElementSize; i++)
                        dataStream[i] = br.ReadByte();
                }
            }
        }
        public void Serialize()
        {
            serialized = new byte[headerSize + nCount* nElementSize];
            using (MemoryStream stream = new MemoryStream(serialized))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(nFlags);
                    bw.Write(nStreamType);
                    bw.Write(nCount);
                    bw.Write(nElementSize);
                    bw.Write(reserved[0]);
                    bw.Write(reserved[1]);
                    for (int i = 0; i < nCount * nElementSize; i++)
                        bw.Write(dataStream[i]);

                }
            }
        }
        public uint GetElementSize()
        {
            return nElementSize;
        } 
        public uint GetSize()
        {
            uint size = 0;
            size = headerSize + nCount * nElementSize;
            return size;
        }
    }
}
