using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using CryEngine;
using System.Xml.Linq;
using System.Globalization;
using unforge;

namespace shipsExporter
{
    public class ObjectContainer
    {
        File_ChCr_746 socFile;

        List<Item> itemsList;
        string objectContainersVer = "3_0_0";
        public string SocpakPath { get; }
        public string name;
        string layerGUID ;
        string layer;
        string id ;
        string VisAreaPos;
        public string prefabGUID;
        public string portName;
        public string visible;
        public List<string> childsNames = new List<string>();
        public List<PrefabObject> prefabObjects = new List<PrefabObject>();
        //public List<ObjectContainer> childsOCs = new List<ObjectContainer>();
        public List<Child> childs = new List<Child>();
        List<Objects> objects = new List<Objects>();
        List<AreaShapes> areaShapes = new List<AreaShapes>();
        //AreaShapeChunk areaShapeChunk;
        Random random = new Random();
        string lodRatio = "100";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">socpak archive path</param>
        /// <param name="port">portName</param>
        public ObjectContainer(string path, string port, List<Item> items)
        {
            itemsList = items;
            SocpakPath = path;
            portName = port;
            name = Path.GetFileNameWithoutExtension(path);
            //Console.WriteLine(name);
            if (File.Exists(path))
            {
                LoadSoc(path,false);
                LoadChilds(path,port);
                //foreach(Child child in childs)
                //{
                //    string childname = child.Name;
                //    //Console.WriteLine("childname {0}", childname);
                //    childsOCs.Add(new ObjectContainer(path,port,childname));
                //}
                //Console.WriteLine("childs count {0}",childs.Count);
            }
            
        }
        ObjectContainer(string path, string port, string childname,bool isExternal,string childPath, List<Item> items)
        {
            itemsList = items;
            SocpakPath = path;
            portName = port;
            name = childPath;
            if(isExternal) name= Path.GetFileNameWithoutExtension(childname);
            //Console.WriteLine(name);
            if (File.Exists(path))
            {
                LoadSoc(path, isExternal, childPath);
                if (!isExternal)
                {
                    LoadChilds(path, port);
                }
                else
                {
                    LoadChilds(childPath, port);
                }
            }
        }
        void LoadChilds(string path,string port)
        {
            XDocument xml = new XDocument();
            string xmlstr="";
            if (File.Exists(path))
            {
                using (ZipArchive zip = ZipFile.Open(path, ZipArchiveMode.Read))
                {
                    string entryname = "";
                    //Console.WriteLine("loading child names");
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {

                        if (entry.FullName == name.ToLower() + ".xml")
                        {
                            entryname = entry.FullName;
                        }
                    }
                    if (entryname == "")
                    {
                        Console.WriteLine("LoadChildsNames entryname=null");
                        foreach (ZipArchiveEntry entry in zip.Entries)
                        {
                            Console.WriteLine(entry.FullName);
                        }
                    }
                    Stream streamFile = new MemoryStream();
                    ZipArchiveEntry xmlzip = zip.GetEntry(entryname);
                    xmlzip.Open().CopyTo(streamFile);
                    BinaryReader br = new BinaryReader(streamFile);
                    br.BaseStream.Position = 0;
                    xmlstr = new string(br.ReadChars((int)streamFile.Length)).TrimEnd('\0').TrimEnd(Environment.NewLine.ToCharArray());
                    br.Close();
                }
                TextReader tr = new StringReader(xmlstr);
                xml = XDocument.Load(tr);
                foreach (XElement el in xml.Descendants("Child"))
                {
                    string newChildName = Path.GetFileName(el.Attribute("name").Value);
                    //string newChildName = el.Attribute("name").Value.Replace('\\','/') ;
                    childsNames.Add(newChildName);
                    Child child = new Child();
                    child.Name = newChildName;
                    child.Path = el.Attribute("name").Value.Replace('\\', '/');
                    if (el.Attribute("tags") != null) child.Tags = el.Attribute("tags").Value;
                    if (el.Attribute("guid") != null) child.Guid = el.Attribute("guid").Value;
                    if (el.Attribute("visible") != null) child.Visible = el.Attribute("visible").Value;
                    if (el.Attribute("external") != null) child.External = el.Attribute("external").Value;
                    if (el.Attribute("rot") != null) child.Rot = el.Attribute("rot").Value;
                    if (el.Attribute("pos") != null) child.Pos = el.Attribute("pos").Value;
                    if (el.Attribute("class") != null) child.Class = el.Attribute("class").Value;
                    if (el.Attribute("label") != null) child.Label = el.Attribute("label").Value;

                    child.Soc = new ObjectContainer(path, port, child.Name, child.IsExternal(), child.Path,itemsList);

                    childs.Add(child);
                }
            }
            else
            {
                Console.WriteLine("Not Found: {0}",path);
            }

        }
        public List<PrefabObject> GetPrefabObjectsByAttachmentName(string attName)
        {
            List<PrefabObject> prefObjs = new List<PrefabObject>();
            foreach (PrefabObject po in prefabObjects)
            {
                if (po.AttachmentPointName == attName) prefObjs.Add(po);
            }
            return prefObjs;
        }
        public XElement GetAsPrefab(string libname,string layerid, string prefid,string layername)
        {
            XElement el = new XElement("Prefab");
            el.Add(new XAttribute("Name", "ObjectContainers." + name));
            el.Add(new XAttribute("Library", libname));
            el.Add(new XAttribute("Id", prefid));

            XElement elObjects = new XElement("Objects");
            foreach (PrefabObject po in prefabObjects)
            {
                if (po.EntityClass== "GeomEntity")
                {
                    //Console.WriteLine(po.Geometry);
                    XElement elObject = new XElement("Object");
                    elObject.Add(new XAttribute("Name", po.Name));
                    elObject.Add(new XAttribute("Id", po.Id));
                    elObject.Add(new XAttribute("LayerGUID", po.LayerGUID));
                    elObject.Add(new XAttribute("Geometry", po.Geometry));
                    elObject.Add(new XAttribute("Layer", po.Layer));
                    elObject.Add(new XAttribute("Type", "GeomEntity"));
                    elObject.Add(new XAttribute("EntityClass", "GeomEntity"));
                    elObject.Add(new XAttribute("LodRatio", lodRatio));
                    if (po.Rotate != null) elObject.Add(new XAttribute("Pos", po.Pos));
                    if (po.Rotate != null) elObject.Add(new XAttribute("Rotate", po.Rotate));
                    elObjects.Add(elObject);
                }
                if (po.EntityClass == "Light")
                {
                    XElement elObject = new XElement("Object");
                    elObject.Add(new XAttribute("Name", po.Name));
                    elObject.Add(new XAttribute("Id", po.Id));
                    elObject.Add(new XAttribute("LayerGUID", po.LayerGUID));
                    elObject.Add(new XAttribute("Layer", po.Layer));
                    elObject.Add(new XAttribute("Type", "Entity"));
                    elObject.Add(new XAttribute("EntityClass", "Light")); 
                    elObject.Add(new XAttribute("Pos", po.Pos));
                    if (po.Rotate != null) elObject.Add(new XAttribute("Rotate", po.Rotate));
                    if (po.Scale != null) elObject.Add(new XAttribute("Scale", po.Scale));
                    elObject.Add(new XAttribute("ColorRGB", "65535"));  
                    elObject.Add(po.Properties);
                    elObjects.Add(elObject);
                }
                if (po.EntityClass == "EnvironmentLight")
                {
                    XElement elObject = new XElement("Object");
                    elObject.Add(new XAttribute("Name", po.Name));
                    elObject.Add(new XAttribute("Id", po.Id));
                    elObject.Add(new XAttribute("LayerGUID", po.LayerGUID));
                    elObject.Add(new XAttribute("Layer", po.Layer));
                    elObject.Add(new XAttribute("Type", "Entity"));
                    elObject.Add(new XAttribute("EntityClass", "EnvironmentLight"));
                    elObject.Add(new XAttribute("Pos", po.Pos));
                    if (po.Rotate != null) elObject.Add(new XAttribute("Rotate", po.Rotate));
                    if (po.Scale != null) elObject.Add(new XAttribute("Scale", po.Scale)); 
                    elObject.Add(po.Properties);
                    elObjects.Add(elObject);
                }
                if (po.Type == "VisArea" && po.GeometryContainsPortals == "0")
                {
                    XElement elObject = new XElement("Object");
                    elObject.Add(new XAttribute("Name", po.Name));
                    elObject.Add(new XAttribute("Id", po.Id));
                    elObject.Add(new XAttribute("LayerGUID", layerid));
                    elObject.Add(new XAttribute("Layer", layername));
                    elObject.Add(new XAttribute("Type", "VisArea")); 
                    elObject.Add(new XAttribute("Pos", po.Pos));
                    elObject.Add(new XAttribute("DisplayFilled", po.DisplayFilled));
                    elObject.Add(new XAttribute("Height", po.Height)); 
                    if (po.Rotate != null) elObject.Add(new XAttribute("Rotate", po.Rotate));
                    if (po.Scale != null) elObject.Add(new XAttribute("Scale", po.Scale));
                    elObject.Add(po.Points);
                    elObjects.Add(elObject);
                }
                if (po.Type == "Portal")
                {
                    XElement elObject = new XElement("Object");
                    elObject.Add(new XAttribute("Name", po.Name));
                    elObject.Add(new XAttribute("Id", po.Id));
                    elObject.Add(new XAttribute("LayerGUID", layerid));
                    elObject.Add(new XAttribute("Layer", layername));
                    elObject.Add(new XAttribute("Type", "Portal"));
                    elObject.Add(new XAttribute("Pos", po.Pos));
                    elObject.Add(new XAttribute("DisplayFilled", po.DisplayFilled));
                    elObject.Add(new XAttribute("Height", po.Height));
                    elObject.Add(new XAttribute("LightBlending", po.LightBlending));
                    elObject.Add(new XAttribute("LightBlendValue", po.LightBlendValue));
                    if (po.Rotate != null) elObject.Add(new XAttribute("Rotate", po.Rotate));
                    if (po.Scale != null) elObject.Add(new XAttribute("Scale", po.Scale));
                    elObject.Add(po.Points);
                    elObjects.Add(elObject);
                }
            }
            el.Add(elObjects);

            return el;
        }
        //string DecodeCryXml(byte[] byteArray, int cryXmlHeaderOffset)
        //{
        //    string newXmlStr="";
        //    File.WriteAllBytes("temp", byteArray);
        //    //Console.WriteLine("socpath {0}", socpakPath);
        //    //Console.WriteLine("objcontainer {0}", name);
        //    //Console.WriteLine("cryXmlHeaderOffset {0}", cryXmlHeaderOffset);
        //    var xml = CryXmlSerializer.ReadFile("temp", ByteOrderEnum.AutoDetect, false, cryXmlHeaderOffset);
        //    if (xml != null)
        //    {
        //        newXmlStr = xml.InnerXml;
        //        //Console.WriteLine("newXmlStr size {0}", newXmlStr.Length);
        //        //Console.WriteLine("newXmlStr |{0}|", newXmlStr);
        //    }
        //    else
        //    {
        //        //Console.WriteLine("error in DecodeCryXml");
        //    }
        //    File.WriteAllText("tempDecoded", newXmlStr);
        //    if(!Directory.Exists("logs/objectContainersXmls"))Directory.CreateDirectory("logs/objectContainersXmls");
        //    string dirName = new DirectoryInfo(socpakPath).Parent.Name;
        //    File.WriteAllText("logs/objectContainersXmls/"+ dirName+"_"+ Path.GetFileNameWithoutExtension( name)+".xml", newXmlStr);
        //    return newXmlStr;
        //}
        //void ExtractGeomEntityCryXml(byte[] byteArray, int cryXmlHeaderOffset = 0)
        //{
        //    ExtractGeomEntity(DecodeCryXml(byteArray, cryXmlHeaderOffset));
        //}
        //void ExtractLightEntityCryXml(byte[] byteArray, int cryXmlHeaderOffset = 0)
        //{
        //    ExtractLightEntity(DecodeCryXml(byteArray, cryXmlHeaderOffset));
        //}
        

