using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.IO.Compression;
using unforge;

namespace shipsExporter
{
    class ShipsExporter : Exporter
    {
        //string objectContainersVer = "3_0_0";
        
        List<ShipImplementation> shipsImplementations = new List<ShipImplementation>();
        List<ShipDefinition> shipsDefinitions = new List<ShipDefinition>();
         
        override public void Init()
        {
            try
            {
                Console.WriteLine("Loading items.");
                LoadItemIterfaces("./data/Scripts/Entities/Items");
                LoadItems("./data/Scripts/Entities/Items");
                Console.WriteLine("Loading DataForge.");
                LoadDataForge("./data/Game.dcb");
                Console.WriteLine("Loading items from DataForge.");
                LoadItemsFromDataforge("./data/Game.xml");
                Console.WriteLine("Exporting Cubemaps.");
                ExportAllCubemaps("./data/ObjectContainers");
                Console.WriteLine("Loading ships definitions.");
                LoadShipsDefinitions("./data/Game.xml");
            }
            catch(FormatException e)
            {
                Console.Write("[ERROR] ");
                Console.WriteLine(e.Message);
                SuccessfullyLoaded = false;
            }
        }

        public ShipImplementation GetShipImplementation(string shipname)
        {
            ShipImplementation ship = null;
            foreach (ShipImplementation sp in shipsImplementations)
            {
                if (sp.name == shipname) ship = sp;
            }
            return ship;
        }
        public ShipDefinition GetShipDefinition(string shipname)
        {
            ShipDefinition ship = null;
            foreach (ShipDefinition sp in shipsDefinitions)
            {
                if (sp.GetShipName() == shipname) ship = sp;
            }
            return ship;
        }
        
        public int ShipsCount() { return shipsDefinitions.Count; }
        public List<ShipImplementation> GetShipsImplemenations() { return shipsImplementations; }
        public List<ShipDefinition> GetShips() { return shipsDefinitions; }

        void ExportAllCubemaps(string ocsPath)
        {
            if (Directory.Exists(ocsPath)&&!Directory.Exists("./exported/Assets/ObjectContainers"))
            {
                foreach (string file in Directory.GetFiles(ocsPath, "*.socpak", SearchOption.AllDirectories))
                {
                    using (ZipArchive zip = ZipFile.Open(file, ZipArchiveMode.Read))
                    {
                        foreach (ZipArchiveEntry entry in zip.Entries)
                        {
                            Stream streamFile = new MemoryStream();
                            //Console.WriteLine("LoadSoc entry.FullName {0}", entry.FullName);
                            if (entry.FullName.EndsWith(".dds"))
                            {
                                Console.Write(".");
                                string dirToSave= Path.GetDirectoryName(file)+"\\" + Path.GetDirectoryName(entry.FullName);
                                dirToSave = dirToSave.TrimStart(".\\data".ToCharArray());
                                dirToSave = ".\\exported\\Assets\\"+dirToSave;

                                string fileName = Path.GetFileName(entry.FullName);
                                Directory.CreateDirectory(dirToSave);

                                ZipArchiveEntry ddsFile = zip.GetEntry(entry.FullName);
                                ddsFile.Open().CopyTo(streamFile);
                                File.WriteAllBytes(dirToSave+"\\"+fileName, ReadFully(streamFile));
                            }
                        } 
                    }
                }
                Console.WriteLine();
            }
            if (!Directory.Exists(ocsPath))
            {
                Console.WriteLine("[ERROR] NOT FOUND: {0}", ocsPath);
                SuccessfullyLoaded = false;
            }
        }
        byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            using (BinaryReader br = new BinaryReader(input))
            {
                br.BaseStream.Position = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    buffer[i] = br.ReadByte();
                }
            }
            
