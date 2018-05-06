using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace shipsExporter
{
    class ShipImplementation
    {
        public List<Part> parts = new List<Part>();
        public List<ObjectContainer> objectContainers = new List<ObjectContainer>();
        public List<Modification> modifications = new List<Modification>();
        public List<Paint> paints = new List<Paint>();
        public string name;
        List<Item> itemsList;

        public ShipImplementation()
        {

        }
        public ShipImplementation(XDocument xmlDoc,string pathFolder, List<Item> items)
        {
            itemsList = items;
            XElement el = xmlDoc.Root; 
                if (el.Name == "Vehicle")
                {
                    name = el.Attribute("name").Value;
                    //Part part = new Part();
                    foreach (XElement ell in el.Elements("Parts").Elements("Part"))
                    {
                        Part part2 = new Part();
                        part2 = LoadParts(ell);
                        //part.parts.Add(part2);
                        parts.Add(part2);
                    }
                    foreach (XElement ell in el.Elements("Modifications").Elements("Modification"))
                    {
                        Modification mod = new Modification();
                        mod.name = ell.Attribute("name").Value;
                        if (ell.Attribute("patchFile") != null) mod.patchName = ell.Attribute("patchFile").Value;
                        if (mod.patchName != null)
                            if (mod.patchName != "")
                            {
                                mod.trueName = Path.GetFileName(mod.patchName);

                                if (File.Exists(pathFolder + "/" + mod.patchName + ".xml"))
                                {
                                    XDocument xmlMod = XDocument.Load(File.OpenRead(pathFolder + "/" + mod.patchName + ".xml"));

                                    if (xmlMod.Element("Modifications").Element("Parts") != null)
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
                                        //Console.WriteLine("No geometry Modifications for " + mod.trueName);
                                    }
                                    mod.objectContainers = LoadObjectContainers(xmlMod.Element("Modifications"));
                                    modifications.Add(mod);
                                }
                                else
                                {
                                   // Console.WriteLine("Not Found:" + pathFolder + "/" + mod.patchName + ".xml");
                                }
                            }
                    }
                    foreach (XElement ell in el.Elements("Paints").Elements("Paint"))
                    {
                        Paint paint = new Paint();
                        paint.name = ell.Attribute("name").Value;
                        paint.material = ell.Attribute("material").Value;
                        paints.Add(paint);
                    }
                    objectContainers = LoadObjectContainers(el);
                    //ship.parts.Add(part);
                }

                // Console.WriteLine();
            
        }

        public int GetModsCount()
        {
            return modifications.Count;
        }
        public List<AttachmentPoint> GetAllAttachmentPoints()
        {
            List<AttachmentPoint> attPoints = new List<AttachmentPoint>();
            foreach(Part pt in parts)
            {
                foreach(AttachmentPoint at in pt.GetAllAttachmentPoints())
                {
                    attPoints.Add(at);
                }
            }
            return attPoints;
        }
        public AttachmentPoint GetAttachmentPointByName(string attName)
        {
            AttachmentPoint attPoint = null;
            foreach(AttachmentPoint at in GetAllAttachmentPoints())
            {
                //Console.WriteLine("AttName {0} Bone {1}",at.name,at.bone);
                if (at.name==attName) attPoint = at;
            }
            return attPoint;
        } 
        public Modification GetModificationByName(string modName)
        {
            Modification mod = null;
            foreach (Modification md in modifications)
            {
                //Console.WriteLine("AttName {0} Bone {1}",at.name,at.bone);
                if (md.name == modName) mod = md;
            }
            return mod;
        }
        public Paint GetPaintByName(string paintName=null)
        {
            if (paintName!=null)
            {
                Paint paint = null;
                foreach (Paint pt in paints)
                {
                    if (pt.name == paintName) paint = pt;
                }
                return paint;
            }
            else return null; 
        }

        static Part LoadParts(XElement ell)
        {
            Part part = new Part();

            if (ell.Attribute("helper") != null) part.helper = ell.Attribute("helper").Value;
            if (ell.Attribute("name") != null) { part.name = ell.Attribute("name").Value; /*Console.WriteLine("part.name {0}", part.name);*/ }
            if (ell.Attribute("class") != null) part.partClass = ell.Attribute("class").Value;
            if (part.partClass == "Animated" && ell.Element("Animated") != null) part.animatedFilename = ell.Element("Animated").Attribute("filename").Value;
            if (part.partClass == "SubPartWheel" && ell.Element("SubPart") != null) part.animatedFilename = ell.Element("SubPart").Attribute("filename").Value;
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
        List<ObjectContainer> LoadObjectContainers(XElement el)
        {
            List<ObjectContainer> objectContainers = new List<ObjectContainer>();
            foreach (XElement ell in el.Descendants("ObjectContainer"))
            {
                string portName = ell.Attribute("portName").Value;
                string filename = ell.Attribute("filename").Value;
                // Console.WriteLine("portname {0}", portName);
                objectContainers.Add(new ObjectContainer(filename, portName,itemsList));

            }
            return objectContainers;
        }
    }
    class Modification
    {
        public string name;
        public string trueName;
        public string patchName;
        public List<Part> parts = new List<Part>();
        public List<ObjectContainer> objectContainers = new List<ObjectContainer>();
    }
    class Paint
    {
        public string name;
        public string material;
    }
    class Part
    {
        public List<Part> parts = new List<Part>();
        public List<AttachmentPoint> attachmentPoints = new List<AttachmentPoint>();
        public string name;
        public string partClass;
        public string animatedFilename;
        public string helper;
        public Part()
        {
            helper = "";
        }
        public List<AttachmentPoint> GetAllAttachmentPoints()
        {
            List<AttachmentPoint> attPoints = new List<AttachmentPoint>();
            foreach(AttachmentPoint ap in attachmentPoints)
            {
                //Console.WriteLine("AttName {0} Bone {1}", ap.name, ap.bone);
                attPoints.Add(ap); 
            }
            foreach (Part pt in parts)
            {
                attPoints.AddRange(pt.GetAllAttachmentPoints());
                //foreach (AttachmentPoint app in pt.GetAllAttachmentPoints())
                //{
                //    Console.WriteLine("AttName {0} Bone {1}", app.name, app.bone);
                //    attPoints.Add(app);
                //}
            }
            return attPoints;
        }
    }
    class AttachmentPoint
    {
        public string name;
        public string bone;
    }
}
