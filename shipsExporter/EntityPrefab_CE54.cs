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
    class EntityPrefab_CE54
    {
        ShipsExporter exporter;
        private static Random random = new Random();
        int i = 0;
        string[] allowedItemClasses = { "VehicleWeapon", "VehicleTurret", "VehicleItemThruster", "VehicleMissileRack", "VehicleItemSeat" };
        string prefablib = "";
        List<XElement> prefabs = new List<XElement>();

        public EntityPrefab_CE54(ShipsExporter exp)
        {
            exporter = exp;
        }
        public string getPrefabGUID(string PrefabLibrary, string PrefabName)
        {
            string guid = "";
            XDocument xml = XDocument.Load(PrefabLibrary);
            foreach (XElement el in xml.Descendants("Prefab"))
            {
                if (el.Attribute("Name").Value == PrefabName) guid = el.Attribute("Id").Value;
            }
            return guid;
        }
        public XElement getPrefab(string PrefabLibrary, string PrefabName)
        {
            XElement obj = new XElement("Prefab");
            XDocument xml = XDocument.Load(PrefabLibrary);
            foreach (XElement el in xml.Descendants("Prefab"))
            {
                if (el.Attribute("Name").Value == PrefabName) obj = el;
            }
            return obj;
        }
        public XElement addObjects(XElement el, Part part, Loadout loadout, string parentid, string partid, string layerid)
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

            if (part.partClass == "Animated")
            {

                if (i != 0)
                    Object.Add(new XAttribute("LinkedTo", parentid));
                Object.Add(new XAttribute("Name", part.name));
                Object.Add(new XAttribute("Id", partid));
                Object.Add(new XAttribute("LayerGUID", layerid));
                Object.Add(new XAttribute("Geometry", part.animatedFilename));
                Object.Add(new XAttribute("Layer", "Main"));
                Object.Add(new XAttribute("Type", "GeomEntity"));
                Object.Add(new XAttribute("EntityClass", "GeomEntity"));
                Object.Add(new XAttribute("LodRatio", "10"));
                if (part.helper != "") Object.Add(new XAttribute("AttachmentType", "CharacterBone"));
                if (part.helper != "") Object.Add(new XAttribute("AttachmentTarget", part.helper));

                XElement Components = new XElement("Components");
                XElement Component = new XElement("Component");
                Component.Add(new XAttribute("typeId", "ec0cd266-a6d1-4774-b499-690bd6fb61ee"));
                Component.Add(new XAttribute("guid", genID()));
                Component.Add(new XAttribute("CryXmlVersion", "2"));
                Component.Add(new XAttribute("Geometry", part.animatedFilename));
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
                Item item = exporter.GetItem(itemport.itemName);
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
                    Object.Add(new XAttribute("LodRatio", "10"));

                    XElement Components = new XElement("Components");
                    XElement Component = new XElement("Component");
                    Component.Add(new XAttribute("typeId", "ec0cd266-a6d1-4774-b499-690bd6fb61ee"));
                    Component.Add(new XAttribute("guid", genID()));
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
                        Item item2 = exporter.GetItem(ip.itemName);
                        XElement obj2 = new XElement("Object");
                        obj2.Add(new XAttribute("Name", ip.portName));
                        obj2.Add(new XAttribute("LinkedTo", partid));
                        obj2.Add(new XAttribute("Id", genID()));
                        obj2.Add(new XAttribute("LayerGUID", layerid));
                        obj2.Add(new XAttribute("Geometry", item2.geometry));
                        obj2.Add(new XAttribute("AttachmentType", "CharacterBone"));
                        obj2.Add(new XAttribute("AttachmentTarget", ip.portName));
                        obj2.Add(new XAttribute("Layer", "Main"));
                        obj2.Add(new XAttribute("Type", "GeomEntity"));
                        obj2.Add(new XAttribute("EntityClass", "GeomEntity"));
                        obj2.Add(new XAttribute("LodRatio", "10"));

                        XElement Components2 = new XElement("Components");
                        XElement Component2 = new XElement("Component");
                        Component2.Add(new XAttribute("typeId", "ec0cd266-a6d1-4774-b499-690bd6fb61ee"));
                        Component2.Add(new XAttribute("guid", genID()));
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
                        string entid = genID();
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
                            PrefabGUID = getPrefabGUID(prefabAtt.prefabLibrary, prefabAtt.prefabName);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error:");
                            Console.WriteLine("prefabLibrary: {0} prefabName: {1} attachmentPoint: {2}", prefabAtt.prefabLibrary, prefabAtt.prefabName, prefabAtt.attachmentPoint);
                            Console.WriteLine("at.name: {0}", at.name);
                            Console.WriteLine("item.name: {0}", item.name);
                            //throw e;
                        }
                        string prefid = genID();
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
                        prefabs.Add(getPrefab(prefabAtt.prefabLibrary, prefabAtt.prefabName));
                        Object.Add(obj2);
                    }
                }

            }

            foreach (Part pr in part.parts)
            {
                XElement obj = new XElement("Object");
                obj = addObjects(obj, pr, loadout, partid, genID(), layerid);
                //if (obj.Attribute("Name") != null)
                Object.Add(obj);
            }

            return Object;
        }
        XElement addPrefabsFromObjectContainers(XElement el, ShipImplementation ship, string parentid, string layerid)
        {
            XElement Object = el;
            layerid = Guid.NewGuid().ToString();
            foreach (ObjectContainer oc in ship.objectContainers)
            {
                string layername = oc.name;
                //layerid = stringToNumbers(layername);
                //Console.WriteLine(at.name);
                XElement obj1 = new XElement("Object");
                string entid = genID();
                string prefid = genID();
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
                string id = genID();
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
                    prefid = genID();
                    XElement obj3 = new XElement("Object");
                    string id2 = genID();
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
        XElement addObjectsFromObjectContainers(XElement el, ShipImplementation ship, string parentid, string layerid)
        {
            XElement Object = el;
            layerid = Guid.NewGuid().ToString();
            foreach (ObjectContainer oc in ship.objectContainers)
            {
                foreach (PrefabObject po in oc.prefabObjects)
                {
                    if (po.Geometry!=null)
                    {
                        string layername = oc.name;
                        XElement obj1 = new XElement("Object");
                        string entid = genID();
                        string prefid = genID();

                        obj1.Add(new XAttribute("Name", po.Name));
                        obj1.Add(new XAttribute("LinkedTo", parentid));
                        obj1.Add(new XAttribute("Id", entid));
                        obj1.Add(new XAttribute("LayerGUID", layerid));
                        obj1.Add(new XAttribute("Geometry", po.Geometry));
                        obj1.Add(new XAttribute("AttachmentType", "CharacterBone"));
                        obj1.Add(new XAttribute("AttachmentTarget", oc.portName));
                        obj1.Add(new XAttribute("Layer", "Main"));
                        if (po.Pos != null) obj1.Add(new XAttribute("Pos",  po.Pos));
                        if (po.Rotate!=null) obj1.Add(new XAttribute("Rotate", po.Rotate));
                        obj1.Add(new XAttribute("Type", "GeomEntity"));
                        obj1.Add(new XAttribute("EntityClass", "GeomEntity"));
                        obj1.Add(new XAttribute("LodRatio", "10"));

                        XElement Components = new XElement("Components");
                        XElement Component = new XElement("Component");
                        Component.Add(new XAttribute("typeId", "ec0cd266-a6d1-4774-b499-690bd6fb61ee"));
                        Component.Add(new XAttribute("guid", genID()));
                        Component.Add(new XAttribute("CryXmlVersion", "2"));
                        Component.Add(new XAttribute("Geometry", po.Geometry));
                        Component.Add(new XAttribute("Physicalize", "ePhysicalizationType_Static"));
                        Component.Add(new XAttribute("ReceiveCollisionEvents", "false"));
                        Component.Add(new XAttribute("Mass", "1"));
                        Component.Add(new XAttribute("Animation", ""));
                        Component.Add(new XAttribute("Speed", "1"));
                        Component.Add(new XAttribute("Loop", "true"));
                        Components.Add(Component);
                        obj1.Add(Components);
                        Object.Add(obj1); 
                  }
                }
            }

            return Object;
        }

        public XDocument generateShip(string shipName)
        {
            //shipName = "AEGS_Gladius";
            XDocument xml = new XDocument();
            string loadoutName = "Default_Loadout_" + shipName;

            XElement PrefabsLibrary = new XElement("PrefabsLibrary");
            //PrefabsLibrary.Attribute("Name").Value = shipName+"_Ship";
            //PrefabsLibrary.Add(new XAttribute("Name", shipName + "_Ship"));
            PrefabsLibrary.Add(new XAttribute("Name", shipName));

            XElement Prefab = new XElement("Prefab");
            string prefabId = genPrefabID();
            //Prefab.Add(new XAttribute("Name", shipName));
            Prefab.Add(new XAttribute("Name", "Ship"));
            Prefab.Add(new XAttribute("Id", prefabId));
            // Prefab.Add(new XAttribute("Library", shipName + "_Ship")); 
            Prefab.Add(new XAttribute("Library", shipName));

            XElement Objects = new XElement("Objects");

            ShipImplementation ship = exporter.GetShipImplementation(shipName);
            Loadout loadout = exporter.GetLoadout(loadoutName);
            Console.WriteLine(loadout.name);


            XElement Object = new XElement("Object");
            string partid = genID();
            string layerid = genID();
            string parentid = "0";
            parentid = partid;

            Object = addObjects(Object, ship.parts.First(), loadout, parentid, partid, layerid);
            //Object = addPrefabsFromObjectContainers(Object, ship, parentid, layerid);
            Object = addObjectsFromObjectContainers(Object, ship, parentid, layerid);
            
            if (Object.Attribute("Name") != null) Objects.Add(Object);
            //Objects.Add(Object);

            XElement ObjectsNew = new XElement("Objects");
            foreach (XElement obj in Objects.Descendants("Object"))
            {
                XElement ObjectNew = new XElement("Object");
                ObjectNew.Add(new XElement("Properties"));
                foreach (XAttribute atr in obj.Attributes())
                {
                    ObjectNew.Add(atr);
                }
                if( obj.Element("Components")!=null) 
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
            return  ret;
        }
        static string genID()
        {
            //return   RandomString(8) + "-" + RandomString(4) + "-" + RandomString(4) + "-" + RandomString(4) + "-" + RandomString(12);
            return Guid.NewGuid().ToString();
        }
        static string genPrefabID()
        {
            //return RandomNumber(8) + "-" + RandomNumber(4) + "-" + RandomString(4) + "-" + RandomString(4) + "-" + RandomString(10);
            return Guid.NewGuid().ToString(); 
        }
        string bytesToString(byte[] bytes)
        {
            string newString = "";
            foreach (byte bt in bytes)
            {
                newString += (bt / 16);
            }
            return newString;
        }
        string stringToNumbers(string str)
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
