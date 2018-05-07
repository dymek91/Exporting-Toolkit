using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using CryEngine;

namespace shipsExporter
{
    class GenerateOptions
    {
        public string shipName;
        public string modificationName;
        public string paintName;
        public string loadoutName;
    }
    class EntityPrefab_CE54_Modded
    {
        ShipsExporter exporter;
        private static Random random = new Random();
        int i = 0;
        string[] allowedItemClasses = { "VehicleWeapon", "VehicleTurret", "VehicleItemThruster", "VehicleMissileRack", "VehicleItemSeat", "MissingItem", "SCItem" };
        string prefablib = "";
        List<XElement> prefabs = new List<XElement>();
        string lodRatio = "100";

        public EntityPrefab_CE54_Modded(ShipsExporter exp)
        {
            exporter = exp;
        }
        public string GetPrefabGUID(string PrefabLibrary, string PrefabName)
        {
            string guid = "";
            XDocument xml = XDocument.Load(PrefabLibrary);
            foreach (XElement el in xml.Descendants("Prefab"))
            {
                if (el.Attribute("Name").Value == PrefabName) guid = el.Attribute("Id").Value;
            }
            return guid;
        }
        public XElement GetPrefab(string PrefabLibrary, string PrefabName)
        {
            XElement obj = new XElement("Prefab");
            XDocument xml = XDocument.Load(PrefabLibrary);
            foreach (XElement el in xml.Descendants("Prefab"))
            {
                if (el.Attribute("Name").Value == PrefabName) obj = el;
            }
            return obj;
        }
        public XElement AddObjects(XElement el, Part part, Loadout loadout, string parentid, string partid, string layerid, Paint paint=null)
        {
            XElement Object = el;

            if (part.partClass == "AnimatedJoint")
            {
                partid = parentid;
                Object.Name = "Node";
                Object.Add(new XAttribute("Name", part.name));
                Object.Add(new XAttribute("LinkedTo", parentid));
                Object.Add(new XAttribute("Id", partid));
            }

            if (part.partClass == "Animated" || part.partClass== "SubPartWheel")
            {
                if(i==0)
                {
                    if (paint != null)
                    {
                        Object.Add(new XAttribute("Material", paint.material));
                    }
                }
                if (i != 0)
                {
                    Object.Add(new XAttribute("LinkedTo", parentid));
                    
                }
                Object.Add(new XAttribute("Name", part.name));
                Object.Add(new XAttribute("Id", partid));
                Object.Add(new XAttribute("LayerGUID", layerid));
                if (part.animatedFilename != null) { Object.Add(new XAttribute("Geometry", part.animatedFilename)); } else { Object.Add(new XAttribute("Geometry", "")); }
                Object.Add(new XAttribute("Layer", "Main"));
                Object.Add(new XAttribute("Type", "GeomEntity"));
                Object.Add(new XAttribute("EntityClass", "GeomEntity"));
                Object.Add(new XAttribute("LodRatio", lodRatio));
                if (part.partClass == "SubPartWheel")
                {
                    Object.Add(new XAttribute("AttachmentType", "CharacterBone"));
                    Object.Add(new XAttribute("AttachmentTarget", part.name));
                }
                else
                {
                    if (part.helper != "") Object.Add(new XAttribute("AttachmentType", "CharacterBone"));
                    if (part.helper != "") Object.Add(new XAttribute("AttachmentTarget", part.helper));
                }
                

                XElement Components = new XElement("Components");
                XElement Component = new XElement("Component");
                Component.Add(new XAttribute("typeId", "ec0cd266-a6d1-4774-b499-690bd6fb61ee"));
                // Component.Add(new XAttribute("guid", GuidUtility.GenID(part.name+loadout.name+part.animatedFilename)));
                Component.Add(new XAttribute("guid", GuidUtility.GenID()));
                Component.Add(new XAttribute("CryXmlVersion", "2"));
                if (part.animatedFilename != null) { Component.Add(new XAttribute("Geometry", part.animatedFilename)); } else { Component.Add(new XAttribute("Geometry", "")); }
                Component.Add(new XAttribute("Physicalize", "ePhysicalizationType_Static"));
                Component.Add(new XAttribute("ReceiveCollisionEvents", "false"));
                Component.Add(new XAttribute("Mass", "1"));
                Component.Add(new XAttribute("Animation", ""));
                Component.Add(new XAttribute("Speed", "1"));
                Component.Add(new XAttribute("Loop", "true"));
                Components.Add(Component);
                Object.Add(Components);
                i++;
            }
            if (part.partClass == "ItemPort")
            {
                ItemPort itemport = loadout.GetItemPort(part.name);
                Item item = exporter.GetItem(itemport.itemName,loadout.name);
                if (item.geometry != "" && item.itemclass != "VehicleItemMultiLight" && allowedItemClasses.Any(item.itemclass.Contains))
                {
                    Object.Add(new XAttribute("Name", part.name));
                    Object.Add(new XAttribute("LinkedTo", parentid));
                    Object.Add(new XAttribute("Id", partid));
                    Object.Add(new XAttribute("LayerGUID", layerid));
                    Object.Add(new XAttribute("Geometry", item.geometry));
                    Object.Add(new XAttribute("AttachmentType", "CharacterBone"));
                    Object.Add(new XAttribute("AttachmentTarget", itemport.portName));
                    Object.Add(new XAttribute("Layer", "Main"));
                    Object.Add(new XAttribute("Type", "GeomEntity"));
                    Object.Add(new XAttribute("EntityClass", "GeomEntity"));
                    Object.Add(new XAttribute("LodRatio", lodRatio));

                    XElement Components = new XElement("Components");
                    XElement Component = new XElement("Component");
                    Component.Add(new XAttribute("typeId", "ec0cd266-a6d1-4774-b499-690bd6fb61ee"));
                    //Component.Add(new XAttribute("guid", GuidUtility.GenID(part.name + loadout.name + part.animatedFilename+ itemport.portName)));
                    Component.Add(new XAttribute("guid", GuidUtility.GenID()));
                    Component.Add(new XAttribute("CryXmlVersion", "2"));
                    Component.Add(new XAttribute("Geometry", item.geometry));
                    Component.Add(new XAttribute("Physicalize", "ePhysicalizationType_Static"));
                    Component.Add(new XAttribute("ReceiveCollisionEvents", "false"));
                    Component.Add(new XAttribute("Mass", "1"));
                    Component.Add(new XAttribute("Animation", ""));
                    Component.Add(new XAttribute("Speed", "1"));
                    Component.Add(new XAttribute("Loop", "true"));
                    Components.Add(Component);
                    Object.Add(Components);

                    foreach (ItemPort ip in itemport.itemPorts)
                    {
                        Item item2 = exporter.GetItem(ip.itemName,loadout.name);
                        XElement obj2 = new XElement("Object");
                        obj2.Add(new XAttribute("Name", ip.portName));
                        obj2.Add(new XAttribute("LinkedTo", partid));
                        //obj2.Add(new XAttribute("Id", GuidUtility.GenID("skjdfu"+part.name + loadout.name + part.animatedFilename + ip.portName+item2.name)));
                        obj2.Add(new XAttribute("Id", GuidUtility.GenID()));
                        obj2.Add(new XAttribute("LayerGUID", layerid));
                        obj2.Add(new XAttribute("Geometry", item2.geometry));
                        obj2.Add(new XAttribute("AttachmentType", "CharacterBone"));
                        obj2.Add(new XAttribute("AttachmentTarget", ip.portName));
                        obj2.Add(new XAttribute("Layer", "Main"));
                        obj2.Add(new XAttribute("Type", "GeomEntity"));
                        obj2.Add(new XAttribute("EntityClass", "GeomEntity"));
                        obj2.Add(new XAttribute("LodRatio", lodRatio));

                        XElement Components2 = new XElement("Components");
                        XElement Component2 = new XElement("Component");
                        Component2.Add(new XAttribute("typeId", "ec0cd266-a6d1-4774-b499-690bd6fb61ee"));
                        //Component2.Add(new XAttribute("guid", GuidUtility.GenID(part.name + loadout.name + part.animatedFilename + ip.portName)));
                        Component2.Add(new XAttribute("guid", GuidUtility.GenID()));
                        Component2.Add(new XAttribute("CryXmlVersion", "2"));
                        Component2.Add(new XAttribute("Geometry", item2.geometry));
                        Component2.Add(new XAttribute("Physicalize", "ePhysicalizationType_Static"));
                        Component2.Add(new XAttribute("ReceiveCollisionEvents", "false"));
                        Component2.Add(new XAttribute("Mass", "1"));
                        Component2.Add(new XAttribute("Animation", ""));
                        Component2.Add(new XAttribute("Speed", "1"));
                        Component2.Add(new XAttribute("Loop", "true"));
                        Components2.Add(Component2);
                        obj2.Add(Components2);

                        Object.Add(obj2);
                    }
                }
                if (item.itemclass == "VehicleItemMultiLight")
                {
                    foreach (AttachmentPoint at in part.attachmentPoints)
                    {
                        //Console.WriteLine(at.name);
                        XElement obj1 = new XElement("Object");
                        string entid = GuidUtility.GenID(at.name+at.bone+part.name+loadout.name);
                        obj1.Add(new XAttribute("Type", "GeomEntity"));
                        obj1.Add(new XAttribute("Name", at.bone));
                        obj1.Add(new XAttribute("LinkedTo", parentid));
                        obj1.Add(new XAttribute("Id", entid));
                        obj1.Add(new XAttribute("LayerGUID", layerid));
                        obj1.Add(new XAttribute("AttachmentType", "CharacterBone"));
                        obj1.Add(new XAttribute("AttachmentTarget", at.bone));
                        obj1.Add(new XAttribute("Layer", "Main"));
                        obj1.Add(new XAttribute("EntityClass", "GeomEntity"));
                        Object.Add(obj1);

                        //PrefabAttachment prefabAtt = item.getAttachment(at.name);
                        //XElement obj2 = getPrefab(prefabAtt.prefabLibrary, prefabAtt.prefabName);
                        XElement obj2 = new XElement("Object");
                        PrefabAttachment prefabAtt = item.getAttachment(at.name);
                        string PrefabGUID = "";
                        try
                        {
                            PrefabGUID = GetPrefabGUID(prefabAtt.prefabLibrary, prefabAtt.prefabName);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error:");
                            Console.WriteLine("prefabLibrary: {0} prefabName: {1} attachmentPoint: {2}", prefabAtt.prefabLibrary, prefabAtt.prefabName, prefabAtt.attachmentPoint);
                            Console.WriteLine("at.name: {0}", at.name);
                            Console.WriteLine("item.name: {0}", item.name);
                            //throw e;
                        }
                        string prefid = GuidUtility.GenID(at.name + at.bone + part.name + loadout.name+"prefab");
                        string prefablibname = Path.GetFileNameWithoutExtension(prefabAtt.prefabLibrary);
                        obj2.Add(new XAttribute("Type", "Prefab"));
                        obj2.Add(new XAttribute("Name", at.name));
                        obj2.Add(new XAttribute("LinkedTo", entid));
                        obj2.Add(new XAttribute("Id", prefid));
                        obj2.Add(new XAttribute("LayerGUID", layerid));
                        obj2.Add(new XAttribute("Layer", "Main"));
                        obj2.Add(new XAttribute("PrefabGUID", PrefabGUID));
                        obj2.Add(new XAttribute("PrefabLibrary", prefablibname));
                        obj2.Add(new XAttribute("PrefabName", prefabAtt.prefabName));
                        prefablib = prefabAtt.prefabLibrary;
                        prefabs.Add(GetPrefab(prefabAtt.prefabLibrary, prefabAtt.prefabName));
                        Object.Add(obj2);
                    }
                }

            }

            foreach (Part pr in part.parts)
            {
                XElement obj = new XElement("Object");
                if (pr.partClass == "ItemPort")
                {
                    ItemPort itemport = loadout.GetItemPort(part.name);
                    if (itemport.itemName != "")
                    { 
                        obj = AddObjects(obj, pr, loadout, partid, GuidUtility.GenID(pr.name+itemport.itemName+loadout.name + "kjhkh" + pr.animatedFilename), layerid);
                    }
                    else
                    {
                        obj = AddObjects(obj, pr, loadout, parentid, GuidUtility.GenID(pr.name + itemport.itemName + loadout.name + "5tyht" + pr.animatedFilename), layerid);
                    }
                }
                else
                {
                    obj = AddObjects(obj, pr, loadout, partid, GuidUtility.GenID(pr.name  + loadout.name+"asddf"+pr.animatedFilename), layerid);
                }
                //if (obj.Attribute("Name") != null)
                Object.Add(obj);
            }

            return Object;
        }
        XElement AddPrefabsFromObjectContainers(XElement el, ShipImplementation ship, string parentid, string layerid)
        {
            XElement Object = el;
            layerid = GuidUtility.GenLayerID("Main");
            foreach (ObjectContainer oc in ship.objectContainers)
            {
                string layername = oc.name;
                //layerid = stringToNumbers(layername);
                //Console.WriteLine(at.name);
                XElement obj1 = new XElement("Object");
                string entid = GuidUtility.GenID("ent"+ship.name+oc.name+parentid);
                string prefid = GuidUtility.GenID("entpref" + ship.name + oc.name + parentid);
                obj1.Add(new XAttribute("Type", "Entity"));
                obj1.Add(new XAttribute("Name", oc.portName));
                obj1.Add(new XAttribute("LinkedTo", parentid));
                obj1.Add(new XAttribute("Id", entid));
                obj1.Add(new XAttribute("LayerGUID", layerid));
                obj1.Add(new XAttribute("AttachmentType", "CharacterBone"));
                obj1.Add(new XAttribute("AttachmentTarget", oc.portName));
                obj1.Add(new XAttribute("Layer", layername));
                obj1.Add(new XAttribute("EntityClass", "FlowgraphEntity"));
                Object.Add(obj1);

                //PrefabAttachment prefabAtt = item.getAttachment(at.name);
                //XElement obj2 = getPrefab(prefabAtt.prefabLibrary, prefabAtt.prefabName);
                XElement obj2 = new XElement("Object");
                string id = GuidUtility.GenID("entid" + ship.name + oc.name + parentid);
                string prefablibname = ship.name;
                obj2.Add(new XAttribute("Type", "Prefab"));
                obj2.Add(new XAttribute("Name", oc.name));
                obj2.Add(new XAttribute("LinkedTo", entid));
                obj2.Add(new XAttribute("Id", id));
                obj2.Add(new XAttribute("LayerGUID", layerid));
                obj2.Add(new XAttribute("Layer", layername));
                obj2.Add(new XAttribute("PrefabGUID", prefid));
                obj2.Add(new XAttribute("PrefabLibrary", prefablibname));
                obj2.Add(new XAttribute("PrefabName", "ObjectContainers." + oc.name));
                //prefablib = prefabAtt.prefabLibrary;
                prefabs.Add(oc.GetAsPrefab(prefablibname, layerid, prefid, layername));
                Object.Add(obj2);

                foreach (Child child in oc.childs)
                {
                    ObjectContainer ocChild = child.Soc;
                    // Console.WriteLine("ading child {0}", ocChild.name);
                    prefid = GuidUtility.GenID("entprefchild" + ship.name + ocChild.name + parentid);
                    XElement obj3 = new XElement("Object");
                    string id2 = GuidUtility.GenID("entprefchild2sdfsd" + ship.name + ocChild.name + parentid);
                    string prefablibname2 = ship.name;
                    obj3.Add(new XAttribute("Type", "Prefab"));
                    obj3.Add(new XAttribute("Name", ocChild.name));
                    obj3.Add(new XAttribute("LinkedTo", entid));
                    obj3.Add(new XAttribute("Id", id2));
                    obj3.Add(new XAttribute("LayerGUID", layerid));
                    obj3.Add(new XAttribute("Layer", layername));
                    obj3.Add(new XAttribute("PrefabGUID", prefid));
                    obj3.Add(new XAttribute("PrefabLibrary", prefablibname2));
                    obj3.Add(new XAttribute("PrefabName", "ObjectContainers." + ocChild.name));
                    //prefablib = prefabAtt.prefabLibrary;
                    prefabs.Add(ocChild.GetAsPrefab(prefablibname2, layerid, prefid, layername));
                    Object.Add(obj3);
                }
               
            }
            return Object;
        }
        XElement AddObjectsFromObjectContainers(XElement el, List<ObjectContainer> objCons, string parentid, string layerid)
        {
            XElement Object = el;
            layerid = GuidUtility.GenLayerID("Main");
            string layerName = "Main"; 
            List<ObjectContainer> objContainers = new List<ObjectContainer>();
            foreach (ObjectContainer oc in objCons)
            {
                objContainers.Add(oc);
                foreach (Child child in oc.childs)
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

                        XElement obj1 = po.GetAsGeomEntity(parentid, layerid, entid, componentGuid, portName, layerName, lodRatio);
                         
                        Object.Add(obj1);

                        foreach(PrefabObject childPo in po.attachments)
                        {
                            string childentid = GuidUtility.GenID();
                            string childcomponentGuid = GuidUtility.GenID();

                            XElement childObj = childPo.GetAsGeomEntity(entid, layerid, childentid, childcomponentGuid, childPo.AttachmentTarget, layerName, lodRatio);

                            Object.Add(childObj);
                        }
                    }
                    if (po.EntityClass == "AnimObject")
                    {
                        layerid = GuidUtility.GenLayerID("Main");
                        layerName = "Main";
                        string entid = GuidUtility.GenID();
                        string componentGuid = GuidUtility.GenID();

                        XElement obj1 = po.GetAsAnimObject(parentid, layerid, entid, componentGuid, portName, layerName, lodRatio);

                        Object.Add(obj1);
                    }
                    if (po.EntityClass == "LightGroup")
                    {
                        layerid = GuidUtility.GenLayerID("Main");
                        layerName = "Main";
                        XElement obj1 = po.GetAsFlowgraphEntity(parentid, layerid, layerName, oc.portName);
                         
                        Object.Add(obj1);

                        int i = 0;
                        //lights and probes (enveroinmentlight)
                        foreach (Light lt in po.lights)
                        {
                            if (lt.EntityClass== "Light")
                            {
                                obj1 = po.GetAsLight(i, layerid, layerName);

                                Object.Add(obj1);
                            }
                            if (lt.EntityClass == "EnvironmentLight")
                            {
                                obj1 = po.GetAsEnvironmentLight(i, layerid, layerName);

                                Object.Add(obj1);
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

                        Object.Add(obj1);

                        layerid = GuidUtility.GenLayerID("Main");
                        layerName = "Main";
                        string entid = GuidUtility.GenID();
                        obj1 = po.GetAsEnvironmentLight2( entid, layerid, layerName);

                        Object.Add(obj1);
                    }
                    if (po.EntityClass == "Light")
                    {
                        layerid = GuidUtility.GenLayerID("Main");
                        layerName = "Main";
                        XElement obj1 = new XElement("Object");
                        string entid = GuidUtility.GenID();
                        string prefid = GuidUtility.GenID();

                        obj1.Add(new XAttribute("Name", po.Name));
                        obj1.Add(new XAttribute("LinkedTo", parentid));
                        obj1.Add(new XAttribute("Id", entid));
                        obj1.Add(new XAttribute("LayerGUID", layerid));
                        obj1.Add(new XAttribute("Layer", layerName));
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

                        Object.Add(obj1);
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

            return Object;
        }
        public List<AttachmentPoint> GetAllAttachmentPoints(ShipImplementation shipp)
        {
            List<AttachmentPoint> attPoints = new List<AttachmentPoint>();
            //foreach (shipp.)
            //{
            //    attPoints.Add();
            //}
            return attPoints;
        }
        public XDocument GenerateShipModification(GenerateOptions genOptions)
        {
            string shipName = genOptions.shipName;
            string modificationName = genOptions.modificationName; 

            XDocument xml = new XDocument();

            ShipImplementation ship = exporter.GetShipImplementation(shipName);
            Paint paint = ship.GetPaintByName(genOptions.paintName);
            Modification shipModification = ship.GetModificationByName(modificationName);
            string modificationTrueName = shipModification.trueName;
            string loadoutName = null;
            if(genOptions.loadoutName!=null)
            {
                loadoutName = genOptions.loadoutName;
                
            }
            else
            {
                loadoutName = "Default_Loadout_" + modificationTrueName;
            }
            Loadout loadout = exporter.GetLoadout(loadoutName);
            Console.WriteLine(loadout.name);
             
            string prefLibName = "";
            if (paint!=null)
            {
                prefLibName= modificationTrueName+"-"+paint.name;
            }
            else
            {
                prefLibName= modificationTrueName;
            }
            prefLibName = "ships/Variants/" + prefLibName;
            XElement PrefabsLibrary = new XElement("PrefabsLibrary");
            PrefabsLibrary.Add(new XAttribute("Name", prefLibName));
            XElement Prefab = new XElement("Prefab");
            string prefabId = GuidUtility.GenPrefabID(prefLibName); 
            Prefab.Add(new XAttribute("Name", "Ship"));
            Prefab.Add(new XAttribute("Id", prefabId)); 
            Prefab.Add(new XAttribute("Library", prefLibName));

            XElement Objects = new XElement("Objects");
            XElement Object = new XElement("Object");
            string partid = GuidUtility.GenID("werw34"+ shipName+ modificationName+ prefLibName);
            string layerid = GuidUtility.GenID("dsfhf555hyyh" + shipName + modificationName+ prefLibName);
            string parentid = "0";
            parentid = partid;
            
            if (shipModification.parts.Count!=0)
            {
                Object = AddObjects(Object, shipModification.parts.First(), loadout, parentid, partid, layerid, paint);
                Object = AddObjectsFromObjectContainers(Object, shipModification.objectContainers, parentid, layerid);
            }
            else
            { 
                Object = AddObjects(Object, ship.parts.First(), loadout, parentid, partid, layerid, paint);
                Object = AddObjectsFromObjectContainers(Object, ship.objectContainers, parentid, layerid);
            }
            i = 0;
            

            if (Object.Attribute("Name") != null) Objects.Add(Object);

            XElement ObjectsNew = new XElement("Objects");
            foreach (XElement obj in Objects.Descendants("Object"))
            {
                XElement ObjectNew = new XElement("Object");
                if (obj.Element("Properties") != null) ObjectNew.Add(obj.Element("Properties"));
                foreach (XAttribute atr in obj.Attributes())
                {
                    ObjectNew.Add(atr);
                }
                if (obj.Element("Components") != null)
                    ObjectNew.Add(obj.Element("Components"));

                if (ObjectNew.Attribute("Name") != null)
                    ObjectsNew.Add(ObjectNew);
            }

            Prefab.Add(ObjectsNew);
            PrefabsLibrary.Add(Prefab);
            foreach (XElement pr in prefabs)
            {
                PrefabsLibrary.Add(pr);
            }
            xml.Add(PrefabsLibrary);

            return xml;
        }
        public XDocument GenerateShip(ShipDefinition ship)
        {
            XDocument xml = new XDocument();
            string shipName = ship.GetShipName();
            ShipImplementation shipImplementation = ship.GetShipImplementation();
            if (shipImplementation!=null)
            {
                Paint paint = ship.GetShipPaint();
                //shipName = "AEGS_Gladius"; 

                string prefLibName = "";
                prefLibName = shipName;
                prefLibName = ship.GetVehicleType() + "/" + ship.GetManufacturer() + "/" + prefLibName;
                XElement PrefabsLibrary = new XElement("PrefabsLibrary");
                //PrefabsLibrary.Attribute("Name").Value = shipName+"_Ship";
                //PrefabsLibrary.Add(new XAttribute("Name", shipName + "_Ship"));
                PrefabsLibrary.Add(new XAttribute("Name", prefLibName));

                XElement Prefab = new XElement("Prefab");
                string prefabId = GuidUtility.GenPrefabID(prefLibName);
                //Prefab.Add(new XAttribute("Name", shipName));
                Prefab.Add(new XAttribute("Name", "Ship"));
                Prefab.Add(new XAttribute("Id", prefabId));
                // Prefab.Add(new XAttribute("Library", shipName + "_Ship")); 
                Prefab.Add(new XAttribute("Library", prefLibName));

                XElement Objects = new XElement("Objects");


                Loadout loadout = ship.GetLoadout();
                //Console.WriteLine(loadout.name);


                XElement Object = new XElement("Object");
                //string partid = GuidUtility.GenID("jhk777gt" + shipName  );
                //string layerid = GuidUtility.GenID("ghjg557kk" + shipName  );
                string partid = GuidUtility.GenID();
                string layerid = GuidUtility.GenID();
                string parentid = "0";
                parentid = partid;

                Object = AddObjects(Object, shipImplementation.parts.First(), loadout, parentid, partid, layerid, paint);
                i = 0;
                //Object = AddPrefabsFromObjectContainers(Object, ship, parentid, layerid);
                Object = AddObjectsFromObjectContainers(Object, shipImplementation.objectContainers, parentid, layerid);

                if (Object.Attribute("Name") != null) Objects.Add(Object);
                //Objects.Add(Object);

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
                foreach (XElement pr in prefabs)
                {
                    PrefabsLibrary.Add(pr);
                }
                xml.Add(PrefabsLibrary);

                //XDocument xmlpreflib = XDocument.Load(prefablib);
                //XElement preflibel = xmlpreflib.Element("PrefabsLibrary");
                //xml.Add(preflibel);
            }

            return xml;
        }
        public XDocument GenerateShipOLD(GenerateOptions genOptions)
        {
            string shipName = genOptions.shipName;
            ShipImplementation ship = exporter.GetShipImplementation(shipName);
            Paint paint = ship.GetPaintByName(genOptions.paintName);
            //shipName = "AEGS_Gladius";
            XDocument xml = new XDocument();
            string loadoutName = null;
            if (genOptions.loadoutName != null)
            {
                loadoutName = genOptions.loadoutName;

            }
            else
            {
                loadoutName = "Default_Loadout_" + shipName;
            }

            string prefLibName = "";
            if (paint != null)
            {
                prefLibName = shipName + "-" + paint.name;
            }
            else
            {
                prefLibName = shipName;
            }
            prefLibName = "ships/"+prefLibName;
            XElement PrefabsLibrary = new XElement("PrefabsLibrary");
            //PrefabsLibrary.Attribute("Name").Value = shipName+"_Ship";
            //PrefabsLibrary.Add(new XAttribute("Name", shipName + "_Ship"));
            PrefabsLibrary.Add(new XAttribute("Name", prefLibName));

            XElement Prefab = new XElement("Prefab");
            string prefabId = GuidUtility.GenPrefabID(prefLibName);
            //Prefab.Add(new XAttribute("Name", shipName));
            Prefab.Add(new XAttribute("Name", "Ship"));
            Prefab.Add(new XAttribute("Id", prefabId));
            // Prefab.Add(new XAttribute("Library", shipName + "_Ship")); 
            Prefab.Add(new XAttribute("Library", prefLibName));

            XElement Objects = new XElement("Objects");

            
            Loadout loadout = exporter.GetLoadout(loadoutName);
            Console.WriteLine(loadout.name);


            XElement Object = new XElement("Object");
            //string partid = GuidUtility.GenID("jhk777gt" + shipName  );
            //string layerid = GuidUtility.GenID("ghjg557kk" + shipName  );
            string partid = GuidUtility.GenID();
            string layerid = GuidUtility.GenID();
            string parentid = "0";
            parentid = partid;

            Object = AddObjects(Object, ship.parts.First(), loadout, parentid, partid, layerid, paint);
            i = 0;
            //Object = AddPrefabsFromObjectContainers(Object, ship, parentid, layerid);
            Object = AddObjectsFromObjectContainers(Object, ship.objectContainers, parentid, layerid);

            if (Object.Attribute("Name") != null) Objects.Add(Object);
            //Objects.Add(Object);

            XElement ObjectsNew = new XElement("Objects");
            foreach (XElement obj in Objects.Descendants("Object"))
            {
                XElement ObjectNew = new XElement("Object");
                //ObjectNew.Add(new XElement("Properties"));
                if (obj.Element("Properties") !=null) ObjectNew.Add(obj.Element("Properties"));
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
            foreach (XElement pr in prefabs)
            {
                PrefabsLibrary.Add(pr);
            }
            xml.Add(PrefabsLibrary);

            //XDocument xmlpreflib = XDocument.Load(prefablib);
            //XElement preflibel = xmlpreflib.Element("PrefabsLibrary");
            //xml.Add(preflibel);

            return xml;
        }
        static string RandomNumber(int length)
        {
            const string chars = "0123456789";
            string ret = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return ret;
        }

        static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnpxyz0123456789";
            string ret = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return ret;
        }
        
        string BytesToString(byte[] bytes)
        {
            string newString = "";
            foreach (byte bt in bytes)
            {
                newString += (bt / 16);
            }
            return newString;
        }
        string StringToNumbers(string str)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(str);
            string newString = "";
            foreach (byte bt in asciiBytes)
            {
                newString += (bt / 16);
            }
            return newString;
        }
    }
}
