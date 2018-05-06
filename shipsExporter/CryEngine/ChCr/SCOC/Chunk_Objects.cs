using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    //SC exclusive
    class Chunk_Objects
    {
        public const uint type = 0x00000010;

        string[] filesPaths;
        int filesPathsCount;

        string[] filesPaths1;
        int filesPaths1Count;

        string[] filesPaths2;
        int filesPaths2Count;

        int objectsStreamSize;

        public byte[] objectsStream = null;

        public Chunk_Objects(byte[] content)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    br.ReadUInt32();
                    filesPaths1Count = br.ReadInt32();
                    filesPaths1 = new string[filesPaths1Count];
                    for(int i = 0;i< filesPaths1Count;i++)
                    {
                        filesPaths1[i] = new string(br.ReadChars(256)).TrimEnd('\0');
                    }
                    filesPaths2Count = br.ReadInt32();
                    filesPaths2 = new string[filesPaths2Count];
                    for (int i = 0; i < filesPaths2Count; i++)
                    {
                        filesPaths2[i] = new string(br.ReadChars(256)).TrimEnd('\0');
                    }
                    //unknown
                    br.ReadUInt32(); br.ReadUInt32(); br.ReadUInt32();
                    br.ReadUInt32(); br.ReadUInt32(); br.ReadUInt32(); br.ReadUInt32();
                    objectsStreamSize = br.ReadInt32();
                    objectsStream = new byte[objectsStreamSize];
                    for (int i = 0; i < objectsStreamSize; i++)
                        objectsStream[i] = br.ReadByte();
                }
            }
            MergeFilesPathsArrays();
        }
        void MergeFilesPathsArrays()
        {
            filesPaths = new string[filesPaths1.Length + filesPaths2.Length];
            Array.Copy(filesPaths1, filesPaths, filesPaths1.Length);
            Array.Copy(filesPaths2, 0, filesPaths, filesPaths1.Length, filesPaths2.Length);
        }
        public int GetFilesCount()
        {
            return filesPaths.Length;
        }
        public string GetFilePath(int id)
        {
            return filesPaths[id];
        }
        public string[] GetFilePaths()
        {
            return filesPaths;
        }
    }
}
