using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace shipsExporter
{
    class EntityPrefab
    {
        ShipsExporter exporter;
        private static Random random = new Random();
        int i = 0;
        string[] allowedItemClasses = { "VehicleWeapon", "VehicleTurret", "VehicleItemThruster", "VehicleMissileRack", "VehicleItemSeat" };
        string prefablib = "";
        List<XElement> prefabs = new List<XElement>();

        public EntityPrefab(ShipsExporter exp)
        {
            exporter = exp;
        }
        public string getPrefabGUID(string PrefabLibrary,string PrefabName)
        {
            string guid="";
            XDocument xml = XDocument.Load(PrefabLibrary);
            foreach(XElement el in xml.Descendants("Prefab"))
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
                Object.Add(new XAttribute("Parent", parentid));
                Object.Add(new XAttribute("Id", partid));
            }

            if (part.partClass == "Animated")
            {

                if (i != 0)
                Object.Add(new XAttribute("Parent", parentid));
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
                i++;
            }
            if (part.partClass == "ItemPort")
            {
                ItemPort itemport = loadout.GetItemPort(part.name);
                Item item = exporter.GetItem(itemport.itemName);
                if (item.geometry != "" && item.itemclass != "VehicleItemMultiLight" && allowedItemClasses.Any(item.itemclass.Contains))
                {
                    Object.Add(new XAttribute("Name", part.name));
                    Object.Add(new XAttribute("Parent", parentid));
                    Object.Add(new XAttribute("Id", partid));
                    Object.Add(new XAttribute("LayerGUID", layerid));
                    Object.Add(new XAttribute("Geometry", item.geometry));
                    Object.Add(new XAttribute("AttachmentType", "CharacterBone"));
                    Object.Add(new XAttribute("AttachmentTarget", itemport.portName));
                    Object.Add(new XAttribute("Layer", "Main"));
                    Object.Add(new XAttribute("Type", "GeomEntity"));
                    Object.Add(new XAttribute("EntityClass", "GeomEntity"));
                    Object.Add(new XAttribute("LodRatio", "10"));

                    foreach (ItemPort ip in itemport.itemPorts)
                    {
                        Item item2 = exporter.GetItem(ip.itemName);
                        XElement obj2 = new XElement("Object");
                        obj2.Add(new XAttribute("Name", ip.portName));
                        obj2.Add(new XAttribute("Parent", partid));
                        obj2.Add(new XAttribute("Id", RandomString(10)));
                        obj2.Add(new XAttribute("LayerGUID", layerid));
                        obj2.Add(new XAttribute("Geometry", item2.geometry));
                        obj2.Add(new XAttribute("AttachmentType", "CharacterBone"));
                        obj2.Add(new XAttribute("AttachmentTarget", ip.portName));
                        obj2.Add(new XAttribute("Layer", "Main"));
                        obj2.Add(new XAttribute("Type", "GeomEntity"));
                        obj2.Add(new XAttribute("EntityClass", "GeomEntity"));
                        obj2.Add(new XAttribute("LodRatio", "10")); 
                        Object.Add(obj2);
                    }
                }
                if (item.itemclass == "VehicleItemMultiLight")
                {
                    foreach (AttachmentPoint at in part.attachmentPoints)
                    {
                        //Console.WriteLine(at.name);
                        XElement obj1 = new XElement("Object");
                        string entid = RandomString(10);
                        obj1.Add(new XAttribute("Type", "Entity"));
                        obj1.Add(new XAttribute("Name", at.bone));
                        obj1.Add(new XAttribute("Parent", parentid));
                        obj1.Add(new XAttribute("Id", entid));
                        obj1.Add(new XAttribute("LayerGUID", layerid));
                        obj1.Add(new XAttribute("AttachmentType", "CharacterBone"));
                        obj1.Add(new XAttribute("AttachmentTarget", at.bone));
                        obj1.Add(new XAttribute("Layer", "Main"));
                        obj1.Add(new XAttribute("EntityClass", "FlowgraphEntity"));
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
                        catch(Exception e)
                        {
                            Console.WriteLine("Error:");  
                            Console.WriteLine("prefabLibrary: {0} prefabName: {1} attachmentPoint: {2}", prefabAtt.prefabLibrary, prefabAtt.prefabName, prefabAtt.attachmentPoint);
                            Console.WriteLine("at.name: {0}", at.name);
                            Console.WriteLine("item.name: {0}", item.name);
                            //throw e;
                        }
                        string prefid = RandomString(10);
                        string prefablibname = Path.GetFileNameWithoutExtension(prefabAtt.prefabLibrary);
                        obj2.Add(new XAttribute("Type", "Prefab"));
                        obj2.Add(new XAttribute("Name", at.name));
                        obj2.Add(new XAttribute("Parent", entid));
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
             
            foreach(Part pr in part.parts)
            {
                XElement obj = new XElement("Object");
                obj = addObjects(obj, pr, loadout, partid, RandomString(10), layerid);
               //if (obj.Attribute("Name") != null)
                    Object.Add(obj);
            }

            return Object;
        }
        XElement addPrefabsFromObjectContainers(XElement el, ShipImplementation ship, string parentid, string layerid)
        {
            XElement Object = el;
            foreach (ObjectContainer oc in ship.objectContainers)
            {
                string layername = oc.name;
                layerid = stringToNumbers(layername);
                //Console.WriteLine(at.name);
                XElement obj1 = new XElement("Object");
                string entid = RandomString(10);
                string prefid = RandomString(10);
                obj1.Add(new XAttribute("Type", "Entity"));
                obj1.Add(new XAttribute("Name", oc.portName));
                obj1.Add(new XAttribute("Parent", parentid));
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
                string id = RandomString(10);
                string prefablibname = ship.name;
                obj2.Add(new XAttribute("Type", "Prefab"));
                obj2.Add(new XAttribute("Name", oc.name));
                obj2.Add(new XAttribute("Parent", entid));
                obj2.Add(new XAttribute("Id", id));
                obj2.Add(new XAttribute("LayerGUID", layerid));
                obj2.Add(new XAttribute("Layer", layername));
                obj2.Add(new XAttribute("PrefabGUID", prefid));
                obj2.Add(new XAttribute("PrefabLibrary", prefablibname));
                obj2.Add(new XAttribute("PrefabName", "ObjectContainers." + oc.name));
                //prefablib = prefabAtt.prefabLibrary;
                prefabs.Add(oc.GetAsPrefab(prefablibname, layerid, prefid, layername));
                Object.Add(obj2);

                foreach(Child child in oc.childs)
                {
                    ObjectContainer ocChild=child.Soc;
                    prefid = RandomString(10);
                    XElement obj3 = new XElement("Object");
                    string id2 = RandomString(10);
                    string prefablibname2 = ship.name;
                    obj3.Add(new XAttribute("Type", "Prefab"));
                    obj3.Add(new XAttribute("Name", ocChild.name));
                    obj3.Add(new XAttribute("Parent", entid));
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

        public XDocument generateShip(string shipName)
        {
            //shipName = "AEGS_Gladius";
            XDocument xml = new XDocument();
            string loadoutName = "Default_Loadout_"+ shipName;

            XElement PrefabsLibrary = new XElement("PrefabsLibrary");
            //PrefabsLibrary.Attribute("Name").Value = shipName+"_Ship";
            //PrefabsLibrary.Add(new XAttribute("Name", shipName + "_Ship"));
            PrefabsLibrary.Add(new XAttribute("Name", shipName));

            XElement Prefab = new XElement("Prefab");
            string prefabId = RandomString(10);
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
            string partid = RandomString(10);
            string layerid = RandomString(10);
            string parentid = "0"; 
            parentid = partid;

            Object = addObjects(Object, ship.parts.First(), loadout, parentid, partid, layerid);
            Object = addPrefabsFromObjectContainers(Object, ship, parentid, layerid);
            if (Object.Attribute("Name") != null) Objects.Add(Object);
            //Objects.Add(Object);

            XElement ObjectsNew = new XElement("Objects");
            foreach(XElement obj in Objects.Descendants("Object"))
            {
                XElement ObjectNew = new XElement("Object");
                foreach(XAttribute atr in obj.Attributes())
                {
                    ObjectNew.Add(atr);
                } 
                if (ObjectNew.Attribute("Name") != null)
                    ObjectsNew.Add(ObjectNew);
            }

            Prefab.Add(ObjectsNew);
            PrefabsLibrary.Add(Prefab); 
            foreach(XElement pr in prefabs)
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
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string ret= new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray()); 
            return "{3675a837-e15f-dfa0-db3a-" + RandomNumber(4) + ret + "}";
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
