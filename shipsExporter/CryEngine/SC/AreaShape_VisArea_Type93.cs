using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    /// <summary> 
    /// 0x0000 0093 - 3d_visarea
    /// </summary>
    class AreaShape_VisArea_Type93
    {
        public const uint type = 0x0000093;

        public string name;
        public Vector3 pos;
        public Vector4 rotate;
        public double height;
        public List<Point3D> points = new List<Point3D>();

        //dev
        uint uknown1Count = 0;
        double[,] uknown1;
        uint uknown2Count = 0;
        int[] uknown2;

        public AreaShape_VisArea_Type93(BinaryReader br, string areaShapeName)
        {
            name = areaShapeName;

            br.ReadUInt32();
            br.ReadUInt32();
            br.ReadUInt32();
            height = br.ReadDouble();
            rotate.x = (float)br.ReadDouble();
            rotate.y = (float)br.ReadDouble();
            rotate.z = (float)br.ReadDouble();
            rotate.w = (float)br.ReadDouble();
            pos.x = (float)br.ReadDouble();
            pos.y = (float)br.ReadDouble();
            pos.z = (float)br.ReadDouble();

            uint pointsCount = 0;
            pointsCount = br.ReadUInt32();
            for (int i = 0; i < pointsCount; i++)
            {
                points.Add(new Point3D(
                    (float)br.ReadDouble(),
                    (float)br.ReadDouble(),
                    (float)br.ReadDouble()
                    ));
            } 
            uknown1Count = br.ReadUInt32();
            uknown1 = new double[uknown1Count,4];
            for (int i = 0; i < uknown1Count; i++)
            {
                //br.ReadChars(32);
                uknown1[i, 0]=br.ReadDouble();
                uknown1[i, 1] = br.ReadDouble();
                uknown1[i, 2] = br.ReadDouble();
                uknown1[i, 3] = br.ReadDouble();
            } 
            uknown2Count = br.ReadUInt32();
            uknown2 = new int[uknown2Count];
            for (int i = 0; i < uknown2Count; i++)
            {
                uknown2[i]=br.ReadInt32();
            }
        }
        public string GetName()
        {
            return name;
        }
        public Vector3 GetPosition()
        {
            return pos;
        }
        public Vector4 GetRotation()
        {
            return rotate;
        }
        public List<Point3D> GetPoints()
        {
            return points;
        }
        public Point3D GetPoint(int id)
        {
            return points[id];
        }
        public double GetHeight()
        {
            return height;
        }
    }
}
