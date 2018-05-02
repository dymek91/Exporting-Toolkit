using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
    public static class Utilities
    {
        public static string FormatFloat(float x)
        {
            return x.ToString("N6").TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray());
        }
    }
}
