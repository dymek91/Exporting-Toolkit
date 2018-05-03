using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using unforge;

namespace shipsExporter
{
    public abstract class Exporter
    {
        public bool SuccessfullyLoaded { get; set; } = true;

        List<ItemInterface> itemIterfaces = new List<ItemInterface>();
        List<Item> items = new List<Item>();
        public List<Loadout> Loadouts { get; } = new List<Loadout>();
        DataForge dataForge;

        abstract public void Init();

        public Loadout GetLoadout(string loadoutName)
        {
            Loadout loadout = new Loadout();
            foreach (Loadout lo in Loadouts)
            {
                if (lo.name == loadoutName) loadout = lo;
            }
            return loadout;
        } 
        public Item GetItem(string itemName, string shipName = "other")
        {
            Item item = null;
            foreach (Item it in items)
            {
                if (it.name == itemName) item = it;
            }
            if (item != null)
            {
                return item;
            }
            else
            {
                //Directory.CreateDirectory("logs/missingItems/"+ shipName);
                Directory.CreateDirectory("logs/missingItems/");

                XDocument xml = new XDocument();
                XElement elItem = new XElement("item");
                XElement elGeometry = new XElement("geometry");
                XElement elThirdperson = new XElement("thirdperson");
                elThirdperson.Add(new XAttribute("name", ""));
                elGeometry.Add(elThirdperson);
                elItem.Add(elGeometry);
                elItem.Add(new XAttribute("name", itemName));
                elItem.Add(new XAttribute("class", "MissingItem"));
                xml.Add(elItem);
                //xml.Save("logs/missingItems/" + shipName +"/" + itemName + ".xml");
                xml.Save("logs/missingItems/" + itemName + ".xml");

                item = new Item();
                return item;
            }
        }
        public int ItemIterfacesCount() { return itemIterfaces.Count; }
        public int ItemsCount() { return items.Count; }
        public int LoadoutsCount() { return Loadouts.Count; }

