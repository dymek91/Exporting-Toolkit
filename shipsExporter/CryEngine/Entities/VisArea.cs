using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
    class VisArea
    {
        uint type = 0;

        string name;
        Vector3 pos;
        Vector4 rotate;
        double height;
        List<Point3D> points = new List<Point3D>();

        public VisArea(string visAreaName, Vector3 position, Vector4 rotation, double visAreaHeight, List<Point3D> visAreaPoints, uint visAreaType=0)
        {
            type = visAreaType;

            name = visAreaName;
            pos = position;
            rotate = rotation;
            height = visAreaHeight;
            points = visAreaPoints;
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
            //Vector4 fixedRotation = new Vector4(rotate.w, rotate.x, rotate.y, rotate.z);
            //return fixedRotation;
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
        public uint GetVisAreaType()
        {
            return type;
        }
        public double GetHeight()
        {
            return height;
        }
    }
}
