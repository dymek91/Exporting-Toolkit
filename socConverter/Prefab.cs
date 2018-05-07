using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipsExporter;
using System.Xml.Linq;
using CryEngine;
using System.IO;

namespace socConverter
{
    class Prefab
    {
        ObjectContainer objectContainer;
        XDocument xmlDoc;
        string xml;

        string lodRatio = "100";


        public Prefab(ObjectContainer soc)
        {
            objectContainer = soc;
            MakePrefab();
        }
        public XDocument GetXDocument()
        {
            return xmlDoc;
        }
        public string GetXml()
        {
            return xml;
        }
        void MakePrefab()
        {
            XDocument doc = new XDocument();

            string prefLibName = objectContainer.SocpakPath;
            prefLibName=prefLibName.TrimStart("./data/".ToCharArray());
            prefLibName=prefLibName.Replace("\\","/");
            prefLibName = prefLibName.TrimEnd(".socpak".ToCharArray());
            prefLibName = prefLibName.TrimEnd(".soc".ToCharArray());

            //./data/ObjectContainers\AC\Area18\area18.socpak
            //prefLibName = ship.GetVehicleType() + "/" + ship.GetManufacturer() + "/" + prefLibName;
            XElement PrefabsLibrary = new XElement("PrefabsLibrary");
            PrefabsLibrary.Add(new XAttribute("Name", prefLibName));

            string prefabName = Path.GetFileNameWithoutExtension(objectContainer.SocpakPath);
            string prefabId = GuidUtility.GenPrefabID(prefLibName);
            XElement Prefab = new XElement("Prefab");
            Prefab.Add(new XAttribute("Name", prefabName));
            Prefab.Add(new XAttribute("Id", prefabId)); 
            Prefab.Add(new XAttribute("Library", prefLibName));

            XElement Objects = new XElement("Objects");

            XElement Object = new XElement("Object");
            string partid = GuidUtility.GenID();
            string layerid = GuidUtility.GenID();
            string parentid = "";
            //parentid = partid;
            Object = AddObjectsFromObjectContainers(Object, objectContainer, parentid, layerid);

            Objects.Add(Object);

            XElement ObjectsNew = new XElement("Objects");
            foreach (XElement obj in Objects.Descendants("Object"))
            {
                XElement ObjectNew = new XElement("Object");
                //ObjectNew.Add(new XElement("Properties"));
                if (obj.Element("Properties") != null) ObjectNew.Add(obj.Element("Properties"));
                foreach (XAttribute atr in obj.Attributes())
                {
                    ObjectNew.Add(atr);
                }
                if (obj.Element("Components") != null)
                    ObjectNew.Add(obj.Element("Components"));

                if (obj.Element("Points") != null)
                    ObjectNew.Add(obj.Element("Points"));

                if (ObjectNew.Attribute("Name") != null)
                    ObjectsNew.Add(ObjectNew);
            }

            Prefab.Add(ObjectsNew);
            PrefabsLibrary.Add(Prefab);
            doc.Add(PrefabsLibrary);

            xmlDoc = doc;
            xml = doc.ToString();
        }

