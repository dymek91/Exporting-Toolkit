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
    //class OLD_ObjectContainer
    //{
    //    File_ChCr_746 socFile;

    //    string objectContainersVer = "3_0_0";
    //    string socpakPath;
    //    public string name;
    //    string layerGUID;
    //    string layer;
    //    string id;
    //    string VisAreaPos;
    //    public string prefabGUID;
    //    public string portName;
    //    public string visible;
    //    public List<string> childsNames = new List<string>();
    //    public List<PrefabObject> prefabObjects = new List<PrefabObject>();
    //    public List<ObjectContainer> childs = new List<ObjectContainer>();
    //    public List<Objects> objects = new List<Objects>();
    //    AreaShapeChunk areaShapeChunk;
    //    Random random = new Random();
    //    string lodRatio = "100";

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="path">socpak archive path</param>
    //    /// <param name="port">portName</param>
    //    public ObjectContainer(string path, string port)
    //    {
    //        socpakPath = path;
    //        portName = port;
    //        name = Path.GetFileNameWithoutExtension(path);
    //        //Console.WriteLine(name);
    //        if (File.Exists(path))
    //        {
    //            LoadSoc(path);
    //            LoadChildsNames(path);
    //            foreach (string childname in childsNames)
    //            {
    //                //Console.WriteLine("childname {0}", childname);
    //                childs.Add(new ObjectContainer(path, port, childname));
    //            }
    //            //Console.WriteLine("childs count {0}",childs.Count);
    //        }

    //    }
    //    ObjectContainer(string path, string port, string childname)
    //    {
    //        socpakPath = path;
    //        portName = port;
    //        name = childname;
    //        //Console.WriteLine(name);
    //        if (File.Exists(path))
    //        {
    //            LoadSoc(path);
    //            LoadChildsNames(path);
    //        }
    //    }
    //    void LoadChildsNames(string path)
    //    {
    //        XDocument xml = new XDocument();
    //        string xmlstr = "";
    //        using (ZipArchive zip = ZipFile.Open(path, ZipArchiveMode.Read))
    //        {
    //            string entryname = "";
    //            //Console.WriteLine("loading child names");
    //            foreach (ZipArchiveEntry entry in zip.Entries)
    //            {

    //                if (entry.FullName == name.ToLower() + ".xml")
    //                {
    //                    entryname = entry.FullName;
    //                }
    //            }
    //            if (entryname == "")
    //            {
    //                Console.WriteLine("LoadChildsNames entryname=null");
    //                foreach (ZipArchiveEntry entry in zip.Entries)
    //                {
    //                    Console.WriteLine(entry.FullName);
    //                }
    //            }
    //            Stream streamFile = new MemoryStream();
    //            ZipArchiveEntry xmlzip = zip.GetEntry(entryname);
    //            xmlzip.Open().CopyTo(streamFile);
    //            BinaryReader br = new BinaryReader(streamFile);
    //            br.BaseStream.Position = 0;
    //            xmlstr = new string(br.ReadChars((int)streamFile.Length)).TrimEnd('\0').TrimEnd(Environment.NewLine.ToCharArray());
    //            br.Close();
    //        }
    //        TextReader tr = new StringReader(xmlstr);
    //        xml = XDocument.Load(tr);
    //        foreach (XElement el in xml.Descendants("Child"))
    //        {
    //            //string newChildName = Path.GetFileName(el.Attribute("name").Value);
    //            string newChildName = el.Attribute("name").Value.Replace('\\', '/');
    //            childsNames.Add(newChildName);
    //            // Console.WriteLine("child {0}", newChildName);
    //        }

    //    }
    //    public List<PrefabObject> GetPrefabObjectsByAttachmentName(string attName)
    //    {
    //        List<PrefabObject> prefObjs = new List<PrefabObject>();
    //        foreach (PrefabObject po in prefabObjects)
    //        {
    //            if (po.AttachmentPointName == attName) prefObjs.Add(po);
    //        }
    //        return prefObjs;
    //    }
    //    public XElement getAsPrefab(string libname, string layerid, string prefid, string layername)
    //    {
    //        XElement el = new XElement("Prefab");
    //        el.Add(new XAttribute("Name", "ObjectContainers." + name));
    //        el.Add(new XAttribute("Library", libname));
    //        el.Add(new XAttribute("Id", prefid));

    //        XElement elObjects = new XElement("Objects");
    //        foreach (PrefabObject po in prefabObjects)
    //        {
    //            if (po.EntityClass == "GeomEntity")
    //            {
    //                //Console.WriteLine(po.Geometry);
    //                XElement elObject = new XElement("Object");
    //                elObject.Add(new XAttribute("Name", po.Name));
    //                elObject.Add(new XAttribute("Id", po.Id));
    //                elObject.Add(new XAttribute("LayerGUID", po.LayerGUID));
    //                elObject.Add(new XAttribute("Geometry", po.Geometry));
    //                elObject.Add(new XAttribute("Layer", po.Layer));
    //                elObject.Add(new XAttribute("Type", "GeomEntity"));
    //                elObject.Add(new XAttribute("EntityClass", "GeomEntity"));
    //                elObject.Add(new XAttribute("LodRatio", lodRatio));
    //                elObject.Add(new XAttribute("Pos", po.Pos));
    //                if (po.Rotate != null) elObject.Add(new XAttribute("Rotate", po.Rotate));
    //                elObjects.Add(elObject);
    //            }
    //            if (po.EntityClass == "Light")
    //            {
    //                XElement elObject = new XElement("Object");
    //                elObject.Add(new XAttribute("Name", po.Name));
    //                elObject.Add(new XAttribute("Id", po.Id));
    //                elObject.Add(new XAttribute("LayerGUID", po.LayerGUID));
    //                elObject.Add(new XAttribute("Layer", po.Layer));
    //                elObject.Add(new XAttribute("Type", "Entity"));
    //                elObject.Add(new XAttribute("EntityClass", "Light"));
    //                elObject.Add(new XAttribute("Pos", po.Pos));
    //                if (po.Rotate != null) elObject.Add(new XAttribute("Rotate", po.Rotate));
    //                if (po.Scale != null) elObject.Add(new XAttribute("Scale", po.Scale));
    //                elObject.Add(new XAttribute("ColorRGB", "65535"));
    //                elObject.Add(po.Properties);
    //                elObjects.Add(elObject);
    //            }
    //            if (po.EntityClass == "EnvironmentLight")
    //            {
    //                XElement elObject = new XElement("Object");
    //                elObject.Add(new XAttribute("Name", po.Name));
    //                elObject.Add(new XAttribute("Id", po.Id));
    //                elObject.Add(new XAttribute("LayerGUID", po.LayerGUID));
    //                elObject.Add(new XAttribute("Layer", po.Layer));
    //                elObject.Add(new XAttribute("Type", "Entity"));
    //                elObject.Add(new XAttribute("EntityClass", "EnvironmentLight"));
    //                elObject.Add(new XAttribute("Pos", po.Pos));
    //                if (po.Rotate != null) elObject.Add(new XAttribute("Rotate", po.Rotate));
    //                if (po.Scale != null) elObject.Add(new XAttribute("Scale", po.Scale));
    //                elObject.Add(po.Properties);
    //                elObjects.Add(elObject);
    //            }
    //            if (po.Type == "VisArea" && po.GeometryContainsPortals == "0")
    //            {
    //                XElement elObject = new XElement("Object");
    //                elObject.Add(new XAttribute("Name", po.Name));
    //                elObject.Add(new XAttribute("Id", po.Id));
    //                elObject.Add(new XAttribute("LayerGUID", layerid));
    //                elObject.Add(new XAttribute("Layer", layername));
    //                elObject.Add(new XAttribute("Type", "VisArea"));
    //                elObject.Add(new XAttribute("Pos", po.Pos));
    //                elObject.Add(new XAttribute("DisplayFilled", po.DisplayFilled));
    //                elObject.Add(new XAttribute("Height", po.Height));
    //                if (po.Rotate != null) elObject.Add(new XAttribute("Rotate", po.Rotate));
    //                if (po.Scale != null) elObject.Add(new XAttribute("Scale", po.Scale));
    //                elObject.Add(po.Points);
    //                elObjects.Add(elObject);
    //            }
    //            if (po.Type == "Portal")
    //            {
    //                XElement elObject = new XElement("Object");
    //                elObject.Add(new XAttribute("Name", po.Name));
    //                elObject.Add(new XAttribute("Id", po.Id));
    //                elObject.Add(new XAttribute("LayerGUID", layerid));
    //                elObject.Add(new XAttribute("Layer", layername));
    //                elObject.Add(new XAttribute("Type", "Portal"));
    //                elObject.Add(new XAttribute("Pos", po.Pos));
    //                elObject.Add(new XAttribute("DisplayFilled", po.DisplayFilled));
    //                elObject.Add(new XAttribute("Height", po.Height));
    //                elObject.Add(new XAttribute("LightBlending", po.LightBlending));
    //                elObject.Add(new XAttribute("LightBlendValue", po.LightBlendValue));
    //                if (po.Rotate != null) elObject.Add(new XAttribute("Rotate", po.Rotate));
    //                if (po.Scale != null) elObject.Add(new XAttribute("Scale", po.Scale));
    //                elObject.Add(po.Points);
    //                elObjects.Add(elObject);
    //            }
    //        }
    //        el.Add(elObjects);

    //        return el;
    //    }
    //    string DecodeCryXml(byte[] byteArray, int cryXmlHeaderOffset)
    //    {
    //        string newXmlStr = "";
    //        File.WriteAllBytes("temp", byteArray);
    //        //Console.WriteLine("socpath {0}", socpakPath);
    //        //Console.WriteLine("objcontainer {0}", name);
    //        //Console.WriteLine("cryXmlHeaderOffset {0}", cryXmlHeaderOffset);
    //        var xml = CryXmlSerializer.ReadFile("temp", ByteOrderEnum.AutoDetect, false, cryXmlHeaderOffset);
    //        if (xml != null)
    //        {
    //            newXmlStr = xml.InnerXml;
    //            //Console.WriteLine("newXmlStr size {0}", newXmlStr.Length);
    //            //Console.WriteLine("newXmlStr |{0}|", newXmlStr);
    //        }
    //        else
    //        {
    //            //Console.WriteLine("error in DecodeCryXml");
    //        }
    //        File.WriteAllText("tempDecoded", newXmlStr);
    //        if (!Directory.Exists("logs/objectContainersXmls")) Directory.CreateDirectory("logs/objectContainersXmls");
    //        string dirName = new DirectoryInfo(socpakPath).Parent.Name;
    //        File.WriteAllText("logs/objectContainersXmls/" + dirName + "_" + Path.GetFileNameWithoutExtension(name) + ".xml", newXmlStr);
    //        return newXmlStr;
    //    }
    //    void extractGeomEntityCryXml(byte[] byteArray, int cryXmlHeaderOffset = 0)
    //    {
    //        extractGeomEntity(DecodeCryXml(byteArray, cryXmlHeaderOffset));
    //    }
    //    void extractLightEntityCryXml(byte[] byteArray, int cryXmlHeaderOffset = 0)
    //    {
    //        extractLightEntity(DecodeCryXml(byteArray, cryXmlHeaderOffset));
    //    }
    //    /// <summary>
    //    /// Exctract GeomEntity objects from XML chunk
    //    /// </summary>
    //    /// <param name="xml"></param>
    //    void extractGeomEntity(string xmlstr)
    //    {
    //        //try
    //        //{
    //        if (xmlstr.Length > 0)
    //        {
    //            //bool decoded = false;
    //            //string label = xmlstr.Substring(0,6);
    //            //Console.WriteLine("label {0}", label);
    //            //if (label != "CryXml") decoded = true; 
    //            //if(!decoded)
    //            //{
    //            //    xmlstr = DecodeCryXml(xmlstr, cryXmlHeaderOffset);
    //            //    decoded = true;
    //            //}
    //            //if (decoded)
    //            //{
    //            TextReader tr = new StringReader(xmlstr);
    //            XDocument xml = XDocument.Load(tr);
    //            string layerguid = Guid.NewGuid().ToString();
    //            foreach (XElement el in xml.Descendants("Entity"))
    //            {
    //                if (el.Attribute("EntityClass") != null) if (el.Attribute("EntityClass").Value == "GeomEntity")
    //                    {
    //                        PrefabObject prObj = new PrefabObject();
    //                        prObj.Name = el.Attribute("Name").Value;
    //                        //Console.WriteLine(prObj.Name);
    //                        if (el.Attribute("Pos") != null) prObj.Pos = el.Attribute("Pos").Value;
    //                        if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
    //                        prObj.EntityClass = el.Attribute("EntityClass").Value;
    //                        prObj.Type = el.Attribute("EntityClass").Value;
    //                        prObj.Id = GuidUtility.genID("sdfsdf3424" + prObj.Name + prObj.EntityClass);
    //                        prObj.Layer = el.Attribute("Layer").Value;
    //                        if (el.Attribute("Geometry") != null)
    //                        {
    //                            prObj.Geometry = el.Attribute("Geometry").Value;
    //                        }
    //                        else
    //                        {
    //                            prObj.Geometry = el.Element("PropertiesDataCore").Element("EntityGeometryResource").Element("Geometry").Element("Geometry").Element("Geometry").Attribute("path").Value;
    //                        }
    //                        byte[] asciiBytes = Encoding.ASCII.GetBytes(el.Attribute("Layer").Value);
    //                        prObj.LayerGUID = layerguid;
    //                        prObj.LodRatio = lodRatio;
    //                        prefabObjects.Add(prObj);
    //                    }
    //            }
    //            //} 
    //        }
    //        //}
    //        //catch(Exception e)
    //        //{
    //        //    Console.WriteLine("|{0}|",xmlstr);
    //        //    Console.Read();
    //        //    throw;
    //        //}
    //    }
    //    void extractLightEntity(string xmlstr)
    //    {

    //        if (xmlstr.Length > 0)
    //        {
    //            TextReader tr = new StringReader(xmlstr);
    //            XDocument xml = XDocument.Load(tr);
    //            string layerguid = GuidUtility.genID("Main");
    //            foreach (XElement el in xml.Descendants("Entity"))
    //            {
    //                if (el.Attribute("EntityClass") != null) if (el.Attribute("EntityClass").Value == "Light")
    //                    {
    //                        PrefabObject prObj = new PrefabObject();
    //                        prObj.Name = el.Attribute("Name").Value;
    //                        prObj.Pos = el.Attribute("Pos").Value;
    //                        if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
    //                        if (el.Attribute("Scale") != null) prObj.Scale = el.Attribute("Scale").Value;
    //                        prObj.EntityClass = el.Attribute("EntityClass").Value;
    //                        prObj.Type = el.Attribute("EntityClass").Value;
    //                        prObj.Id = GuidUtility.genID("sfdf4rgds" + prObj.Name); ;
    //                        prObj.Layer = el.Attribute("Layer").Value;
    //                        byte[] asciiBytes = Encoding.ASCII.GetBytes(el.Attribute("Layer").Value);
    //                        prObj.LayerGUID = layerguid;
    //                        prObj.Properties = el.Element("Properties");
    //                        prefabObjects.Add(prObj);
    //                    }
    //                if (el.Attribute("EntityClass") != null) if (el.Attribute("EntityClass").Value == "LightGroup")
    //                    {
    //                        PrefabObject prObj = new PrefabObject();
    //                        prObj.lights = new List<Light>();
    //                        prObj.Name = el.Attribute("Name").Value;
    //                        if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
    //                        if (el.Attribute("Pos") != null) prObj.Pos = el.Attribute("Pos").Value;
    //                        if (el.Attribute("Scale") != null) prObj.Scale = el.Attribute("Scale").Value;
    //                        prObj.EntityClass = "LightGroup";
    //                        prObj.Layer = el.Attribute("Layer").Value;
    //                        prObj.Id = GuidUtility.genID(prObj.Name + "dfgjhdkjf");
    //                        int i = 0;
    //                        foreach (XElement ell in el.Descendants("Light"))
    //                        {
    //                            XElement relativeXForm = ell.Element("RelativeXForm");
    //                            Light lightEnt = new Light();
    //                            lightEnt.Name = el.Attribute("Name").Value;
    //                            if (relativeXForm.Attribute("translation") != null) lightEnt.translation = relativeXForm.Attribute("translation").Value;
    //                            if (relativeXForm.Attribute("rotation") != null) lightEnt.rotation = relativeXForm.Attribute("rotation").Value;
    //                            if (el.Attribute("Scale") != null) lightEnt.Scale = el.Attribute("Scale").Value;
    //                            lightEnt.EntityClass = "Light";
    //                            lightEnt.Type = "Light";
    //                            lightEnt.Id = GuidUtility.genID("sdf3r4ds" + prObj.Name + lightEnt.Name + i);
    //                            lightEnt.Layer = el.Attribute("Layer").Value;
    //                            lightEnt.LayerGUID = layerguid;
    //                            //lightEnt.AttachmentPointName = el.Attribute("Layer").Value; ;
    //                            lightEnt.Properties = ell.Element("Properties");
    //                            prObj.lights.Add(lightEnt);
    //                            i++;
    //                        }
    //                        prefabObjects.Add(prObj);
    //                    }
    //            }
    //        }

    //    }
    //    void extractEnvironmentLightEntity(string xmlstr)
    //    {
    //        try
    //        {
    //            if (xmlstr.Length > 0)
    //            {
    //                TextReader tr = new StringReader(xmlstr);
    //                XDocument xml = XDocument.Load(tr);
    //                string layerguid = GuidUtility.genID("Main");
    //                foreach (XElement el in xml.Descendants("Entity"))
    //                {
    //                    if (el.Attribute("EntityClass") != null) if (el.Attribute("EntityClass").Value == "EnvironmentLight")
    //                        {
    //                            PrefabObject prObj = new PrefabObject();
    //                            prObj.Name = el.Attribute("Name").Value;
    //                            prObj.Pos = el.Attribute("Pos").Value;
    //                            if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
    //                            if (el.Attribute("Scale") != null) prObj.Scale = el.Attribute("Scale").Value;
    //                            prObj.EntityClass = el.Attribute("EntityClass").Value;
    //                            prObj.Type = el.Attribute("EntityClass").Value;
    //                            prObj.Id = GuidUtility.genID("dfgdfg453" + prObj.Name + prObj.Pos + prObj.EntityClass);
    //                            prObj.Layer = el.Attribute("Layer").Value;
    //                            byte[] asciiBytes = Encoding.ASCII.GetBytes(el.Attribute("Layer").Value);
    //                            prObj.LayerGUID = layerguid;
    //                            prObj.Properties = el.Element("Properties");
    //                            prefabObjects.Add(prObj);
    //                        }
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("|{0}|", xmlstr);
    //            throw;
    //        }
    //    }
    //    void extractVisAreaEntity(string xmlstr)
    //    {
    //        try
    //        {
    //            if (xmlstr.Length > 0)
    //            {
    //                TextReader tr = new StringReader(xmlstr);
    //                XDocument xml = XDocument.Load(tr);
    //                foreach (XElement el in xml.Descendants("Object"))
    //                {
    //                    if (el.Attribute("Type") != null) if (el.Attribute("Type").Value == "VisArea")
    //                        {
    //                            //Console.WriteLine("sdf");
    //                            PrefabObject prObj = new PrefabObject();
    //                            prObj.Name = el.Attribute("Name").Value;
    //                            prObj.Pos = el.Attribute("Pos").Value;
    //                            //prObj.Pos = "0,0,0";
    //                            if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
    //                            if (el.Attribute("Scale") != null) prObj.Scale = el.Attribute("Scale").Value;
    //                            if (el.Attribute("GeometryFile") != null) prObj.GeometryFile = el.Attribute("GeometryFile").Value;
    //                            if (el.Attribute("GeometryContainsPortals") != null) prObj.GeometryContainsPortals = el.Attribute("GeometryContainsPortals").Value;
    //                            prObj.Type = el.Attribute("Type").Value;
    //                            prObj.Id = GuidUtility.genID("dfghk55ggu" + prObj.Name + prObj.Pos);
    //                            prObj.Points = el.Element("Points");
    //                            prObj.DisplayFilled = el.Attribute("DisplayFilled").Value;
    //                            prObj.Height = el.Attribute("Height").Value;
    //                            prObj.VisAreaPos = el.Attribute("Pos").Value;
    //                            VisAreaPos = el.Attribute("Pos").Value;
    //                            prObj.fixPoints();
    //                            PrefabObject prObjFromChunk = areaShapeChunk.getPrefabObjByName(el.Attribute("Name").Value);
    //                            if (prObjFromChunk.Name != null)
    //                            {
    //                                prObj.Pos = prObjFromChunk.Pos;
    //                                prObj.Rotate = prObjFromChunk.Rotate;
    //                                prObj.Height = prObjFromChunk.Height;
    //                                prObj.Points = prObjFromChunk.Points;
    //                            }
    //                            prefabObjects.Add(prObj);
    //                        }
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("|{0}|", xmlstr);
    //            Console.WriteLine(name);
    //            throw;
    //        }
    //    }
    //    void extractPortalEntity(string xmlstr)
    //    {
    //        try
    //        {
    //            if (xmlstr.Length > 0)
    //            {
    //                TextReader tr = new StringReader(xmlstr);
    //                XDocument xml = XDocument.Load(tr);
    //                foreach (XElement el in xml.Descendants("Object"))
    //                {
    //                    if (el.Attribute("Type") != null) if (el.Attribute("Type").Value == "Portal")
    //                        {
    //                            PrefabObject prObj = new PrefabObject();
    //                            prObj.Name = el.Attribute("Name").Value;
    //                            prObj.Pos = el.Attribute("Pos").Value;
    //                            //prObj.Pos = "0,0,0";
    //                            //if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
    //                            if (el.Attribute("Rotate") != null) prObj.Rotate = "1,0,0,0";
    //                            if (el.Attribute("Scale") != null) prObj.Scale = el.Attribute("Scale").Value;
    //                            prObj.Type = el.Attribute("Type").Value;
    //                            prObj.Id = GuidUtility.genID("dfghk55ggu" + prObj.Name + prObj.Pos + prObj.Type);
    //                            prObj.Points = el.Element("Points");
    //                            prObj.DisplayFilled = el.Attribute("DisplayFilled").Value;
    //                            prObj.Height = el.Attribute("Height").Value;
    //                            prObj.LightBlending = el.Attribute("LightBlending").Value;
    //                            prObj.LightBlendValue = el.Attribute("LightBlendValue").Value;
    //                            if (VisAreaPos != null) prObj.VisAreaPos = VisAreaPos; else { Console.WriteLine(socpakPath); Console.WriteLine(el.Attribute("Name").Value); };
    //                            if (name == "frnt") Console.WriteLine(VisAreaPos);
    //                            prObj.fixPoints();
    //                            PrefabObject prObjFromChunk = areaShapeChunk.getPrefabObjByName(el.Attribute("Name").Value);
    //                            if (prObjFromChunk.Name != null)
    //                            {
    //                                prObj.Pos = prObjFromChunk.Pos;
    //                                prObj.Rotate = prObjFromChunk.Rotate;
    //                                prObj.Height = prObjFromChunk.Height;
    //                                prObj.Points = prObjFromChunk.Points;
    //                            }
    //                            prefabObjects.Add(prObj);
    //                        }
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("|{0}|", xmlstr);
    //            Console.WriteLine(name);
    //            Console.WriteLine(socpakPath);
    //            throw;
    //        }
    //    }

    //    /// <summary>
    //    /// loading object container from socpak
    //    /// </summary>
    //    /// <param name="path">socpak path</param>
    //    void LoadSoc(string path)
    //    {
    //        Stream streamFile = new MemoryStream();
    //        using (ZipArchive zip = ZipFile.Open(path, ZipArchiveMode.Read))
    //        {
    //            //Console.WriteLine(path);
    //            string entryname = "";
    //            foreach (ZipArchiveEntry entry in zip.Entries)
    //            {
    //                //Console.WriteLine("LoadSoc entry.FullName {0}", entry.FullName);
    //                if (entry.FullName == name.ToLower() + ".soc")
    //                    entryname = entry.FullName;
    //            }
    //            if (entryname == "")
    //            {
    //                Console.WriteLine("entryname=null");
    //                foreach (ZipArchiveEntry entry in zip.Entries)
    //                {
    //                    Console.WriteLine(entry.FullName);
    //                }
    //            }
    //            byte[] asciiBytes = Encoding.ASCII.GetBytes(entryname);
    //            layerGUID = Guid.NewGuid().ToString();
    //            prefabGUID = Guid.NewGuid().ToString();
    //            //Console.WriteLine("name {0}", name);
    //            //Console.WriteLine("entryname {0}",entryname);
    //            //ZipArchiveEntry socfile = zip.GetEntry(entryname);
    //            ZipArchiveEntry socfile = zip.GetEntry(entryname);
    //            socfile.Open().CopyTo(streamFile);
    //            streamFile.Position = 0;
    //            //Console.WriteLine(socfile.Length);
    //        }
    //        //Console.WriteLine(streamFile.Length);


    //        ////////////////////////
    //        //layerGUID = "1" + RandomString(5);
    //        //prefabGUID = "1" + RandomString(5);
    //        id = "1" + RandomString(5);

    //        ////////////////////////  
    //        socFile = new File_ChCr_746(streamFile, name.ToLower() + ".soc");

    //        for (int i = 0; i < socFile.chunks.Count; i++)
    //        {
    //            switch (socFile.chunks[i].type)
    //            {
    //                case (ushort)Chunk_Objects.type:
    //                    Chunk_Objects chunkObjects = new Chunk_Objects(socFile.chunks[i].content);
    //                    objects.Add(new Objects(chunkObjects.objectsStream));
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }

    //        //BinaryReader br = new BinaryReader(streamFile); 
    //        //Console.WriteLine("dataChunkSize: {0} count: {1}", objectsChunks[i].dataChunkSize.ToString("X8"), objectsChunks[i].getDataElementsCount());
    //        //for (int j = 0; j < objectsChunks[i].getDataElementsCount(); j++)
    //        //{
    //        //    PrefabObject prefabObj = new PrefabObject();

    //        //    //////////////////////////
    //        //    //Console.WriteLine(br.BaseStream.Position);
    //        //    uint subchunkType = br.ReadUInt32();
    //        //    DataStreamSubChunk subchunk = new DataStreamSubChunk(br, subchunkType);
    //        //    if (subchunkType == 7) continue;
    //        //    if (subchunkType == 16) continue;
    //        //    double x = subchunk.pos.x;
    //        //    double y = subchunk.pos.y;
    //        //    double z = subchunk.pos.z;
    //        //    //if(Double.IsInfinity(z))
    //        //    //{
    //        //    //    z = 1;
    //        //    //}
    //        //    int id = subchunk.id;
    //        //    CryEngine.Quaternion quat = new CryEngine.Quaternion(subchunk.rotMatrix34);
    //        //    //if(Double.IsInfinity(quat.W))
    //        //    //{
    //        //    //    quat.W = 1;
    //        //    //}
    //        //    string objectname = Path.GetFileNameWithoutExtension(objectsChunks[i].filesPaths[id]);
    //        //    prefabObj.Name = objectname;
    //        //    prefabObj.Geometry = objectsChunks[i].filesPaths[id];
    //        //    prefabObj.Pos = (float)x + "," + (float)y + "," + (float)z;
    //        //    prefabObj.Rotate = quat.W + "," + quat.X + "," + quat.Y + "," + quat.Z;
    //        //    //Console.WriteLine(prefabObj.Geometry);
    //        //    prefabObj.EntityClass = "GeomEntity";
    //        //    byte[] asciiBytes = Encoding.ASCII.GetBytes(objectname);
    //        //    prefabObj.Layer = objectname;
    //        //    //prefabObj.LayerGUID = "3675a837-e15f-dfa0-db3a-"+ bytesToString(asciiBytes);
    //        //    prefabObj.LayerGUID = GuidUtility.genLayerID("sdf33" + objectname + prefabObj.Geometry + prefabObj.Pos);
    //        //    prefabObj.Id = GuidUtility.genPrefabID("sdf34gffg" + objectname + prefabObj.Geometry + prefabObj.Pos);
    //        //    prefabObjects.Add(prefabObj);

    //        //    //} 
    //        //if (AreaShapeChunkOffset != 0)
    //        //{
    //        //    //Console.WriteLine(AreaShapeChunkOffset);
    //        //    br.BaseStream.Position = AreaShapeChunkOffset;
    //        //    areaShapeChunk = new AreaShapeChunk(br, socpakPath);
    //        //    //foreach(PrefabObject prefabObj in areaShapeChunk.prefabObjects)
    //        //    //{

    //        //    //}
    //        //}

    //        //br.BaseStream.Position = xmlChunkOffset;

    //        //if (!IsDecoded(br))
    //        //{
    //        //    byte[] xmlSCOCEntities = null;
    //        //    br.BaseStream.Position = xmlChunkOffset;
    //        //    xmlSCOCEntities = br.ReadBytes((int)xmlChunkSize);
    //        //    extractGeomEntityCryXml(xmlSCOCEntities, (int)xmlChunkOffset);
    //        //    extractLightEntityCryXml(xmlSCOCEntities, (int)xmlChunkOffset);
    //        //}
    //        //else
    //        //{
    //        //    string xmlSCOCEntities = null;
    //        //    br.BaseStream.Position = xmlChunkOffset;
    //        //    xmlSCOCEntities = new string(br.ReadChars((int)xmlChunkSize)).TrimEnd('\0').TrimEnd(Environment.NewLine.ToCharArray());
    //        //    //Console.WriteLine(xmlSCOCEntities);
    //        //    //Console.WriteLine("end");
    //        //    extractGeomEntity(xmlSCOCEntities);
    //        //    // extractLightEntity(xmlSCOCEntities);
    //        //    // extractEnvironmentLightEntity(xmlSCOCEntities);
    //        //    // extractVisAreaEntity(xmlSCOCEntities);
    //        //    // extractPortalEntity(xmlSCOCEntities);
    //        //}
    //        //  br.Close();

    //    }
    //    void LoadSocOLD(string path)
    //    {
    //        Stream streamFile = new MemoryStream();
    //        using (ZipArchive zip = ZipFile.Open(path, ZipArchiveMode.Read))
    //        {
    //            //Console.WriteLine(path);
    //            string entryname = "";
    //            foreach (ZipArchiveEntry entry in zip.Entries)
    //            {
    //                //Console.WriteLine("LoadSoc entry.FullName {0}", entry.FullName);
    //                if (entry.FullName == name.ToLower() + ".soc")
    //                    entryname = entry.FullName;
    //            }
    //            if (entryname == "")
    //            {
    //                Console.WriteLine("entryname=null");
    //                foreach (ZipArchiveEntry entry in zip.Entries)
    //                {
    //                    Console.WriteLine(entry.FullName);
    //                }
    //            }
    //            byte[] asciiBytes = Encoding.ASCII.GetBytes(entryname);
    //            layerGUID = Guid.NewGuid().ToString();
    //            prefabGUID = Guid.NewGuid().ToString();
    //            //Console.WriteLine("name {0}", name);
    //            //Console.WriteLine("entryname {0}",entryname);
    //            //ZipArchiveEntry socfile = zip.GetEntry(entryname);
    //            ZipArchiveEntry socfile = zip.GetEntry(entryname);
    //            socfile.Open().CopyTo(streamFile);
    //            //Console.WriteLine(socfile.Length);
    //        }
    //        //Console.WriteLine(streamFile.Length);


    //        ////////////////////////
    //        //layerGUID = "1" + RandomString(5);
    //        //prefabGUID = "1" + RandomString(5);
    //        id = "1" + RandomString(5);

    //        ////////////////////////  
    //        BinaryReader br = new BinaryReader(streamFile);
    //        br.BaseStream.Position = 0;
    //        br.ReadUInt32();
    //        br.ReadUInt32();
    //        uint headerElements = br.ReadUInt32();
    //        br.ReadUInt32();
    //        uint[,] dataStreamChunksOffsets = new uint[headerElements, 2];
    //        uint xmlChunkOffset = 0;
    //        uint xmlChunkSize = 0;
    //        uint AreaShapeChunkOffset = 0;//portals, visareas
    //        uint AreaShapeChunkSize = 0;
    //        int dataStreamsCount = 0;
    //        for (int i = 0; i < (int)headerElements; i++)
    //        {
    //            uint chunkType = br.ReadUInt16();
    //            //Console.WriteLine("{0:X}", chunkType);
    //            br.ReadUInt16();
    //            uint id = br.ReadUInt32();
    //            uint size = br.ReadUInt32();
    //            uint offset = br.ReadUInt32();
    //            if (chunkType == 0x00000010) { dataStreamChunksOffsets[dataStreamsCount, 0] = offset; dataStreamChunksOffsets[dataStreamsCount, 1] = id; dataStreamsCount++; }
    //            if (chunkType == 0x00000004) { xmlChunkOffset = offset; xmlChunkSize = size; }
    //            if (chunkType == 0x0000000E) { AreaShapeChunkOffset = offset; AreaShapeChunkSize = size; }
    //        }
    //        //Console.WriteLine("dataStreamsCount {0}", dataStreamsCount);
    //        ObjectsChunk[] objectsChunks = new ObjectsChunk[dataStreamsCount];
    //        for (int i = 0; i < dataStreamsCount; i++)
    //        {
    //            br.BaseStream.Position = dataStreamChunksOffsets[i, 0];
    //            br.ReadUInt32();
    //            objectsChunks[i].setfilesPaths1((int)br.ReadUInt32());
    //            for (int j = 0; j < objectsChunks[i].filesPaths1Count; j++)
    //            {
    //                objectsChunks[i].filesPaths1[j] = new string(br.ReadChars(256)).TrimEnd('\0');
    //                //Console.WriteLine(objectsChunks[i].filesPaths1[j]);
    //            }
    //            objectsChunks[i].setfilesPaths2((int)br.ReadUInt32());
    //            for (int j = 0; j < objectsChunks[i].filesPaths2Count; j++)
    //            {
    //                objectsChunks[i].filesPaths2[j] = new string(br.ReadChars(256)).TrimEnd('\0');
    //                //Console.WriteLine(objectsChunks[i].filesPaths2[j]);
    //            }
    //            objectsChunks[i].mergeFilesPathsArrays();
    //            //Console.WriteLine("curr pos {0}", br.BaseStream.Position.ToString("X8"));
    //            br.ReadUInt32(); br.ReadUInt32(); br.ReadUInt32();
    //            br.ReadUInt32(); br.ReadUInt32(); br.ReadUInt32(); br.ReadUInt32();
    //            objectsChunks[i].dataChunkSize = br.ReadUInt32();
    //            //Console.WriteLine("dataChunkSize: {0} count: {1}", objectsChunks[i].dataChunkSize.ToString("X8"), objectsChunks[i].getDataElementsCount());
    //            for (int j = 0; j < objectsChunks[i].getDataElementsCount(); j++)
    //            {
    //                PrefabObject prefabObj = new PrefabObject();

    //                //////////////////////////
    //                //Console.WriteLine(br.BaseStream.Position);
    //                uint subchunkType = br.ReadUInt32();
    //                DataStreamSubChunk subchunk = new DataStreamSubChunk(br, subchunkType);
    //                if (subchunkType == 7) continue;
    //                if (subchunkType == 16) continue;
    //                double x = subchunk.pos.x;
    //                double y = subchunk.pos.y;
    //                double z = subchunk.pos.z;
    //                //if(Double.IsInfinity(z))
    //                //{
    //                //    z = 1;
    //                //}
    //                int id = subchunk.id;
    //                CryEngine.Quaternion quat = new CryEngine.Quaternion(subchunk.rotMatrix34);
    //                //if(Double.IsInfinity(quat.W))
    //                //{
    //                //    quat.W = 1;
    //                //}
    //                string objectname = Path.GetFileNameWithoutExtension(objectsChunks[i].filesPaths[id]);
    //                prefabObj.Name = objectname;
    //                prefabObj.Geometry = objectsChunks[i].filesPaths[id];
    //                prefabObj.Pos = (float)x + "," + (float)y + "," + (float)z;
    //                prefabObj.Rotate = quat.W + "," + quat.X + "," + quat.Y + "," + quat.Z;
    //                //Console.WriteLine(prefabObj.Geometry);
    //                prefabObj.EntityClass = "GeomEntity";
    //                byte[] asciiBytes = Encoding.ASCII.GetBytes(objectname);
    //                prefabObj.Layer = objectname;
    //                //prefabObj.LayerGUID = "3675a837-e15f-dfa0-db3a-"+ bytesToString(asciiBytes);
    //                prefabObj.LayerGUID = GuidUtility.genLayerID("sdf33" + objectname + prefabObj.Geometry + prefabObj.Pos);
    //                prefabObj.Id = GuidUtility.genPrefabID("sdf34gffg" + objectname + prefabObj.Geometry + prefabObj.Pos);
    //                prefabObjects.Add(prefabObj);

    //            }

    //        }
    //        if (AreaShapeChunkOffset != 0)
    //        {
    //            //Console.WriteLine(AreaShapeChunkOffset);
    //            br.BaseStream.Position = AreaShapeChunkOffset;
    //            areaShapeChunk = new AreaShapeChunk(br, socpakPath);
    //            //foreach(PrefabObject prefabObj in areaShapeChunk.prefabObjects)
    //            //{

    //            //}
    //        }

    //        br.BaseStream.Position = xmlChunkOffset;

    //        if (!IsDecoded(br))
    //        {
    //            byte[] xmlSCOCEntities = null;
    //            br.BaseStream.Position = xmlChunkOffset;
    //            xmlSCOCEntities = br.ReadBytes((int)xmlChunkSize);
    //            extractGeomEntityCryXml(xmlSCOCEntities, (int)xmlChunkOffset);
    //            extractLightEntityCryXml(xmlSCOCEntities, (int)xmlChunkOffset);
    //        }
    //        else
    //        {
    //            string xmlSCOCEntities = null;
    //            br.BaseStream.Position = xmlChunkOffset;
    //            xmlSCOCEntities = new string(br.ReadChars((int)xmlChunkSize)).TrimEnd('\0').TrimEnd(Environment.NewLine.ToCharArray());
    //            //Console.WriteLine(xmlSCOCEntities);
    //            //Console.WriteLine("end");
    //            extractGeomEntity(xmlSCOCEntities);
    //            // extractLightEntity(xmlSCOCEntities);
    //            // extractEnvironmentLightEntity(xmlSCOCEntities);
    //            // extractVisAreaEntity(xmlSCOCEntities);
    //            // extractPortalEntity(xmlSCOCEntities);
    //        }
    //        br.Close();

    //    }
    //    string ConvertBytesToString(byte[] data)
    //    {
    //        char[] characters = data.Select(b => (char)b).ToArray();
    //        Console.WriteLine(new string(characters));
    //        return new string(characters);

    //    }
    //    string RandomString(int length)
    //    {
    //        const string chars = "123456789";
    //        return new string(Enumerable.Repeat(chars, length)
    //          .Select(s => s[random.Next(s.Length)]).ToArray());
    //    }
    //    string bytesToString(byte[] bytes)
    //    {
    //        string newString = "";
    //        foreach (byte bt in bytes)
    //        {
    //            newString += (bt / 16);
    //        }
    //        return newString;
    //    }
    //    string stringToNumbers(byte[] bytes)
    //    {
    //        string newString = "";
    //        foreach (byte bt in bytes)
    //        {
    //            newString += (bt / 16);
    //        }
    //        return newString;
    //    }
    //    static bool IsDecoded(BinaryReader br)
    //    {
    //        bool decoded = false;
    //        string fileLabel = new string(br.ReadChars(6));
    //        if (fileLabel != "CryXml") decoded = true;

    //        return decoded;
    //    }
    //}
    //class PrefabObject
    //{
    //    string _pos;
    //    string _rot;
    //    string _VisAreaPos;
    //    public List<Light> lights;
    //    public string Name;
    //    public string Type;
    //    public string Layer;
    //    public string LayerGUID;
    //    public string Id;
    //    public string Rotate
    //    {
    //        get { return _rot; }
    //        set
    //        {
    //            //Console.WriteLine("sdf");
    //            _rot = value;
    //            string[] coords;
    //            coords = _rot.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    //            rotv.x = float.Parse(coords[0], CultureInfo.InvariantCulture.NumberFormat);
    //            rotv.y = float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat);
    //            rotv.z = float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat);
    //            rotv.w = float.Parse(coords[3], CultureInfo.InvariantCulture.NumberFormat);
    //        }
    //    }
    //    public string Scale;
    //    public string Geometry;
    //    public string LodRatio;
    //    public string HiddenInGame;
    //    public string EntityClass;
    //    public XElement Points;
    //    public string DisplayFilled;
    //    public string Height;
    //    public XElement Properties;
    //    public string LightBlending;
    //    public string LightBlendValue;
    //    public string GeometryFile;
    //    public string GeometryContainsPortals;
    //    public string AttachmentPointName;
    //    Vector3 posv;
    //    Vector3 visareaposv;
    //    Vector4 rotv;
    //    public string VisAreaPos
    //    {
    //        get { return _VisAreaPos; }
    //        set
    //        {
    //            //Console.WriteLine("sdf");
    //            _VisAreaPos = value;
    //            string[] coords;
    //            coords = _VisAreaPos.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    //            visareaposv.x = float.Parse(coords[0], CultureInfo.InvariantCulture.NumberFormat);
    //            visareaposv.y = float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat);
    //            visareaposv.z = float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat);
    //            if (Name == "Portal001-021") Console.WriteLine(visareaposv.x);
    //        }
    //    }
    //    public string Pos
    //    {
    //        get { return _pos; }
    //        set
    //        {
    //            //Console.WriteLine("sdf");
    //            _pos = value;
    //            string[] coords;
    //            coords = _pos.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    //            posv.x = float.Parse(coords[0], CultureInfo.InvariantCulture.NumberFormat);
    //            posv.y = float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat);
    //            posv.z = float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat);
    //        }
    //    }
    //    public void setPos(Vector3 vec)
    //    {
    //        string format = "N6";
    //        Pos = vec.x.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
    //            + "," + vec.y.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
    //            + "," + vec.z.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray());
    //    }
    //    public void setRotate(Vector4 vec)
    //    {
    //        string format = "N6";
    //        Rotate = vec.w.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
    //            + "," + vec.x.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
    //            + "," + vec.y.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
    //            + "," + vec.z.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray());
    //    }

    //    public void fixPoints()
    //    {
    //        //XElement newel = new XElement("Points");
    //        //List<Point> points = new List<Point>(); 
    //        //foreach (XElement el in Points.Elements("Point"))
    //        //{
    //        //    points.Add(new Point(el.Attribute("Pos").Value));
    //        //}
    //        //Point ptt = points[0];
    //        //ptt = new Point((ptt.x - visareaposv.x ) + "," + (ptt.y - visareaposv.y ) + "," + (ptt.z - visareaposv.z ));
    //        //foreach (Point pt in points)
    //        //{
    //        //    XElement el = new XElement("Point");
    //        //    //string newpos = (pt.x - posv.x) + "," + (pt.y - posv.y) + "," + (pt.z - posv.z);
    //        //    //string newpos = (posv.x - pt.x) + "," + (posv.y - pt.y) + "," + (posv.z - pt.z);
    //        //    //string newpos = (pt.x - ptt.x) + "," + (pt.y - ptt.y) + "," + (pt.z - ptt.z);
    //        //    //string newpos = (-pt.x) + "," + (-pt.y) + "," + (-pt.z);
    //        //    string newpos = (pt.x - visareaposv.x) + "," + (pt.y - visareaposv.y) + "," + (pt.z - visareaposv.z);
    //        //    //string newpos = (pt.x - visareaposv.x - ptt.x) + "," + (pt.y - visareaposv.y - ptt.y) + "," + (pt.z - visareaposv.z - ptt.z);
    //        //    //string newpos = (visareaposv.x - pt.x) + "," + (visareaposv.y - pt.y) + "," + (visareaposv.z - pt.z);
    //        //    el.Add(new XAttribute("Pos", newpos));
    //        //    newel.Add(el);
    //        //}
    //        //Points = newel;
    //        //// Pos = (-posv.x) + "," + (-posv.y) + "," + (-posv.z);
    //        ////Rotate = (-rotv.x) + "," + (-rotv.y) + "," + (-rotv.z) + "," + (-rotv.w); 
    //        ////Pos = (ptt.x - posv.x) + "," + (ptt.y - posv.y) + "," + (ptt.z - posv.z); 
    //        ////Pos = (posv.x - ptt.x) + "," + (posv.y - ptt.y) + "," + (posv.z - ptt.z);
    //        ////Pos = (points[0].x - points[1].x) + "," + (posv.z) + "," + "0";
    //        ////Pos = (posv.x - visareaposv.x) + "," + (posv.y - visareaposv.y) + "," + (posv.z - visareaposv.z);
    //        //Pos = "2.25,0,-1.625";
    //        // Rotate = "1,0,0,0";
    //    }

    //    //public void  SetPos(Vector3) {_pos = }
    //    //public void SetPos() {
    //    //    get { return _pos.x+"," + _pos.x + "," + _pos.x ; }
    //    //    set { _pos = value; }
    //    //}
    //}
    //class Point
    //{
    //    public float x { get { return posv.x; } set { } }
    //    public float y { get { return posv.y; } set { } }
    //    public float z { get { return posv.z; } set { } }
    //    Vector3 posv;
    //    string _pos;
    //    public string Pos
    //    {
    //        get { return _pos; }
    //        set
    //        {
    //            _pos = value;
    //            string[] coords;
    //            coords = _pos.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    //            posv.x = float.Parse(coords[0], CultureInfo.InvariantCulture.NumberFormat);
    //            posv.y = float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat);
    //            posv.z = float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat);
    //        }
    //    }
    //    public Point(string posstr)
    //    {
    //        Pos = posstr;
    //    }
    //    public Point(Vector3 posvec)
    //    {
    //        string format = "N6";
    //        Pos = posvec.x.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
    //            + "," + posvec.y.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
    //            + "," + posvec.z.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray());
    //    }
    //    public Point(float x, float y, float z)
    //    {
    //        string format = "N6";
    //        Pos = x.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
    //            + "," + y.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray())
    //            + "," + z.ToString(format).TrimEnd("0".ToCharArray()).TrimEnd(".".ToCharArray());
    //    }
    //}

    ///// <summary>
    ///// Id in header: 0000 000E
    ///// </summary>
    //class AreaShapeChunk
    //{
    //    uint size;
    //    uint VisAreasCount;
    //    uint PortalsCount;
    //    string socpak;
    //    List<VisAreaSubChunk> visAreaSubChunks = new List<VisAreaSubChunk>();
    //    List<PortalSubChunk> portalSubChunks = new List<PortalSubChunk>();
    //    public List<PrefabObject> prefabObjects = new List<PrefabObject>();
    //    public AreaShapeChunk(BinaryReader br, string socname)
    //    {
    //        br.ReadUInt32();
    //        size = br.ReadUInt32();
    //        VisAreasCount = br.ReadUInt32();
    //        PortalsCount = br.ReadUInt32();
    //        br.ReadUInt32();
    //        for (int i = 0; i < VisAreasCount; i++) visAreaSubChunks.Add(new VisAreaSubChunk(br));
    //        for (int i = 0; i < PortalsCount; i++) portalSubChunks.Add(new PortalSubChunk(br, socname));
    //        makePrefabObjects();
    //    }
    //    public PrefabObject getPrefabObjByName(string name)
    //    {
    //        PrefabObject probj = new PrefabObject();
    //        foreach (PrefabObject pr in prefabObjects)
    //        {
    //            if (pr.Name.ToLower() == name.ToLower()) probj = pr;
    //        }
    //        return probj;
    //    }
    //    void makePrefabObjects()
    //    {
    //        foreach (VisAreaSubChunk sc in visAreaSubChunks)
    //        {
    //            if (sc.type == 0x00000090 || sc.type == 0x00000091 || sc.type == 0x000000B0)
    //            {
    //                PrefabObject prefabObj = new PrefabObject();
    //                prefabObj.Name = sc.name;
    //                prefabObj.setPos(sc.Pos);
    //                prefabObj.setRotate(sc.Rotate);
    //                prefabObj.Height = sc.height.ToString();

    //                XElement elPoints = new XElement("Points");
    //                foreach (Point pt in sc.points)
    //                {
    //                    XElement elPoint = new XElement("Point");
    //                    elPoint.Add(new XAttribute("Pos", pt.Pos));
    //                    elPoints.Add(elPoint);
    //                }
    //                prefabObj.Points = elPoints;
    //                prefabObjects.Add(prefabObj);
    //            }
    //        }
    //        foreach (PortalSubChunk sc in portalSubChunks)
    //        {
    //            if (sc.type == 0x00000038)
    //            {
    //                PrefabObject prefabObj = new PrefabObject();
    //                prefabObj.Name = sc.name;
    //                prefabObj.setPos(sc.Pos);
    //                prefabObj.setRotate(sc.Rotate);
    //                prefabObj.Height = sc.height.ToString();

    //                XElement elPoints = new XElement("Points");
    //                foreach (Point pt in sc.points)
    //                {
    //                    XElement elPoint = new XElement("Point");
    //                    elPoint.Add(new XAttribute("Pos", pt.Pos));
    //                    elPoints.Add(elPoint);
    //                }
    //                prefabObj.Points = elPoints;
    //                prefabObjects.Add(prefabObj);
    //            }
    //        }
    //    }
    //    private class VisAreaSubChunk
    //    {
    //        public string name;
    //        /// <summary>
    //        /// 0x0000 0090 - visarea;
    //        /// 0x0000 0093 - 3d_visarea
    //        /// </summary>
    //        public uint type;
    //        public Vector3 Pos;
    //        public Vector4 Rotate;
    //        public double height;
    //        public List<Point> points = new List<Point>();
    //        public VisAreaSubChunk(BinaryReader br)
    //        {
    //            br.ReadUInt32();
    //            name = new string(br.ReadChars(32)).TrimEnd('\0');
    //            //Console.WriteLine("VisAreaSubChunk {0} {1} {2}",name,br.BaseStream.Position, br.BaseStream.Position.ToString("X"));
    //            type = br.ReadUInt32();
    //            //Console.WriteLine("type {0}", type.ToString("X"));
    //            if (type == 0x00000090) readType90(br);
    //            if (type == 0x00000093) readType93(br);
    //            if (type == 0x00000091) readType91(br);
    //            if (type == 0x000000B0) readTypeB0(br);
    //        }
    //        void readType90(BinaryReader br)
    //        {
    //            br.ReadUInt32();
    //            br.ReadUInt32();
    //            br.ReadUInt32();
    //            height = br.ReadDouble();
    //            Rotate.x = (float)br.ReadDouble();
    //            Rotate.y = (float)br.ReadDouble();
    //            Rotate.z = (float)br.ReadDouble();
    //            Rotate.w = (float)br.ReadDouble();
    //            Pos.x = (float)br.ReadDouble();
    //            Pos.y = (float)br.ReadDouble();
    //            Pos.z = (float)br.ReadDouble();

    //            uint pointsCount = 0;
    //            pointsCount = br.ReadUInt32();
    //            for (int i = 0; i < pointsCount; i++)
    //            {
    //                points.Add(new Point(
    //                    (float)br.ReadDouble(),
    //                    (float)br.ReadDouble(),
    //                    (float)br.ReadDouble()
    //                    ));
    //            }
    //        }
    //        void readType93(BinaryReader br)
    //        {
    //            br.ReadUInt32();
    //            br.ReadUInt32();
    //            br.ReadUInt32();
    //            height = br.ReadDouble();
    //            Rotate.x = (float)br.ReadDouble();
    //            Rotate.y = (float)br.ReadDouble();
    //            Rotate.z = (float)br.ReadDouble();
    //            Rotate.w = (float)br.ReadDouble();
    //            Pos.x = (float)br.ReadDouble();
    //            Pos.y = (float)br.ReadDouble();
    //            Pos.z = (float)br.ReadDouble();

    //            uint pointsCount = 0;
    //            pointsCount = br.ReadUInt32();
    //            for (int i = 0; i < pointsCount; i++)
    //            {
    //                points.Add(new Point(
    //                    (float)br.ReadDouble(),
    //                    (float)br.ReadDouble(),
    //                    (float)br.ReadDouble()
    //                    ));
    //            }
    //            uint uknown1Count = 0;
    //            uknown1Count = br.ReadUInt32();
    //            for (int i = 0; i < uknown1Count; i++)
    //            {
    //                //br.ReadChars(32);
    //                br.ReadDouble();
    //                br.ReadDouble();
    //                br.ReadDouble();
    //                br.ReadDouble();
    //            }
    //            uint uknown2Count = 0;
    //            uknown2Count = br.ReadUInt32();
    //            for (int i = 0; i < uknown1Count; i++)
    //            {
    //                br.ReadSingle();
    //            }
    //        }
    //        void readType91(BinaryReader br)
    //        {
    //            //Console.WriteLine("sdf");
    //            br.ReadUInt32();
    //            br.ReadUInt32();
    //            br.ReadUInt32();
    //            height = br.ReadDouble();
    //            Rotate.x = (float)br.ReadDouble();
    //            Rotate.y = (float)br.ReadDouble();
    //            Rotate.z = (float)br.ReadDouble();
    //            Rotate.w = (float)br.ReadDouble();
    //            Pos.x = (float)br.ReadDouble();
    //            Pos.y = (float)br.ReadDouble();
    //            Pos.z = (float)br.ReadDouble();

    //            uint pointsCount = 0;
    //            pointsCount = br.ReadUInt32();
    //            //Console.WriteLine("pointsCount {0}", pointsCount.ToString("X"));
    //            for (int i = 0; i < pointsCount; i++)
    //            {
    //                points.Add(new Point(
    //                    (float)br.ReadDouble(),
    //                    (float)br.ReadDouble(),
    //                    (float)br.ReadDouble()
    //                    ));
    //            }
    //            uint uknown1Count = 0;
    //            uknown1Count = br.ReadUInt32();
    //            //Console.WriteLine("uknown1Count {0}", uknown1Count.ToString("X"));
    //            for (int i = 0; i < uknown1Count; i++)
    //            {
    //                //string xmlSCOCEntities = new string(br.ReadChars(32));
    //                //br.ReadChars(32);
    //                br.ReadDouble();
    //                br.ReadDouble();
    //                br.ReadDouble();
    //                br.ReadDouble();
    //            }
    //            uint uknown2Count = 0;
    //            uknown2Count = br.ReadUInt32();
    //            //Console.WriteLine("uknown2Count {0}", uknown2Count.ToString("X"));
    //            for (int i = 0; i < uknown2Count; i++)
    //            {
    //                br.ReadSingle();
    //            }
    //        }
    //        void readTypeB0(BinaryReader br)
    //        {
    //            br.ReadUInt32();
    //            br.ReadUInt32();
    //            br.ReadUInt32();
    //            height = br.ReadDouble();
    //            Rotate.x = (float)br.ReadDouble();
    //            Rotate.y = (float)br.ReadDouble();
    //            Rotate.z = (float)br.ReadDouble();
    //            Rotate.w = (float)br.ReadDouble();
    //            Pos.x = (float)br.ReadDouble();
    //            Pos.y = (float)br.ReadDouble();
    //            Pos.z = (float)br.ReadDouble();

    //            uint pointsCount = 0;
    //            pointsCount = br.ReadUInt32();
    //            //Console.WriteLine("pointsCount {0}", pointsCount.ToString("X"));
    //            for (int i = 0; i < pointsCount; i++)
    //            {
    //                points.Add(new Point(
    //                    (float)br.ReadDouble(),
    //                    (float)br.ReadDouble(),
    //                    (float)br.ReadDouble()
    //                    ));
    //            }
    //        }
    //    }
    //    private class PortalSubChunk
    //    {
    //        public string name;
    //        string socpak;
    //        /// <summary>
    //        /// 0x0000 0038 - portal;
    //        /// 0x0000 0018 - childportal_3d_visarea
    //        /// </summary>
    //        public uint type;
    //        public Vector3 Pos;
    //        public Vector4 Rotate;
    //        public double height;
    //        public List<Point> points = new List<Point>();
    //        public PortalSubChunk(BinaryReader br, string socname)
    //        {
    //            //Console.WriteLine(socname); 
    //            socpak = socname;
    //            try
    //            {
    //                br.ReadUInt32();
    //                name = new string(br.ReadChars(32)).TrimEnd('\0');
    //                //Console.WriteLine(name);
    //                type = br.ReadUInt32();
    //                if (type == 0x00000038) readType38(br);
    //                if (type == 0x00000018) readType18(br);
    //            }
    //            catch (Exception e)
    //            {
    //                Console.WriteLine(socpak);
    //                Console.WriteLine(e.Message);
    //                Console.Read();
    //                //throw;
    //            }
    //        }
    //        void readType38(BinaryReader br)
    //        {
    //            br.ReadUInt32();
    //            br.ReadUInt32();
    //            br.ReadUInt32();
    //            height = br.ReadDouble();
    //            Rotate.x = (float)br.ReadDouble();
    //            Rotate.y = (float)br.ReadDouble();
    //            Rotate.z = (float)br.ReadDouble();
    //            Rotate.w = (float)br.ReadDouble();
    //            Pos.x = (float)br.ReadDouble();
    //            Pos.y = (float)br.ReadDouble();
    //            Pos.z = (float)br.ReadDouble();

    //            uint pointsCount = 0;
    //            pointsCount = br.ReadUInt32();
    //            for (int i = 0; i < pointsCount; i++)
    //            {
    //                points.Add(new Point(
    //                    (float)br.ReadDouble(),
    //                    (float)br.ReadDouble(),
    //                    (float)br.ReadDouble()
    //                    ));
    //            }
    //        }
    //        void readType18(BinaryReader br)
    //        {
    //            br.ReadUInt32();
    //            br.ReadUInt32();
    //            br.ReadUInt32();
    //            height = br.ReadDouble();
    //            Rotate.x = (float)br.ReadDouble();
    //            Rotate.y = (float)br.ReadDouble();
    //            Rotate.z = (float)br.ReadDouble();
    //            Rotate.w = (float)br.ReadDouble();
    //            Pos.x = (float)br.ReadDouble();
    //            Pos.y = (float)br.ReadDouble();
    //            Pos.z = (float)br.ReadDouble();

    //            uint pointsCount = 0;
    //            pointsCount = br.ReadUInt32();
    //            for (int i = 0; i < pointsCount; i++)
    //            {
    //                points.Add(new Point(
    //                    (float)br.ReadDouble(),
    //                    (float)br.ReadDouble(),
    //                    (float)br.ReadDouble()
    //                    ));
    //            }
    //        }
    //    }
    //}

    //struct ObjectsChunk
    //{
    //    //public ObjectsChunk(uint fPathsCount) : this()
    //    //{
    //    //    Console.WriteLine((int)fPathsCount);
    //    //    filesPaths = new string[(int)fPathsCount];
    //    //    filesPathsCount =(int) fPathsCount;
    //    //}
    //    public string[] filesPaths;
    //    public string[] filesPaths1;
    //    public string[] filesPaths2;
    //    public int filesPathsCount;
    //    public int filesPaths1Count;
    //    public int filesPaths2Count;
    //    public uint dataChunkSize { get; set; }
    //    public int getDataElementsCount()
    //    {
    //        return (int)dataChunkSize / 176;
    //    }
    //    public void setfilesPaths1(int count)
    //    {
    //        //Console.WriteLine(count);
    //        filesPaths1Count = count;
    //        filesPaths1 = new string[count];
    //    }
    //    public void setfilesPaths2(int count)
    //    {
    //        //Console.WriteLine(count);
    //        filesPaths2Count = count;
    //        filesPaths2 = new string[count];
    //    }
    //    public void mergeFilesPathsArrays()
    //    {
    //        filesPaths = new string[filesPaths1.Length + filesPaths2.Length];
    //        Array.Copy(filesPaths1, filesPaths, filesPaths1.Length);
    //        Array.Copy(filesPaths2, 0, filesPaths, filesPaths1.Length, filesPaths2.Length);
    //    }
    //}
    //class DataStreamSubChunk
    //{
    //    public CryEngine.Matrix3x4 rotMatrix34;
    //    public CryEngine.Vector3 vector1;
    //    public CryEngine.Vector3 vector2;
    //    public CryEngine.Vector3 pos;
    //    public int id;
    //    public uint temp1;
    //    public DataStreamSubChunk()
    //    {
    //    }
    //    public DataStreamSubChunk(BinaryReader br, uint type)
    //    {
    //        if (type == 1) ReadType1(br);
    //        if (type == 7) ReadType7(br);
    //        if (type == 16) ReadType16(br);
    //    }
    //    public void ReadType1(BinaryReader br)
    //    {
    //        vector1.X = (float)br.ReadDouble();
    //        vector1.Y = (float)br.ReadDouble();
    //        vector1.Z = (float)br.ReadDouble();
    //        vector2.X = (float)br.ReadDouble();
    //        vector2.Y = (float)br.ReadDouble();
    //        vector2.Z = (float)br.ReadDouble();
    //        br.ReadUInt32();
    //        br.ReadUInt32();
    //        id = br.ReadInt16();
    //        temp1 = br.ReadUInt16();
    //        rotMatrix34.m00 = (float)br.ReadDouble();
    //        rotMatrix34.m01 = (float)br.ReadDouble();
    //        rotMatrix34.m02 = (float)br.ReadDouble();
    //        rotMatrix34.m03 = (float)br.ReadDouble();
    //        rotMatrix34.m10 = (float)br.ReadDouble();
    //        rotMatrix34.m11 = (float)br.ReadDouble();
    //        rotMatrix34.m12 = (float)br.ReadDouble();
    //        rotMatrix34.m13 = (float)br.ReadDouble();
    //        rotMatrix34.m20 = (float)br.ReadDouble();
    //        rotMatrix34.m21 = (float)br.ReadDouble();
    //        rotMatrix34.m22 = (float)br.ReadDouble();
    //        rotMatrix34.m23 = (float)br.ReadDouble();
    //        br.BaseStream.Position = br.BaseStream.Position + 16;

    //        pos.x = rotMatrix34.m03;
    //        pos.y = rotMatrix34.m13;
    //        pos.z = rotMatrix34.m23;
    //    }
    //    public void ReadType7(BinaryReader br)
    //    {
    //        br.BaseStream.Position = br.BaseStream.Position + 148;
    //    }
    //    public void ReadType16(BinaryReader br)
    //    {
    //        br.BaseStream.Position = br.BaseStream.Position + 132;
    //    }
    //}
}
