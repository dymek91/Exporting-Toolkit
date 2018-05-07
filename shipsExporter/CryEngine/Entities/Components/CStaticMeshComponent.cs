using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq; 

namespace CryEngine
{
    class CStaticMeshComponent : Component
    {
        virtual public string TypeName { get;} = "Cry::DefaultComponents::CStaticMeshComponent";
        virtual public string TypeGUID { get; } = "6ddd0033-6aaa-4b71-b8ea-108258205e29";
        public string CryXmlVersion { get; set; } = "2";
        public string Name { get; set; } = "";
        public string UserAdded { get; set; } = "true";
        public string GUID { get; set; }

        Transform _transform;
        Propeties _properties;

        public CStaticMeshComponent(string guid,string meshPath)
        {
            GUID = guid;
            _transform = new Transform();
            _properties = new Propeties();
            _properties.FilePath = meshPath;
        }

        public CStaticMeshComponent(string guid, string meshPath,Vector3 pos, Vector3 rot, Vector3 scale)
        {
            GUID = guid;
            _transform = new Transform(pos,rot,scale);
            _properties = new Propeties();
            _properties.FilePath = meshPath;
        }

        public XElement Get()
        {
            XElement component = new XElement("Component");

            component.Add(new XAttribute("TypeName", TypeName));
            component.Add(new XAttribute("TypeGUID", TypeGUID));
            component.Add(new XAttribute("CryXmlVersion", CryXmlVersion));
            component.Add(new XAttribute("Name", Name));
            component.Add(new XAttribute("UserAdded", UserAdded));
            component.Add(new XAttribute("GUID", GUID));

            component.Add(_transform.Get());
            component.Add(_properties.Get());

            return component;
        }

        private class Propeties
        {
            public string Type { get; set; } = "Render";
            public string FilePath { get; set; }
            public string Material { get; set; } = "";

            Render _render = new Render();
            Physics _physics = new Physics();

            public XElement Get()
            {
                XElement properties = new XElement("Properties");

                properties.Add(new XAttribute("Type", Type));
                properties.Add(new XAttribute("FilePath", FilePath));
                if(Material!="") properties.Add(new XAttribute("Material", Material));

                properties.Add(_render.Get());
                properties.Add(_physics.Get());

                return properties;
            }

            private class Render
            {
                public string ShadowSpec { get; set; } = "Low";
                public string IgnoreVisArea { get; set; } = "False";
                public string ViewDistRatio { get; set; } = "100";
                public string LODDistance { get; set; } = "100";
                public string GIMode { get; set; } = "None";

                public XElement Get()
                {
                    XElement render = new XElement("Render");

                    render.Add(new XAttribute("ShadowSpec", ShadowSpec));
                    render.Add(new XAttribute("IgnoreVisArea", IgnoreVisArea));
                    render.Add(new XAttribute("ViewDistRatio", ViewDistRatio));
                    render.Add(new XAttribute("LODDistance", LODDistance));
                    render.Add(new XAttribute("GIMode", GIMode));

                    return render;
                }
            }
            private class Physics
            {
                public string Mass { get; set; } = "10";
                public string Density { get; set; } = "0";

                public XElement Get()
                {
                    XElement physics = new XElement("Physics");

                    physics.Add(new XAttribute("Mass", Mass));
                    physics.Add(new XAttribute("Density", Density));

                    return physics;
                }
            }
        }
    }
}
