using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    //SC exclusive
    class Object_Type1
    {
        public const uint type = 0x0000001;
        int size = 172;

        public CryEngine.Matrix3x4 rotMatrix34;
        public CryEngine.Vector3 vector1;
        public CryEngine.Vector3 vector2;
        public CryEngine.Vector3 pos;
        public CryEngine.Vector4 rotation;
        public CryEngine.Vector3 scale;
        public CryEngine.Quaternion quaternion;
        public int id;
        public uint temp1;

        public string path = "";
        string[] filesPaths;

        public Object_Type1(BinaryReader br,string[] filePathsCollection)
        {
            filesPaths = filePathsCollection;

            /////////////////////////
            vector1.X = (float)br.ReadDouble();
            vector1.Y = (float)br.ReadDouble();
            vector1.Z = (float)br.ReadDouble();
            vector2.X = (float)br.ReadDouble();
            vector2.Y = (float)br.ReadDouble();
            vector2.Z = (float)br.ReadDouble();
            br.ReadUInt32();
            br.ReadUInt32();
            id = br.ReadInt16();
            temp1 = br.ReadUInt16();
            rotMatrix34.m00 = (float)br.ReadDouble();
            rotMatrix34.m01 = (float)br.ReadDouble();
            rotMatrix34.m02 = (float)br.ReadDouble();
            rotMatrix34.m03 = (float)br.ReadDouble();
            rotMatrix34.m10 = (float)br.ReadDouble();
            rotMatrix34.m11 = (float)br.ReadDouble();
            rotMatrix34.m12 = (float)br.ReadDouble();
            rotMatrix34.m13 = (float)br.ReadDouble();
            rotMatrix34.m20 = (float)br.ReadDouble();
            rotMatrix34.m21 = (float)br.ReadDouble();
            rotMatrix34.m22 = (float)br.ReadDouble();
            rotMatrix34.m23 = (float)br.ReadDouble();
            br.BaseStream.Position = br.BaseStream.Position + 16;
            //////////////////////////

            pos = rotMatrix34.GetTranslation();
            scale = rotMatrix34.GetScale();
            rotMatrix34.DescaleRotation();
            quaternion = new Quaternion(rotMatrix34);
            //quaternion = quaternion * new Quaternion(-0.7071f, 0, 0, 0.7071f);
            //quaternion = new Quaternion(quaternion.x, quaternion.z, (-1)*quaternion.y, quaternion.w);
            //quaternion = quaternion.Normalized;
            rotation.x = quaternion.x;
            rotation.y = quaternion.y;
            rotation.z = quaternion.z;
            rotation.w = quaternion.w;

            path = GetFilePath(id);
        }
        string GetFilePath(int id)
        {
            if (id>=0)
            {
                return filesPaths[id];
            }else
            {
                Console.WriteLine("Object_Type1::GetFilePath | Invalid ID {0}", id);
                return "";
            }
        }
    }
}