            return buffer;
        }
        
        void LoadShipsDefinitions(string path)
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
                            if (el.Attribute("__path").Value.StartsWith("libs/foundry/records/vehicle/spaceship"))
                            {
                                Console.Write(".");
                                string __path = el.Attribute("__path").Value;

                                string manufacturer = Path.GetFileName( Path.GetDirectoryName(__path)); 
                                string vehicleType = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(__path)));

                                ShipDefinition shipDef = new ShipDefinition(el, manufacturer, vehicleType);
                                if ((shipDef.GetShipImplementation()!=null) && (shipDef.GetLoadout() != null))
                                {
                                    shipsDefinitions.Add(shipDef);
                                }
                            }
                            if (el.Attribute("__path").Value.StartsWith("libs/foundry/records/vehicle/groundvehicle"))
                            {
                                Console.Write(".");
                                string __path = el.Attribute("__path").Value;

                                string manufacturer = Path.GetFileName(Path.GetDirectoryName(__path));
                                string vehicleType = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(__path)));

                                ShipDefinition shipDef = new ShipDefinition(el, manufacturer, vehicleType);
                                if ((shipDef.GetShipImplementation() != null) && (shipDef.GetLoadout() != null))
                                {
                                    shipsDefinitions.Add(shipDef);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine();
            }
        }
       
       
        Part LoadParts(XElement ell)
        {
            Part part = new Part(); 

            if (ell.Attribute("helper") != null) part.helper = ell.Attribute("helper").Value;
            if (ell.Attribute("name") != null) { part.name = ell.Attribute("name").Value; /*Console.WriteLine("part.name {0}", part.name);*/ }
            if (ell.Attribute("class") != null) part.partClass = ell.Attribute("class").Value;
            if (part.partClass == "Animated" && ell.Element("Animated") != null) part.animatedFilename = ell.Element("Animated").Attribute("filename").Value;
            //Console.WriteLine(part.name + " " + part.partClass + " " + part.animatedFilename);
            if (part.partClass == "ItemPort")
                if (ell.Element("ItemPort") != null)
                    if (ell.Element("ItemPort").Element("AttachmentPoints") != null)
                        foreach (XElement ellx in ell.Element("ItemPort").Element("AttachmentPoints").Elements("AttachmentPoint"))
                        {
                            AttachmentPoint attpoint = new AttachmentPoint();
                            attpoint.name = ellx.Attribute("name").Value;
                            attpoint.bone = ellx.Attribute("bone").Value;
                            part.attachmentPoints.Add(attpoint);
                            //Console.WriteLine(attpoint.name);
                        }
            foreach (XElement ell2 in ell.Elements("Parts").Elements("Part"))
            {
                Part part2 = new Part();
                part2 = LoadParts(ell2);
                part.parts.Add(part2);
            }

            return part;
        }
        static List<ObjectContainer> LoadObjectContainers(XElement el)
        {
            List<ObjectContainer> objectContainers = new List<ObjectContainer>();
            foreach (XElement ell in el.Descendants("ObjectContainer"))
            {
                string portName = ell.Attribute("portName").Value;
                string filename = ell.Attribute("filename").Value;
               // Console.WriteLine("portname {0}", portName);
                objectContainers.Add(new ObjectContainer(filename, portName));
                
            }
            return objectContainers;
        }
        //static List<ObjectContainer_SC_3_0_0> LoadObjectContainers3_0_0(XElement el)
        //{
        //    List<ObjectContainer_SC_3_0_0> objectContainers = new List<ObjectContainer_SC_3_0_0>();
        //    foreach (XElement ell in el.Descendants("ObjectContainer"))
        //    {
        //        string portName = ell.Attribute("portName").Value;
        //        string filename = ell.Attribute("filename").Value;
        //        // Console.WriteLine("portname {0}", portName);
        //        objectContainers.Add(new ObjectContainer_SC_3_0_0(filename, portName));

        //    }
        //    return objectContainers;
        //}
        void LoadShipsImplementations(string path)
        { 
            if (Directory.Exists(path))
            {

                foreach (var file in Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories))
                {
                   // Console.WriteLine(file);
                    XDocument xml = XDocument.Load(file);
                    IEnumerable<XElement> elements = xml.Elements();
                    foreach (var el in elements)
                    {
                        ShipImplementation ship = new ShipImplementation();
                        if (el.Name == "Vehicle")
                        {
                            ship.name = el.Attribute("name").Value;
                            //Part part = new Part();
                            foreach (XElement ell in el.Elements("Parts").Elements("Part"))
                            {
                                Part part2 = new Part();
                                part2 = LoadParts(ell);
                                //part.parts.Add(part2);
                                ship.parts.Add(part2);
                            }
                            foreach (XElement ell in el.Elements("Modifications").Elements("Modification"))
                            {
                                Modification mod = new Modification(); 
                                mod.name = ell.Attribute("name").Value;
                                if(ell.Attribute("patchFile")!=null) mod.patchName = ell.Attribute("patchFile").Value;
                                if (mod.patchName != null)
                                    if(mod.patchName!="")
                                    {
                                        mod.trueName = Path.GetFileName(mod.patchName);

                                        if (File.Exists(path + "/" + mod.patchName + ".xml"))
                                        {
                                            XDocument xmlMod = XDocument.Load(File.OpenRead(path + "/" + mod.patchName + ".xml"));

                                            if (xmlMod.Element("Modifications").Element("Parts")!=null)
                                            {
                                                foreach (XElement elPart in xmlMod.Element("Modifications").Elements("Parts").Elements("Part"))
                                                {
                                                    //Console.WriteLine(elPart.Attribute("name").Value);
                                                    Part part = new Part();
                                                    part = LoadParts(elPart);
                                                    mod.parts.Add(part);
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("No geometry Modifications for " + mod.trueName);
                                            }
                                            mod.objectContainers = LoadObjectContainers(xmlMod.Element("Modifications"));
                                            ship.modifications.Add(mod);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Not Found:" + path + "/" + mod.patchName + ".xml");
                                        }
                                    }
                            }
                            foreach(XElement ell in el.Elements("Paints").Elements("Paint"))
                            {
                                Paint paint = new Paint();
                                paint.name = ell.Attribute("name").Value;
                                paint.material = ell.Attribute("material").Value;
                                ship.paints.Add(paint);
                            }
                            ship.objectContainers = LoadObjectContainers(el);
                            //ship.parts.Add(part);
                            shipsImplementations.Add(ship);
                        }
                        
                       // Console.WriteLine();
                    }
                }
            }
        }
        
    }
}
