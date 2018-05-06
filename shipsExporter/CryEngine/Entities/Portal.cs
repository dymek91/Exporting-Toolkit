using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
    class Portal
    {
        uint type=0;

        string name;
        Vector3 pos;
        Vector4 rotate;
        double height;
        List<Point3D> points = new List<Point3D>();

        public Portal(string portalName, Vector3 position,Vector4 rotation,double portalHeight, List<Point3D> portalPoints,uint portalType=0)
        {
            type = portalType;

            name= portalName;
            pos= position;
            rotate= rotation;
            height= portalHeight;
            points = portalPoints;
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
        public uint GetPortalType()
        {
            return type;
        }
        public double GetHeight()
        {
            return height;
        }
    }
}
