using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    /// <summary>
    /// 0x0000 0090 - visarea; 
    /// </summary>
    class AreaShape_VisArea_Type90
    {
        public const uint type = 0x0000090;

        public string name;  
        public Vector3 pos;
        public Vector4 rotate;
        public double height;
        public List<Point3D> points = new List<Point3D>();

        public AreaShape_VisArea_Type90(BinaryReader br, string areaShapeName)
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
