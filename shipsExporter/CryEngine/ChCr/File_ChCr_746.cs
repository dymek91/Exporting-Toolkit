using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    class File_ChCr_746
    {
        public string filePath="";
        bool hasConvertedFlag = false;
        bool wrongSignature = false;

        public uint signatureString; //0x68437243 = CrCh = (uint) 1749250627
        public uint version = 0x00000746;
        public uint chunkCount;
        public uint chunkTableOffset;
        public List<Chunk> chunks = new List<Chunk>();

        uint fileHeaderSize = 16;
        uint singleChunkHeaderSize = 16;
        
        public File_ChCr_746()
        {
        }

        public File_ChCr_746(string path)
        {
            filePath = path;
            using (FileStream fs = File.Open(
                    path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    ReadBinary(br);
                } 
            }
        }
        public File_ChCr_746(Stream stream,string path="")
        {
            filePath = path; 
            using (BinaryReader br = new BinaryReader(stream))
            {
                ReadBinary(br);
            } 
        }
        void ReadBinary(BinaryReader br)
        {
            signatureString = br.ReadUInt32();
            if (signatureString == 1749250627)//ChCr
            {
                version = br.ReadUInt32();
                chunkCount = br.ReadUInt32();
                chunkTableOffset = br.ReadUInt32();

                br.BaseStream.Position = chunkTableOffset;
                for (int i = 0; i < chunkCount; i++)
                {
                    Chunk chunk = new Chunk();

                    chunk.type = br.ReadUInt16();
                    chunk.version = br.ReadUInt16();
                    chunk.chunkId = br.ReadUInt32();
                    chunk.size = br.ReadUInt32();
                    chunk.pos = br.ReadUInt32();

                    chunks.Add(chunk);
                }
                //optimization - dont load huge data when already converted
                if (!IsAlreadyConverted(br.BaseStream.Length, br))
                {
                    for (int i = 0; i < chunkCount; i++)
                    {
                        br.BaseStream.Position = chunks[i].pos;
                        chunks[i].content = new byte[chunks[i].size];
                        for (int j = 0; j < chunks[i].size; j++)
                            chunks[i].content[j] = br.ReadByte();
                    }
                }
                //if (fs.Length - br.BaseStream.Position>=9)
                //{
                //    string flag = new string( br.ReadChars(9));
                //    if (flag == "CONVERTED")
                //        hasConvertedFlag = true;
                //}
            }
            else
            {
                wrongSignature = true;
            }
        }
        bool IsAlreadyConverted(long fileLength,BinaryReader br)
        {
            bool alreadyConverted=false;

            long chunksEndPosition = chunks[ chunks.Count-1].pos + chunks[chunks.Count - 1].size;
            if (fileLength - chunksEndPosition >= 9)
            {
                br.BaseStream.Position = chunksEndPosition;
                string flag = new string(br.ReadChars(9));
                if (flag == "CONVERTED")
                {
                    hasConvertedFlag = true;
                    alreadyConverted = true;
                }
            }

            return alreadyConverted;
        }
        public void OverwriteChunksIds(uint startNumber)
        {
            uint idOffset = startNumber;
            idOffset++;
            for (int i=0;i<chunkCount;i++)
            {
                chunks[i].chunkId = chunks[i].chunkId+ idOffset; 
            }
            for (int i = 0; i < chunkCount; i++)
            {
                //patch Mesh (compiled) chunk nStreamChunkID
                if (chunks[i].type == 0x00001000 && chunks[i].version == 0x0000802)
                {
                    Chunk_Mesh_802 meshChunk = new Chunk_Mesh_802(chunks[i].content);
                    for (int j = 0; j < 128; j++)
                    {
                        if (meshChunk.nStreamChunkID[j] != 0)
                            meshChunk.nStreamChunkID[j] = meshChunk.nStreamChunkID[j] + idOffset;
                    }
                    meshChunk.nSubsetsChunkId = meshChunk.nSubsetsChunkId + idOffset;

                    meshChunk.Serialize();
                    chunks[i].content = meshChunk.serialized;
                }
                if (chunks[i].type == 0x00001000 && chunks[i].version == 0x0000801)
                {
                    Chunk_Mesh_801 meshChunk = new Chunk_Mesh_801(chunks[i].content);
                    for (int j = 0; j < 16; j++)
                    {
                        if (meshChunk.nStreamChunkID[j] != 0)
                            meshChunk.nStreamChunkID[j] = meshChunk.nStreamChunkID[j] + idOffset;
                    }
                    meshChunk.nSubsetsChunkId = meshChunk.nSubsetsChunkId + idOffset;

                    meshChunk.Serialize();
                    chunks[i].content = meshChunk.serialized;
                }
                //patch Node chunk ObjectID
                if (chunks[i].type == 0x0000100B)
                {
                    Chunk_Node_824 nodeChunk = new Chunk_Node_824(chunks[i].content);
                    nodeChunk.objectID = nodeChunk.objectID + idOffset;
                    if (nodeChunk.parentID != -1)
                        nodeChunk.parentID = nodeChunk.parentID + (int)idOffset;
                    nodeChunk.matID = nodeChunk.matID + idOffset;

                    nodeChunk.pos_cont_id = -1;
                    nodeChunk.rot_cont_id = -1;
                    nodeChunk.scl_cont_id = -1;

                    nodeChunk.Serialize();
                    chunks[i].content = nodeChunk.serialized;
                }
            }
        }
        public uint GetMaxId()
        {
            uint maxId=0;
            for (int i = 0; i < chunkCount; i++)
                if (chunks[i].chunkId > maxId) maxId = chunks[i].chunkId;
            return maxId;
        }
        public void RecalculateChunksPositions()
        {
            uint currentPosition = 0;
            currentPosition = currentPosition + fileHeaderSize;
            for(int i=0;i< chunkCount;i++)
            {
                currentPosition = currentPosition + singleChunkHeaderSize;
            }
            for(int i=0;i<chunkCount;i++)
            {
                chunks[i].pos = currentPosition;
                currentPosition = currentPosition + chunks[i].size;
            }
        }
        public Chunk GetChunkById(uint id)
        {
            Chunk retChunk = null;
            foreach(Chunk chunk in chunks)
            {
                if(chunk.chunkId == id)
                {
                    retChunk = chunk;
                }
            }
            return retChunk;
        }
        public void RenderAndSaveFile(string path,bool flagAsConverted=false)
        { 
            BinaryWriter bw = new BinaryWriter(File.Open(
                    path, FileMode.Open, FileAccess.ReadWrite));

            //write file header
            bw.Write(signatureString);
            bw.Write(version);
            bw.Write(chunkCount);
            bw.Write(chunkTableOffset);
            //write chunks headers
            for (int i = 0; i < chunkCount; i++)
            {
                bw.Write(chunks[i].type);
                bw.Write(chunks[i].version);
                bw.Write(chunks[i].chunkId);
                bw.Write(chunks[i].size);
                bw.Write(chunks[i].pos);
            }
            //write chunks contents
            for (int i = 0; i < chunkCount; i++)
            {
                for (int j = 0; j < chunks[i].size; j++)
                {
                    bw.Write(chunks[i].content[j]);
                }
            }
            if(flagAsConverted && !hasConvertedFlag)
            {
                bw.Write("CONVERTED".ToCharArray());
            }
            bw.Close();
        }
        public bool HasConvertedFlag()
        {
            return hasConvertedFlag;
        }
        public bool HasWrongSignature()
        {
            return wrongSignature;
        }
    }
}
