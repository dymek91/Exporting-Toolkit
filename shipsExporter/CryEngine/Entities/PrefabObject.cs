using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace CryEngine
{
    public class PrefabObject
    {
        string _pos;
        string _rot;
        string _scale;
        string _VisAreaPos;
        public List<Light> lights;
        public List<PrefabObject> attachments = new List<PrefabObject>();
        public string Name;
        public string Type;
        public string Layer;
        public string LayerGUID;
        public string Id;
        public string ParentId;
        public string Rotate
        {
            get { return _rot; }
            set
            {
                //Console.WriteLine("sdf");
                _rot = value;
                string[] coords;
                coords = _rot.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                rotv.w = float.Parse(coords[0], CultureInfo.InvariantCulture.NumberFormat);
                rotv.x = float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat);
                rotv.y = float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat);
                rotv.z = float.Parse(coords[3], CultureInfo.InvariantCulture.NumberFormat);
            }
        }
        public string Scale
        {
            get { return _scale; }
            set
            {
                //Console.WriteLine("sdf");
                _scale = value;
                string[] coords;
                coords = _scale.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                scalev.x = float.Parse(coords[0], CultureInfo.InvariantCulture.NumberFormat);
                scalev.y = float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat);
                scalev.z = float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat);
            }
        }
        public string Geometry;
        public string Material;
        public string LodRatio;
        public string HiddenInGame;
        public string EntityClass;
        public XElement Points;
        public string DisplayFilled;
        public string Height;
        public XElement Properties;
        public string LightBlending;
        public string LightBlendValue;
        public string GeometryFile;
        public string GeometryContainsPortals;
        public string AttachmentPointName;
        public string AttachmentType;
        public string AttachmentTarget;
        Vector3 posv;
        Vector3 scalev;
        Vector3 visareaposv;
        Vector4 rotv;
        public Matrix3x4 Transform34 { get; set; }
        public string VisAreaPos
        {
            get { return _VisAreaPos; }
            set
            {
                //Console.WriteLine("sdf");
                _VisAreaPos = value;
                string[] coords;
                coords = _VisAreaPos.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                visareaposv.x = float.Parse(coords[0], CultureInfo.InvariantCulture.NumberFormat);
                visareaposv.y = float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat);
                visareaposv.z = float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat);
                if (Name == "Portal001-021") Console.WriteLine(visareaposv.x);
            }
        }
        public string Pos
        {
            get { return _pos; }
            set
            {
                //Console.WriteLine("sdf");
                //char[] allowedChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.', '-', };
                _pos = value;
                string[] coords;
                coords = _pos.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < coords.Length; i++)
                {
                    Regex rgx = new Regex("[^0-9-\\.\\,Ee]");
                    coords[i]=rgx.Replace(coords[i], "");
                }
                posv.x = float.Parse(coords[0], CultureInfo.InvariantCulture.NumberFormat);
                posv.y = float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat);
                posv.z = float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat);
            }
        }
        public void SetPos(Vector3 vec)
        {
            string format = "N6";
            Pos = vec.x.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
                + "," + vec.y.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
                + "," + vec.z.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray());
        }
        public void SetRotate(Vector4 vec)
        {
            string format = "N6";
            Rotate = vec.w.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
                + "," + vec.x.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
                + "," + vec.y.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
                + "," + vec.z.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray());
        }
        public void SetScale(Vector3 vec)
        {
            string format = "N6";
            Scale = vec.x.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
                + "," + vec.y.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
                + "," + vec.z.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray());
        }
        public Vector4 GetRotate()
        {
            return rotv;
        }

        public XElement GetAsGeomEntity(string parentid, string layerid,string entid,string componentGuid,string portName,string layerName,string lodRatio)
        {
            XElement obj1 = new XElement("Object");

            obj1.Add(new XAttribute("Name", Name));
            if(parentid!="")obj1.Add(new XAttribute("LinkedTo", parentid));
            obj1.Add(new XAttribute("Id", entid));
            obj1.Add(new XAttribute("LayerGUID", layerid));
            obj1.Add(new XAttribute("Geometry", Geometry));
            if (portName != "") obj1.Add(new XAttribute("AttachmentType", "CharacterBone"));
            if (portName != "") obj1.Add(new XAttribute("AttachmentTarget", portName));
            obj1.Add(new XAttribute("Layer", layerName));
            if (Pos != null) obj1.Add(new XAttribute("Pos", Pos));
            if (Rotate != null) obj1.Add(new XAttribute("Rotate", Rotate));
            if (Scale != null) obj1.Add(new XAttribute("Scale", Scale));
            obj1.Add(new XAttribute("Type", "GeomEntity"));
            obj1.Add(new XAttribute("EntityClass", "GeomEntity"));
            obj1.Add(new XAttribute("LodRatio", lodRatio));
            obj1.Add(new XAttribute("ViewDistRatio", "100"));
            obj1.Add(new XAttribute("HasEntity", "1"));

            if (Material != null)
            {
                if (Material != "") obj1.Add(new XAttribute("Material", Material));
            }

            //XElement Components = new XElement("Components");
            //XElement Component = new XElement("Component");
            //Component.Add(new XAttribute("typeId", "ec0cd266-a6d1-4774-b499-690bd6fb61ee"));
            //Component.Add(new XAttribute("guid", componentGuid));
            //Component.Add(new XAttribute("CryXmlVersion", "2"));
            //Component.Add(new XAttribute("Geometry", Geometry));
            //Component.Add(new XAttribute("Physicalize", "ePhysicalizationType_Static"));
            //Component.Add(new XAttribute("ReceiveCollisionEvents", "false"));
            //Component.Add(new XAttribute("Mass", "1"));
            //Component.Add(new XAttribute("Animation", ""));
            //Component.Add(new XAttribute("Speed", "1"));
            //Component.Add(new XAttribute("Loop", "true"));
            //Components.Add(Component);
            //obj1.Add(Components);


            //if((Math.Abs(posv.x)>1000)|| (Math.Abs(posv.y) > 1000)|| (Math.Abs(posv.z) > 1000))
            //{
            //    obj1 = new XElement("Object");
            //}

            return obj1;
        }
        public XElement GetAsGeomEntityWithComponent(string parentid, string layerid, string entid, string componentGuid, string portName, string layerName, string lodRatio)
        {
            EntityWithComponent entityObject = new EntityWithComponent();

            entityObject.Name = Name;
            if (parentid != "") entityObject.LinkedTo = parentid;
            entityObject.Id = entid;
            entityObject.LayerGUID = layerid;
            entityObject.Layer = layerName;
            entityObject.LodRatio = lodRatio;
            entityObject.ViewDistRatio = "100";
            if (portName != "") entityObject.AttachmentType = "CharacterBone";
            if (portName != "") entityObject.AttachmentTarget = portName;
            if (Pos != null) entityObject.Pos = Pos;
            if (Rotate != null) entityObject.Rotate = Rotate;
            if (Scale != null) entityObject.Scale = Scale;

            if (Material != null)
            {
                if (Material != "") entityObject.Material = Material;
            }

            //Quaternion quaterion = new Quaternion(Transform34);
            //Angles3 rotation = new Angles3(quaterion);
            //rotation = rotation/ scalev;
            //Vector3 rotationAngles = rotation.Angles();
            //if(Pos.StartsWith("13.49028"))
            //{
            //    string sdfbreak= "break";
            //}
            CStaticMeshComponent meshComponent = new CStaticMeshComponent(componentGuid, Geometry,new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            entityObject.Components.Add(meshComponent);

            return entityObject.Get();
        }
        public XElement GetAsFlowgraphEntity(string parentid, string layerid,string layerName,string portName)
        {
            XElement obj1 = new XElement("Object");

            obj1.Add(new XAttribute("Name", Name));
            if (parentid != "") obj1.Add(new XAttribute("LinkedTo", parentid));
            obj1.Add(new XAttribute("Id", Id));
            obj1.Add(new XAttribute("LayerGUID", layerid));
            obj1.Add(new XAttribute("Layer", layerName));
            if (Pos != null)
            {
                obj1.Add(new XAttribute("Pos", Pos));
            }
            else
            {
                obj1.Add(new XAttribute("Pos", "0,0,0"));
            }
            if (Rotate != null) obj1.Add(new XAttribute("Rotate", Rotate));
            obj1.Add(new XAttribute("Type", "Entity"));
            obj1.Add(new XAttribute("EntityClass", "FlowgraphEntity"));
            if (portName != "") obj1.Add(new XAttribute("AttachmentType", "CharacterBone"));
            if (portName != "") obj1.Add(new XAttribute("AttachmentTarget", portName));
            return obj1;
        }
        public XElement GetAsLight(int lightID, string layerid, string layerName)
        {
            XElement obj1 = new XElement("Object");

            obj1.Add(new XAttribute("Name", lights[lightID].Name + "-Light" + lightID));
            if (Id != "")obj1.Add(new XAttribute("LinkedTo", Id));
            obj1.Add(new XAttribute("Id", lights[lightID].Id));
            obj1.Add(new XAttribute("LayerGUID", layerid));
            obj1.Add(new XAttribute("Layer", layerName));
            if (lights[lightID].translation != null)
            {
                obj1.Add(new XAttribute("Pos", lights[lightID].translation));
            }
            else
            {
                obj1.Add(new XAttribute("Pos", "0,0,0"));
            }
            if (lights[lightID].rotation != null) obj1.Add(new XAttribute("Rotate", lights[lightID].rotation));
            obj1.Add(new XAttribute("Type", "Entity"));
            obj1.Add(new XAttribute("EntityClass", "Light"));
            obj1.Add(new XAttribute("ColorRGB", "65535"));
            obj1.Add(lights[lightID].Properties);

            return obj1;
        }
        public XElement GetAsEnvironmentLight(int lightID, string layerid, string layerName)
        {
            XElement obj1 = new XElement("Object");

            obj1.Add(new XAttribute("Name", lights[lightID].Name + "-Probe" + lightID));
            if (Id != "") obj1.Add(new XAttribute("LinkedTo", Id));
            obj1.Add(new XAttribute("Id", lights[lightID].Id));
            obj1.Add(new XAttribute("LayerGUID", layerid));
            obj1.Add(new XAttribute("Layer", layerName));
            if (lights[lightID].translation != null)
            {
                obj1.Add(new XAttribute("Pos", lights[lightID].translation));
            }
            else
            {
                obj1.Add(new XAttribute("Pos", "0,0,0"));
            }
            if (lights[lightID].rotation != null) obj1.Add(new XAttribute("Rotate", lights[lightID].rotation));
            obj1.Add(new XAttribute("Type", "Entity"));
            obj1.Add(new XAttribute("EntityClass", "EnvironmentLight"));
            obj1.Add(new XAttribute("ColorRGB", "65535"));
            obj1.Add(lights[lightID].Properties);

            return obj1;
        }
        public XElement GetAsVisArea(string entID, string layerid, string layerName)
        { 
            XElement elObject = new XElement("Object");

            elObject.Add(new XAttribute("Name", Name));
            elObject.Add(new XAttribute("Id", entID));
            elObject.Add(new XAttribute("LayerGUID", layerid));
            elObject.Add(new XAttribute("Layer", layerName));
            elObject.Add(new XAttribute("Type", "VisArea"));
            elObject.Add(new XAttribute("Pos", Pos));
            elObject.Add(new XAttribute("DisplayFilled", DisplayFilled));
            elObject.Add(new XAttribute("Height", Height));
            //if (Rotate != null) elObject.Add(new XAttribute("Rotate", Rotate));
            if (Rotate != null) elObject.Add(new XAttribute("Rotate", "1,0,0,0"));
            if (Scale != null) elObject.Add(new XAttribute("Scale", Scale));
            elObject.Add(Points); 

            return elObject;
        }
        public XElement GetAsPortal(string entID, string layerid, string layerName)
        {
            XElement elObject = new XElement("Object");

            elObject.Add(new XAttribute("Name", Name));
            elObject.Add(new XAttribute("Id", entID));
            elObject.Add(new XAttribute("LayerGUID", layerid));
            elObject.Add(new XAttribute("Layer", layerName));
            elObject.Add(new XAttribute("Type", "Portal"));
            elObject.Add(new XAttribute("Pos", Pos));
            elObject.Add(new XAttribute("DisplayFilled", DisplayFilled));
            elObject.Add(new XAttribute("Height", Height));
            elObject.Add(new XAttribute("LightBlending", LightBlending));
            elObject.Add(new XAttribute("LightBlendValue", LightBlendValue));
            //if (Rotate != null) elObject.Add(new XAttribute("Rotate", Rotate));
            if (Rotate != null) elObject.Add(new XAttribute("Rotate", "1,0,0,0"));
            if (Scale != null) elObject.Add(new XAttribute("Scale", Scale));
            elObject.Add(Points);

            return elObject;
        }
        public XElement GetAsEnvironmentLight2(string entID, string layerid, string layerName)
        {
            XElement elObject = new XElement("Object");
            elObject.Add(new XAttribute("Name", Name+"_probe"));
            if (Id != "") elObject.Add(new XAttribute("LinkedTo", Id));
            elObject.Add(new XAttribute("Id", entID));
            elObject.Add(new XAttribute("LayerGUID", layerid));
            elObject.Add(new XAttribute("Layer", layerName));
            elObject.Add(new XAttribute("Type", "Entity"));
            elObject.Add(new XAttribute("EntityClass", "EnvironmentLight"));
            elObject.Add(new XAttribute("Pos", "0,0,0"));
            if (Rotate != null) elObject.Add(new XAttribute("Rotate", "1,0,0,0"));
            if (Scale != null) elObject.Add(new XAttribute("Scale", "1,1,1"));
            elObject.Add(Properties);

            return elObject;
        }
        public void FixPoints()
        {
            //XElement newel = new XElement("Points");
            //List<Point> points = new List<Point>(); 
            //foreach (XElement el in Points.Elements("Point"))
            //{
            //    points.Add(new Point(el.Attribute("Pos").Value));
            //}
            //Point ptt = points[0];
            //ptt = new Point((ptt.x - visareaposv.x ) + "," + (ptt.y - visareaposv.y ) + "," + (ptt.z - visareaposv.z ));
            //foreach (Point pt in points)
            //{
            //    XElement el = new XElement("Point");
            //    //string newpos = (pt.x - posv.x) + "," + (pt.y - posv.y) + "," + (pt.z - posv.z);
            //    //string newpos = (posv.x - pt.x) + "," + (posv.y - pt.y) + "," + (posv.z - pt.z);
            //    //string newpos = (pt.x - ptt.x) + "," + (pt.y - ptt.y) + "," + (pt.z - ptt.z);
            //    //string newpos = (-pt.x) + "," + (-pt.y) + "," + (-pt.z);
            //    string newpos = (pt.x - visareaposv.x) + "," + (pt.y - visareaposv.y) + "," + (pt.z - visareaposv.z);
            //    //string newpos = (pt.x - visareaposv.x - ptt.x) + "," + (pt.y - visareaposv.y - ptt.y) + "," + (pt.z - visareaposv.z - ptt.z);
            //    //string newpos = (visareaposv.x - pt.x) + "," + (visareaposv.y - pt.y) + "," + (visareaposv.z - pt.z);
            //    el.Add(new XAttribute("Pos", newpos));
            //    newel.Add(el);
            //}
            //Points = newel;
            //// Pos = (-posv.x) + "," + (-posv.y) + "," + (-posv.z);
            ////Rotate = (-rotv.x) + "," + (-rotv.y) + "," + (-rotv.z) + "," + (-rotv.w); 
            ////Pos = (ptt.x - posv.x) + "," + (ptt.y - posv.y) + "," + (ptt.z - posv.z); 
            ////Pos = (posv.x - ptt.x) + "," + (posv.y - ptt.y) + "," + (posv.z - ptt.z);
            ////Pos = (points[0].x - points[1].x) + "," + (posv.z) + "," + "0";
            ////Pos = (posv.x - visareaposv.x) + "," + (posv.y - visareaposv.y) + "," + (posv.z - visareaposv.z);
            //Pos = "2.25,0,-1.625";
            // Rotate = "1,0,0,0";
        }

        //public void  SetPos(Vector3) {_pos = }
        //public void SetPos() {
        //    get { return _pos.x+"," + _pos.x + "," + _pos.x ; }
        //    set { _pos = value; }
        //}
    }
}