        /// <summary>
        /// loading object container from socpak
        /// </summary>
        /// <param name="path">socpak path</param>
        void LoadSoc(string path,bool isExternal,string childPath="")
        {
            Stream streamFile = new MemoryStream();
            string zipPakPath = path;
            if (isExternal) zipPakPath = childPath;
            if (File.Exists(zipPakPath))
            {
                using (ZipArchive zip = ZipFile.Open(zipPakPath, ZipArchiveMode.Read))
                {
                    //Console.WriteLine(path);
                    string entryname = "";
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        //Console.WriteLine("LoadSoc entry.FullName {0}", entry.FullName);
                        if (entry.FullName == name.ToLower() + ".soc")
                            entryname = entry.FullName;
                    }
                    if (entryname == "")
                    {
                        Console.WriteLine("entryname=null");
                        foreach (ZipArchiveEntry entry in zip.Entries)
                        {
                            Console.WriteLine(entry.FullName);
                        }
                    }
                    byte[] asciiBytes = Encoding.ASCII.GetBytes(entryname);
                    layerGUID = Guid.NewGuid().ToString();
                    prefabGUID = Guid.NewGuid().ToString();
                    //Console.WriteLine("name {0}", name);
                    //Console.WriteLine("entryname {0}",entryname);
                    //ZipArchiveEntry socfile = zip.GetEntry(entryname);
                    ZipArchiveEntry socfile = zip.GetEntry(entryname);
                    socfile.Open().CopyTo(streamFile);
                    streamFile.Position = 0;
                    //Console.WriteLine(socfile.Length);
                }
                //Console.WriteLine(streamFile.Length);


                ////////////////////////
                //layerGUID = "1" + RandomString(5);
                //prefabGUID = "1" + RandomString(5);
                id = "1" + RandomString(5);

                ////////////////////////  
                socFile = new File_ChCr_746(streamFile, name.ToLower() + ".soc");

                for (int i = 0; i < socFile.chunks.Count; i++)
                {
                    switch (socFile.chunks[i].type)
                    {
                        //objects chunk
                        case (ushort)Chunk_Objects.type:
                            Chunk_Objects chunkObjects = new Chunk_Objects(socFile.chunks[i].content);
                            objects.Add(new Objects(chunkObjects.objectsStream, chunkObjects.GetFilePaths()));
                            break;
                        case (ushort)Chunk_AreaShape.type:
                            Chunk_AreaShape chunkAreaShape = new Chunk_AreaShape(socFile.chunks[i].content);
                            areaShapes.Add(new AreaShapes(chunkAreaShape.areaShapesStream, chunkAreaShape.GetVisAreasCount(), chunkAreaShape.GetPortalsCount()));
                            break;
                        case (ushort)Chunk_XML.type:
                            Chunk_XML chunkXML = new Chunk_XML(socFile.chunks[i].content);
                            SCOC_Entities SCOCEntities = new SCOC_Entities(chunkXML.xmlStream);
                            string xmlSCOCEntities = SCOCEntities.GetXML();
                            ExtractSCOCEntities(xmlSCOCEntities);
                            //save soc xml chunk
                            string folder = Path.GetFileName(Path.GetDirectoryName(path));
                            Directory.CreateDirectory("logs/objectContainersXmls/");
                            File.WriteAllText("logs/objectContainersXmls/" + folder + "-" + name.ToLower().Replace(@"/", "-") + ".xml", xmlSCOCEntities);
                            break;
                        default:
                            break;
                    }
                }
                MakePrefabObjects();
            }
            else
            {
                Console.WriteLine("Not Found {0}", zipPakPath);
            }
        }
        void ExtractSCOCEntities(string xmlSCOCEntities)
        {
            ExtractGeomEntity(xmlSCOCEntities);
            ExtractLightEntity(xmlSCOCEntities);
            ExtractEnvironmentLightEntity(xmlSCOCEntities);
            //ExtractVisAreaEntity(xmlSCOCEntities);
            //ExtractPortalEntity(xmlSCOCEntities);
        }
        void ExtractGeomEntity(string xmlstr)
        {
            TextReader tr = new StringReader(xmlstr);
            XDocument xml = XDocument.Load(tr);
            XElement elSCOC_Entities = xml.Root;
            
            foreach(XElement elEntity in elSCOC_Entities.Elements("Entity"))
            {
                SCOC_Entity scocEntity = new SCOC_Entity(elEntity);
                PrefabObject prefObj = scocEntity.GetGeomAsPrefabObject(itemsList);
                if(prefObj!=null) prefabObjects.Add(prefObj);
            }
        }
        /// <summary>
        /// Exctract GeomEntity objects from XML chunk
        /// </summary>
        /// <param name="xml"></param>
        void ExtractGeomEntityOLD(string xmlstr)
        {
            //try
            //{
            if (xmlstr.Length > 0)
            {
                //bool decoded = false;
                //string label = xmlstr.Substring(0,6);
                //Console.WriteLine("label {0}", label);
                //if (label != "CryXml") decoded = true; 
                //if(!decoded)
                //{
                //    xmlstr = DecodeCryXml(xmlstr, cryXmlHeaderOffset);
                //    decoded = true;
                //}
                //if (decoded)
                //{
                TextReader tr = new StringReader(xmlstr);
                XDocument xml = XDocument.Load(tr);
                string layerguid = Guid.NewGuid().ToString();
                foreach (XElement el in xml.Descendants("Entity"))
                {
                    if (el.Attribute("EntityClass") != null) if (el.Attribute("EntityClass").Value == "GeomEntity")
                        {
                            PrefabObject prObj = new PrefabObject();
                            prObj.Name = el.Attribute("Name").Value;
                            //Console.WriteLine(prObj.Name);
                            //if(prObj.Name=="lmp_emergency_a-071")
                            //{
                            //    throw new System.ArgumentException("lmp_emergency_a-071", "original");
                            //}
                            if (el.Attribute("Pos") != null) prObj.Pos = el.Attribute("Pos").Value;
                            if (el.Attribute("Rotate") != null)
                            {
                                prObj.Rotate = el.Attribute("Rotate").Value;
                                // Quaternion quat = new Quaternion( prObj.GetRotate().y, prObj.GetRotate().z, prObj.GetRotate().w, prObj.GetRotate().x);
                                //quat =ValidateQuaternion(quat);
                                // Vector4 quatV = new Vector4(quat.x, quat.y, quat.z, quat.w);
                                // prObj.SetRotate(quatV);
                            }
                            if (el.Attribute("Scale") != null) prObj.Scale = el.Attribute("Scale").Value;
                            prObj.EntityClass = el.Attribute("EntityClass").Value;
                            prObj.Type = el.Attribute("EntityClass").Value;
                            prObj.Id = GuidUtility.GenID("sdfsdf3424" + prObj.Name + prObj.EntityClass);
                            prObj.Layer = el.Attribute("Layer").Value;
                            if (el.Attribute("Geometry") != null)
                            {
                                prObj.Geometry = el.Attribute("Geometry").Value;
                            }
                            else
                            {
                                prObj.Geometry = el.Element("PropertiesDataCore").Element("EntityGeometryResource").Element("Geometry").Element("Geometry").Element("Geometry").Attribute("path").Value;
                            }
                            byte[] asciiBytes = Encoding.ASCII.GetBytes(el.Attribute("Layer").Value);
                            prObj.LayerGUID = layerguid;
                            prObj.LodRatio = lodRatio;
                            prefabObjects.Add(prObj);
                        }
                }
                //} 
            }
            //}
            //catch(Exception e)
            //{
            //    Console.WriteLine("|{0}|",xmlstr);
            //    Console.Read();
            //    throw;
            //}
        }
        void ExtractLightEntity(string xmlstr)
        {

            if (xmlstr.Length > 0)
            {
                TextReader tr = new StringReader(xmlstr);
                XDocument xml = XDocument.Load(tr);
                string layerguid = GuidUtility.GenID("Main");
                foreach (XElement el in xml.Descendants("Entity"))
                {
                    if (el.Attribute("EntityClass") != null) if (el.Attribute("EntityClass").Value == "Light")
                        {
                            PrefabObject prObj = new PrefabObject();
                            prObj.Name = el.Attribute("Name").Value;
                            if (el.Attribute("Pos") != null)
                            {
                                prObj.Pos = el.Attribute("Pos").Value;
                            }
                            else
                            {
                                prObj.Pos = "0,0,0";
                            }
                            if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
                            if (el.Attribute("Scale") != null) prObj.Scale = el.Attribute("Scale").Value;
                            prObj.EntityClass = el.Attribute("EntityClass").Value;
                            prObj.Type = el.Attribute("EntityClass").Value;
                            prObj.Id = GuidUtility.GenID("sfdf4rgds" + prObj.Name); ;
                            prObj.Layer = el.Attribute("Layer").Value;
                            byte[] asciiBytes = Encoding.ASCII.GetBytes(el.Attribute("Layer").Value);
                            prObj.LayerGUID = layerguid;
                            prObj.Properties = el.Element("Properties");
                            prefabObjects.Add(prObj);
                        }
                    if (el.Attribute("EntityClass") != null) if (el.Attribute("EntityClass").Value == "LightGroup")
                        {
                            PrefabObject prObj = new PrefabObject();
                            prObj.lights = new List<Light>();
                            prObj.Name = el.Attribute("Name").Value;
                            if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
                            if (el.Attribute("Pos") != null) prObj.Pos = el.Attribute("Pos").Value;
                            if (el.Attribute("Scale") != null) prObj.Scale = el.Attribute("Scale").Value;
                            prObj.EntityClass = "LightGroup";
                            prObj.Layer = el.Attribute("Layer").Value;
                            prObj.Id = GuidUtility.GenID(prObj.Name + "dfgjhdkjf");
                            int i = 0;
                            foreach (XElement ell in el.Descendants("Light"))
                            {
                                XElement relativeXForm = ell.Element("RelativeXForm");
                                Light lightEnt = new Light();
                                lightEnt.Name = el.Attribute("Name").Value;
                                if (relativeXForm.Attribute("translation") != null) lightEnt.translation = relativeXForm.Attribute("translation").Value;
                                if (relativeXForm.Attribute("rotation") != null) lightEnt.rotation = relativeXForm.Attribute("rotation").Value;
                                if (el.Attribute("Scale") != null) lightEnt.Scale = el.Attribute("Scale").Value;
                                lightEnt.EntityClass = "Light";
                                lightEnt.Type = "Entity";
                                lightEnt.Id = GuidUtility.GenID();
                                lightEnt.Layer = el.Attribute("Layer").Value;
                                lightEnt.LayerGUID = layerguid;
                                //lightEnt.AttachmentPointName = el.Attribute("Layer").Value; ;
                                lightEnt.Properties = ell.Element("Properties");
                                prObj.lights.Add(lightEnt);
                                i++;
                            }
                            i = 0;
                            foreach (XElement ell in el.Descendants("EnvironmentProbe"))
                            {
                                XElement relativeXForm = ell.Element("RelativeXForm");
                                Light lightEnt = new Light();
                                lightEnt.Name = el.Attribute("Name").Value;
                                if (relativeXForm.Attribute("translation") != null) lightEnt.translation = relativeXForm.Attribute("translation").Value;
                                if (relativeXForm.Attribute("rotation") != null) lightEnt.rotation = relativeXForm.Attribute("rotation").Value;
                                if (el.Attribute("Scale") != null) lightEnt.Scale = el.Attribute("Scale").Value;
                                lightEnt.EntityClass = "EnvironmentLight";
                                lightEnt.Type = "Entity";
                                lightEnt.Id = GuidUtility.GenID();
                                lightEnt.Layer = el.Attribute("Layer").Value;
                                lightEnt.LayerGUID = layerguid;
                                //lightEnt.AttachmentPointName = el.Attribute("Layer").Value; ;
                                lightEnt.Properties = ell.Element("Properties");
                                prObj.lights.Add(lightEnt);
                                i++;
                            }
                            prefabObjects.Add(prObj);
                        }
                }
            }

        }
        void ExtractEnvironmentLightEntity(string xmlstr)
        {
            try
            {
                if (xmlstr.Length > 0)
                {
                    TextReader tr = new StringReader(xmlstr);
                    XDocument xml = XDocument.Load(tr);
                    string layerguid = GuidUtility.GenID("Main");
                    foreach (XElement el in xml.Descendants("Entity"))
                    {
                        if (el.Attribute("EntityClass") != null) if (el.Attribute("EntityClass").Value == "EnvironmentLight")
                            {
                                PrefabObject prObj = new PrefabObject();
                                prObj.Name = el.Attribute("Name").Value;
                                if (el.Attribute("Pos") != null) prObj.Pos = el.Attribute("Pos").Value;
                                if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
                                if (el.Attribute("Scale") != null) prObj.Scale = el.Attribute("Scale").Value;
                                prObj.EntityClass = el.Attribute("EntityClass").Value;
                                prObj.Type = el.Attribute("EntityClass").Value;
                                prObj.Id = GuidUtility.GenID("dfgdfg453" + prObj.Name + prObj.Pos + prObj.EntityClass);
                                prObj.Layer = el.Attribute("Layer").Value;
                                byte[] asciiBytes = Encoding.ASCII.GetBytes(el.Attribute("Layer").Value);
                                prObj.LayerGUID = layerguid;
                                prObj.Properties = el.Element("Properties");
                                prefabObjects.Add(prObj);
                            }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("|{0}|", xmlstr);
                Console.WriteLine("Press Enter to continue...");
                Console.Read();
                //throw;
            }
        }
        void ExtractVisAreaEntity(string xmlstr)
        {
            try
            {
                if (xmlstr.Length > 0)
                {
                    TextReader tr = new StringReader(xmlstr);
                    XDocument xml = XDocument.Load(tr);
                    foreach (XElement el in xml.Descendants("Object"))
                    {
                        if (el.Attribute("Type") != null) if (el.Attribute("Type").Value == "VisArea")
                            {
                                //Console.WriteLine("sdf");
                                PrefabObject prObj = new PrefabObject();
                                prObj.Name = el.Attribute("Name").Value;
                                prObj.Pos = el.Attribute("Pos").Value;
                                //prObj.Pos = "0,0,0";
                                if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
                                if (el.Attribute("Scale") != null) prObj.Scale = el.Attribute("Scale").Value;
                                if (el.Attribute("GeometryFile") != null) prObj.GeometryFile = el.Attribute("GeometryFile").Value;
                                if (el.Attribute("GeometryContainsPortals") != null) prObj.GeometryContainsPortals = el.Attribute("GeometryContainsPortals").Value;
                                prObj.Type = el.Attribute("Type").Value;
                                prObj.Id = GuidUtility.GenID("dfghk55ggu" + prObj.Name + prObj.Pos);
                                prObj.Points = el.Element("Points");
                                prObj.DisplayFilled = el.Attribute("DisplayFilled").Value;
                                prObj.Height = el.Attribute("Height").Value;
                                prObj.VisAreaPos = el.Attribute("Pos").Value;
                                VisAreaPos = el.Attribute("Pos").Value;
                                prObj.FixPoints();
                                PrefabObject prObjFromChunk = areaShapes[0].GetPrefabObjByName(el.Attribute("Name").Value);
                                foreach (AreaShapes arShape in areaShapes)
                                {
                                    prObjFromChunk = arShape.GetPrefabObjByName(el.Attribute("Name").Value);
                                }
                                if (prObjFromChunk.Name != null)
                                {
                                    prObj.Pos = prObjFromChunk.Pos;
                                    prObj.Rotate = prObjFromChunk.Rotate;
                                    prObj.Height = prObjFromChunk.Height;
                                    prObj.Points = prObjFromChunk.Points;
                                }
                                prefabObjects.Add(prObj);
                            }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("|{0}|", xmlstr);
                Console.WriteLine(name);
                throw;
            }
        }
        void ExtractPortalEntity(string xmlstr)
        {
            try
            {
                if (xmlstr.Length > 0)
                {
                    TextReader tr = new StringReader(xmlstr);
                    XDocument xml = XDocument.Load(tr);
                    foreach (XElement el in xml.Descendants("Object"))
                    {
                        if (el.Attribute("Type") != null) if (el.Attribute("Type").Value == "Portal")
                            {
                                PrefabObject prObj = new PrefabObject();
                                prObj.Name = el.Attribute("Name").Value;
                                prObj.Pos = el.Attribute("Pos").Value;
                                //prObj.Pos = "0,0,0";
                                //if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
                                if (el.Attribute("Rotate") != null) prObj.Rotate = "1,0,0,0";
                                if (el.Attribute("Scale") != null) prObj.Scale = el.Attribute("Scale").Value;
                                prObj.Type = el.Attribute("Type").Value;
                                prObj.Id = GuidUtility.GenID("dfghk55ggu" + prObj.Name + prObj.Pos + prObj.Type);
                                prObj.Points = el.Element("Points");
                                prObj.DisplayFilled = el.Attribute("DisplayFilled").Value;
                                prObj.Height = el.Attribute("Height").Value;
                                prObj.LightBlending = el.Attribute("LightBlending").Value;
                                prObj.LightBlendValue = el.Attribute("LightBlendValue").Value;
                                if (VisAreaPos != null) prObj.VisAreaPos = VisAreaPos; else { Console.WriteLine(SocpakPath); Console.WriteLine(el.Attribute("Name").Value); };
                                if (name == "frnt") Console.WriteLine(VisAreaPos);
                                prObj.FixPoints();
                                PrefabObject prObjFromChunk = areaShapes[0].GetPrefabObjByName(el.Attribute("Name").Value);
                                foreach (AreaShapes arShape in areaShapes)
                                {
                                    prObjFromChunk = arShape.GetPrefabObjByName(el.Attribute("Name").Value);
                                }
                                if (prObjFromChunk.Name != null)
                                {
                                    prObj.Pos = prObjFromChunk.Pos;
                                    prObj.Rotate = prObjFromChunk.Rotate;
                                    prObj.Height = prObjFromChunk.Height;
                                    prObj.Points = prObjFromChunk.Points;
                                }
                                prefabObjects.Add(prObj);
                            }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("|{0}|", xmlstr);
                Console.WriteLine(name);
                Console.WriteLine(SocpakPath);
                throw;
            }
        }
        void MakePrefabObjects()
        {
            foreach(Objects obj in objects)
            { 
                foreach(Object_Type1 geomObj in obj.GetObject_Type1())
                {
                    PrefabObject prefabObj = new PrefabObject();

                    //////////////////////////
                    //Console.WriteLine(br.BaseStream.Position); 
                    double x = geomObj.pos.x;
                    double y = geomObj.pos.y;
                    double z = geomObj.pos.z; 
                    int id = geomObj.id;
                    Quaternion quat = geomObj.quaternion;
                    Vector3 scale = geomObj.scale;
                    prefabObj.Transform34 = geomObj.rotMatrix34;
                    //quat = ValidateQuaternion(quat);
                    string objectname = Path.GetFileNameWithoutExtension(geomObj.path);
                    prefabObj.Name = objectname;
                    prefabObj.Geometry = geomObj.path;
                    prefabObj.Pos = (float)x + "," + (float)y + "," + (float)z;
                    prefabObj.Rotate = quat.W + "," + quat.X + "," + quat.Y + "," + quat.Z;
                    prefabObj.Scale = scale.X + "," + scale.Y + "," + scale.Z;
                    //Console.WriteLine(prefabObj.Geometry);
                    prefabObj.EntityClass = "GeomEntity";
                    byte[] asciiBytes = Encoding.ASCII.GetBytes(objectname);
                    prefabObj.Layer = objectname;
                    //prefabObj.LayerGUID = "3675a837-e15f-dfa0-db3a-"+ bytesToString(asciiBytes);
                    prefabObj.LayerGUID = GuidUtility.GenLayerID("sdf33" + objectname + prefabObj.Geometry + prefabObj.Pos);
                    prefabObj.Id = GuidUtility.GenPrefabID("sdf34gffg" + objectname + prefabObj.Geometry + prefabObj.Pos);
                    prefabObjects.Add(prefabObj);
                }
            }
            //foreach (AreaShapes areaShape in areaShapes)
            //{
            //    foreach (PrefabObject prefabObj in areaShape.GetPrefabObjects())
            //    {
            //        prefabObjects.Add(prefabObj);
            //    }
            //}
        }
        Quaternion ValidateQuaternion(Quaternion quat)
        {
            Quaternion newQuat=new Quaternion(0,0,0,1);
            if (Math.Abs(1 - quat.LengthSquared) < 0.05) newQuat = quat;
            return newQuat;
        }
        string ConvertBytesToString(byte[] data)
        {
            char[] characters = data.Select(b => (char)b).ToArray();
            Console.WriteLine(new string(characters));
            return new string(characters);
            
        }
        string RandomString(int length)
        {
            const string chars = "123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        string bytesToString(byte[] bytes)
        {
            string newString = "";
            foreach (byte bt in bytes)
            {
                newString += (bt/16);
            }
            return newString;
        }
        string stringToNumbers(byte[] bytes)
        {
            string newString = "";
            foreach (byte bt in bytes)
            {
                newString += (bt / 16);
            }
            return newString;
        }
        static bool IsDecoded(BinaryReader br)
        {
            bool decoded = false; 
            string fileLabel = new string(br.ReadChars(6));
            if (fileLabel != "CryXml") decoded = true; 

            return decoded;
        }
    }
    public class Child
    {
        bool _visible=true;
        bool _external=false;
        public ObjectContainer Soc { get; set; }
        public string Tags { get; set; }
        public string Name { get; set; }
        public string Guid { get; set; }
        public string Visible {
            get
            {
                if (_visible) return "1";
                else return "0"; 
            }
            set
            {
                if (value == "1") _visible = true;
                else _visible = false;
            }
        } 
        public string External {
            get
            {
                if (_external) return "1";
                else return "0";
            }
            set
            {
                if (value == "1") _external = true;
                else _external = false;
            }
        }
        public string Rot { get; set; }
        public string Pos { get; set; }
        public string Class { get; set; }
        public string Label { get; set; }

        public string Path { get; set; }

        public bool IsExternal() { return _external; }


        /// <summary>
        /// root for objects from Soc
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="layerid"></param>
        /// <param name="layerName"></param>
        /// <param name="portName"></param>
        /// <returns></returns>
        public XElement GetAsFlowgraphEntity(string parentid, string layerid, string layerName, string portName)
        {
            XElement obj1 = new XElement("Object");

            obj1.Add(new XAttribute("Name", Name));
            obj1.Add(new XAttribute("LinkedTo", parentid));
            obj1.Add(new XAttribute("Id", Guid));
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
            if (Rot != null) obj1.Add(new XAttribute("Rotate", Rot));
            obj1.Add(new XAttribute("Type", "Entity"));
            obj1.Add(new XAttribute("EntityClass", "FlowgraphEntity"));
            if (portName!="")
            {
                obj1.Add(new XAttribute("AttachmentType", "CharacterBone"));
                obj1.Add(new XAttribute("AttachmentTarget", portName));
            }
            return obj1;
        }
    }
}