        XElement AddObjectsFromObjectContainers(XElement el, ObjectContainer objCon, string parentid, string layerid)
        {
            XElement elObject = el;
            layerid = GuidUtility.GenLayerID("Main");
            string layerName = "Main";
            List<ObjectContainer> objContainers = new List<ObjectContainer>();
            objContainers.Add(objCon);
            foreach (Child child in objCon.childs)
            {
                if (!child.IsExternal())
                {
                    objContainers.Add(child.Soc);
                }
            }
            foreach (ObjectContainer oc in objContainers)
            {
                string portName = oc.portName;
                foreach (PrefabObject po in oc.prefabObjects)
                {
                    if (po.Geometry != null)
                    {
                        layerid = GuidUtility.GenLayerID("Main");
                        layerName = "Main";
                        //string layerName = oc.name; 
                        //string entid = GuidUtility.GenID("ent" + parentid +oc.name+po.Name+ po.Geometry);
                        //string compGuid = GuidUtility.GenID("entcomp" + parentid + oc.name + po.Name + po.Geometry);
                        string entid = GuidUtility.GenID();
                        string componentGuid = GuidUtility.GenID();
                        //string prefid = GuidUtility.GenID("entpref" + parentid + oc.name + po.Name+ po.Geometry);

                        XElement obj1 = po.GetAsEntityWithComponent(parentid, layerid, entid, componentGuid, portName, layerName, lodRatio);

                        elObject.Add(obj1);

                        foreach (PrefabObject childPo in po.attachments)
                        {
                            string childentid = GuidUtility.GenID();
                            string childcomponentGuid = GuidUtility.GenID();

                            XElement childObj = childPo.GetAsEntityWithComponent(entid, layerid, childentid, childcomponentGuid, childPo.AttachmentTarget, layerName, lodRatio);

                            elObject.Add(childObj);
                        }
                    }
                    if (po.EntityClass == "AnimObject")
                    {
                        layerid = GuidUtility.GenLayerID("Main");
                        layerName = "Main"; 
                        string entid = GuidUtility.GenID();
                        string componentGuid = GuidUtility.GenID();

                        XElement obj1 = po.GetAsAnimObject(parentid, layerid, entid, componentGuid, portName, layerName, lodRatio);

                        elObject.Add(obj1);
                    }

                    if (po.EntityClass == "LightGroup")
                    {
                        layerid = GuidUtility.GenLayerID("Main");
                        layerName = "Main";
                        XElement obj1 = po.GetAsFlowgraphEntity(parentid, layerid, layerName, oc.portName);

                        elObject.Add(obj1);

                        int i = 0;
                        //lights and probes (enveroinmentlight)
                        foreach (Light lt in po.lights)
                        {
                            if (lt.EntityClass == "Light")
                            {
                                obj1 = po.GetAsLight(i, layerid, layerName);

                                elObject.Add(obj1);
                            }
                            if (lt.EntityClass == "EnvironmentLight")
                            {
                                obj1 = po.GetAsEnvironmentLight(i, layerid, layerName);

                                elObject.Add(obj1);
                            }
                            i++;
                        }
                    }
                    //if (po.Type == "VisArea")
                    //{
                    //    layerid = GuidUtility.GenLayerID("Main");
                    //    layerName = "Main";
                    //    string entid = GuidUtility.GenID();
                    //    XElement obj1 = po.GetAsVisArea(entid, layerid, layerName);

                    //    Object.Add(obj1);
                    //}
                    //if (po.Type == "Portal")
                    //{
                    //    layerid = GuidUtility.GenLayerID("Main");
                    //    layerName = "Main";
                    //    string entid = GuidUtility.GenID();
                    //    XElement obj1 = po.GetAsPortal(entid, layerid, layerName);

                    //    Object.Add(obj1);
                    //}
                    if (po.Type == "EnvironmentLight")
                    {
                        layerid = GuidUtility.GenLayerID("Main");
                        layerName = "Main";
                        XElement obj1 = po.GetAsFlowgraphEntity(parentid, layerid, layerName, oc.portName);

                        elObject.Add(obj1);

                        layerid = GuidUtility.GenLayerID("Main");
                        layerName = "Main";
                        string entid = GuidUtility.GenID();
                        obj1 = po.GetAsEnvironmentLight2(entid, layerid, layerName);

                        elObject.Add(obj1);
                    }
                    if (po.EntityClass == "Light")
                    {
                        string layername = oc.name;
                        XElement obj1 = new XElement("Object");
                        string entid = GuidUtility.GenID();
                        string prefid = GuidUtility.GenID();

                        obj1.Add(new XAttribute("Name", po.Name));
                        if (parentid != "") obj1.Add(new XAttribute("LinkedTo", parentid));
                        obj1.Add(new XAttribute("Id", entid));
                        obj1.Add(new XAttribute("LayerGUID", layerid));
                        obj1.Add(new XAttribute("Layer", "Main"));
                        if (po.Pos != null)
                        {
                            obj1.Add(new XAttribute("Pos", po.Pos));
                        }
                        else
                        {
                            obj1.Add(new XAttribute("Pos", "0,0,0"));
                        }
                        if (po.Rotate != null) obj1.Add(new XAttribute("Rotate", po.Rotate));
                        obj1.Add(new XAttribute("Type", "Entity"));
                        obj1.Add(new XAttribute("EntityClass", "Light"));
                        obj1.Add(new XAttribute("ColorRGB", "65535"));
                        obj1.Add(po.Properties);

                        elObject.Add(obj1);
                    }
                }

                //foreach (Part part in ship.parts)
                //{
                //    if (part.attachmentPoints != null)
                //    {
                //        List<PrefabObject> prefObjects = oc.GetPrefabObjectsByAttachmentName(p);
                //    }
                //}
            }
            foreach (Child child in objCon.childs)
            {
                if (child.IsExternal())
                {
                    layerid = GuidUtility.GenLayerID("Main");
                    layerName = "Main";
                    XElement obj1root = child.GetAsFlowgraphEntity(parentid, layerid, layerName, "");
                    elObject.Add(obj1root);

                    elObject = AddObjectsFromObjectContainers(elObject, child.Soc, child.Guid, layerid);
                }
            }

            return elObject;
        }
    }
}
