using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CryEngine
{
    class Point3D
    {
        public float x { get { return posv.x; } set { } }
        public float y { get { return posv.y; } set { } }
        public float z { get { return posv.z; } set { } }
        Vector3 posv;
        string _pos;
        public string Pos
        {
            get { return _pos; }
            set
            {
                _pos = value;
                string[] coords;
                coords = _pos.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                posv.x = float.Parse(coords[0], CultureInfo.InvariantCulture.NumberFormat);
                posv.y = float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat);
                posv.z = float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat);
            }
        }
        public Point3D(string posstr)
        {
            Pos = posstr;
        }
        public Point3D(Vector3 posvec)
        {
            string format = "N6";
            Pos = posvec.x.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
                + "," + posvec.y.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
                + "," + posvec.z.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray());
        }
        public Point3D(float x, float y, float z)
        {
            string format = "N6";
            Pos = x.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
                + "," + y.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
                + "," + z.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray());
        }
    }
}