        public void LoadDataForge(string path)
        {
            if (File.Exists(path) && !File.Exists(Path.ChangeExtension(path, "xml")))
            {
                using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        dataForge = new DataForge(br);
                        //dataForge.GenerateSerializationClasses();
                        dataForge.Save(Path.ChangeExtension(path, "xml"));
                        Console.WriteLine();
                    }
                }
            }
            if(!File.Exists(path))
            {
                Console.WriteLine("[ERROR] NOT FOUND: {0}", path);
                SuccessfullyLoaded = false;
            }
        }

        public void LoadItemIterfaces(string path)
        {
            if (Directory.Exists(path))
            {

                foreach (var file in Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories))
                {
                    //Console.WriteLine(file);
                    XDocument xml = XDocumentHelper.Load(file);
                    IEnumerable<XElement> elements = xml.Elements();
                    foreach (var el in elements)
                    {
                        if (el.Name == "interface")
                        {
                            ItemInterface itemInterface = new ItemInterface();
                            itemInterface.name = el.Attribute("name").Value;
                            //Console.WriteLine(el.Name); 
                            //Console.WriteLine(el.Element("geometry").Element("thirdperson"));
                            //Console.WriteLine(el.Descendants("geometry").Descendants("thirdperson"));
                            //foreach (XElement ell in el.Descendants("geometry").Descendants("thirdperson"))
                            //{
                            //    Console.WriteLine(ell);
                            //}
                            if (el.Element("geometry") != null)
                            {
                                if (el.Element("geometry").Element("thirdperson") != null)
                                {
                                    itemInterface.geometry = el.Element("geometry").Element("thirdperson").Attribute("name").Value;
                                    //Console.WriteLine(itemInterface.geometry);
                                    //Console.WriteLine(el.Element("geometry").Element("thirdperson").Attribute("name").Value);
                                }
                            }
                            //Console.WriteLine(itemInterface.name);

                            itemIterfaces.Add(itemInterface);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("[ERROR] NOT FOUND: {0}", path);
                SuccessfullyLoaded = false;
            }
        }
        public void LoadItems(string path)
        {
            if (Directory.Exists(path))
            {

                foreach (var file in Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories))
                {
                    //Console.WriteLine(file);
                    XDocument xml = XDocumentHelper.Load(file);
                    IEnumerable<XElement> elements = xml.Elements();
                    foreach (var el in elements)
                    {
                        if (el.Name == "item")
                        {
                            Item item = new Item();
                            item.name = el.Attribute("name").Value;
                            item.itemclass = el.Attribute("class").Value;
                            //Console.WriteLine(el.Name); 
                            //Console.WriteLine(el.Element("geometry").Element("thirdperson"));
                            //Console.WriteLine(el.Descendants("geometry").Descendants("thirdperson"));
                            //foreach (XElement ell in el.Descendants("geometry").Descendants("thirdperson"))
                            //{
                            //    Console.WriteLine(ell);
                            //}
                            if (el.Element("geometry") != null)
                            {
                                if (el.Element("geometry").Element("thirdperson") != null)
                                {
                                    item.geometry = el.Element("geometry").Element("thirdperson").Attribute("name").Value;
                                    //Console.WriteLine(item.geometry);
                                    //Console.WriteLine(el.Element("geometry").Element("thirdperson").Attribute("name").Value);
                                }
                            }
                            if (el.Attribute("interface") != null)
                            {
                                item.intrface = el.Attribute("interface").Value;
                                ItemInterface iteminterface = FindItemInterface(item.intrface);
                                if (iteminterface.geometry != "") item.geometry = iteminterface.geometry;
                                //Console.WriteLine(item.geometry);
                            }
                            if (el.Element("PrefabAttachments") != null)
                            {
                                foreach (XElement ell in el.Element("PrefabAttachments").Descendants("PrefabAttachment"))
                                {
                                    PrefabAttachment prefabAtt = new PrefabAttachment();
                                    prefabAtt.attachmentPoint = ell.Attribute("attachmentPoint").Value;
                                    prefabAtt.prefabLibrary = ell.Attribute("prefabLibrary").Value;
                                    prefabAtt.prefabName = ell.Attribute("prefabName").Value;
                                    item.PrefabAttachments.Add(prefabAtt);
                                    // Console.WriteLine("prefabatt {0} {1} {2}", prefabAtt.attachmentPoint, prefabAtt.prefabLibrary, prefabAtt.prefabName);


                                }
                                //Console.WriteLine(item.PrefabAttachments.Count);
                            }
                            //Console.WriteLine(item.name);
                            items.Add(item);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("[ERROR] NOT FOUND: {0}", path);
                SuccessfullyLoaded = false;
            }
        }
        public void LoadItemsFromDataforge(string path)
        {
            if (File.Exists(path))
            {
                XDocument xmlGame = XDocument.Load(path);
                IEnumerable<XElement> elements = xmlGame.Element("DataForge").Elements();
                foreach (var el in elements)
                {
                    string[] words = el.Name.ToString().Split('.');
                    if (words.Length == 2)
                    {
                        string classDef = words[0];
                        string itemName = words[1];
                        if (classDef == "EntityClassDefinition")
                        {
                            if (el.Attribute("__path").Value.StartsWith("libs/foundry/records/entities/scitem"))
                            {
                                Item item = new Item();
                                item.name = itemName;
                                item.itemclass = "SCItem";
                                // Console.WriteLine(el.Name);
                                // Console.WriteLine(el.Element("geometry").Element("thirdperson"));
                                // Console.WriteLine(el.Descendants("geometry").Descendants("thirdperson"));
                                foreach (XElement ell in el.Descendants("geometry").Descendants("thirdperson"))
                                {
                                    Console.WriteLine(ell);
                                }
                                if (el.Element("Components") != null)
                                {
                                    if (el.Element("Components").Element("SGeometryResourceParams") != null)
                                    {
                                        if (el.Element("Components").Element("SGeometryResourceParams").Element("Geometry") != null)
                                        {
                                            if (el.Element("Components").Element("SGeometryResourceParams").Element("Geometry").Element("Geometry") != null)
                                            {
                                                if (el.Element("Components").Element("SGeometryResourceParams").Element("Geometry").Element("Geometry").Element("Geometry") != null)
                                                {
                                                    item.geometry = el.Element("Components").Element("SGeometryResourceParams").Element("Geometry").Element("Geometry").Element("Geometry").Attribute("path").Value;
                                                }
                                            }
                                        }
                                    }
                                }

                                //Console.WriteLine(item.name);
                                items.Add(item);
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("[ERROR] NOT FOUND: {0}", path);
                SuccessfullyLoaded = false;
            }
        }
        public void LoadLoadouts(string path)
        {
            if (Directory.Exists(path))
            { 
                foreach (var file in Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories))
                {
                    //Console.WriteLine(file);
                    XDocument xml = XDocumentHelper.Load(file);
                    string name = Path.GetFileNameWithoutExtension(file);
                    Loadout loadout = new Loadout(xml, name, file);
                    if (loadout.name != null)
                        Loadouts.Add(loadout); 
                }
            }
            else
            {
                Console.WriteLine("[ERROR] NOT FOUND: {0}", path);
                SuccessfullyLoaded = false;
            }
        }
        ItemInterface FindItemInterface(string name)
        {
            ItemInterface retintr = new ItemInterface();
            retintr.name = "";
            retintr.geometry = "";

            foreach (ItemInterface intr in itemIterfaces)
            {
                if (intr.name == name) retintr = intr;
            }

            return retintr;
        }
    }
}
