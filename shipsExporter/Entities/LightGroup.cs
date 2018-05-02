using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CryEngine
{
    class LightGroup
    {

    }
   public  class Light
    {
        public string Name;
        public string translation;
        public string rotation;
        public string Scale;
        public string EntityClass;
        public string Type;
        public string Id;
        public string Layer;
        public string LayerGUID; 
        public XElement Properties; 
    }
    class EnvironmentProbe
    {
        public string Name;
        public string pos;
        public string rotate;
        public string EntityClass="Entity";
        public string Type= "EntityWithComponent";
        public string Id;
        public string Layer;
        public string LayerGUID;

        //Component
        public Vector3 BoxSize;
        public string GeneratedCubemapPath;
    }
}
