using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
    class CAnimatedMeshComponent : CStaticMeshComponent
    {
        override public string TypeName { get; } = "Cry::DefaultComponents::CAnimatedMeshComponent";
        override public string TypeGUID { get; } = "5f543092-53ea-46d7-9376-266e778317d7";

        public CAnimatedMeshComponent(string guid, string meshPath)
            : base(guid, meshPath)
        { 
        }
        public CAnimatedMeshComponent(string guid, string meshPath, Vector3 pos, Vector3 rot, Vector3 scale)
             : base(guid, meshPath,  pos,  rot,  scale)
        { 
        }
    }
}
