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

namespace shipsExporter
{
    /// <summary>
    /// no needed atm - normal ObjectContainer class can handle 3.0.0
    /// </summary>
    //class ObjectContainer_SC_3_0_0
    //{
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
    //    public List<ObjectContainer_SC_3_0_0> childs = new List<ObjectContainer_SC_3_0_0>();
    //    //AreaShapeChunk areaShapeChunk;
    //    Random random = new Random();

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="path">socpak archive path</param>
    //    /// <param name="port">portName</param>
    //    public ObjectContainer_SC_3_0_0(string path, string port)
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
    //                childs.Add(new ObjectContainer_SC_3_0_0(path, port, childname));
    //            }
    //            //Console.WriteLine(childs.Count);
    //        }

    //    }
    //    ObjectContainer_SC_3_0_0(string path, string port, string childname)
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
    //            foreach (ZipArchiveEntry entry in zip.Entries)
    //            {
    //                //Console.WriteLine(entry.Name);
    //                if (entry.Name == name.ToLower() + ".xml")
    //                    entryname = entry.Name;
    //            }
    //            if (entryname == "")
    //            {
    //                Console.WriteLine("entryname=null");
    //                foreach (ZipArchiveEntry entry in zip.Entries)
    //                {
    //                    Console.WriteLine(entry.Name);
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
    //            childsNames.Add(el.Attribute("name").Value);
    //            //Console.WriteLine(el.Attribute("name").Value);
    //        }

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
    //                elObject.Add(new XAttribute("LodRatio", "10"));
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

    //    /// <summary>
    //    /// Exctract GeomEntity objects from XML chunk
    //    /// </summary>
    //    /// <param name="xml"></param>
    //    void extractGeomEntity(string xmlstr)
    //    {
    //        try
    //        {
    //            if (xmlstr.Length > 0)
    //            {
    //                TextReader tr = new StringReader(xmlstr);
    //                XDocument xml = XDocument.Load(tr);
    //                string layerguid = Guid.NewGuid().ToString();
    //                foreach (XElement el in xml.Descendants("Entity"))
    //                {
    //                    if (el.Attribute("EntityClass") != null) if (el.Attribute("EntityClass").Value == "GeomEntity")
    //                        {
    //                            PrefabObject prObj = new PrefabObject();
    //                            prObj.Name = el.Attribute("Name").Value;
    //                            prObj.Pos = el.Attribute("Pos").Value;
    //                            if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
    //                            prObj.EntityClass = el.Attribute("EntityClass").Value;
    //                            prObj.Type = el.Attribute("EntityClass").Value;
    //                            prObj.Id = Guid.NewGuid().ToString();
    //                            prObj.Layer = el.Attribute("Layer").Value;
    //                            prObj.Geometry = el.Attribute("Geometry").Value;
    //                            byte[] asciiBytes = Encoding.ASCII.GetBytes(el.Attribute("Layer").Value);
    //                            prObj.LayerGUID = layerguid;
    //                            prObj.LodRatio = "10";
    //                            prefabObjects.Add(prObj);
    //                        }
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("|{0}|", xmlstr);
    //            Console.Read();
    //            throw;
    //        }
    //    }
    //    void extractLightEntity(string xmlstr)
    //    {
    //        try
    //        {
    //            if (xmlstr.Length > 0)
    //            {
    //                TextReader tr = new StringReader(xmlstr);
    //                XDocument xml = XDocument.Load(tr);
    //                string layerguid = Guid.NewGuid().ToString();
    //                foreach (XElement el in xml.Descendants("Entity"))
    //                {
    //                    if (el.Attribute("EntityClass") != null) if (el.Attribute("EntityClass").Value == "Light")
    //                        {
    //                            PrefabObject prObj = new PrefabObject();
    //                            prObj.Name = el.Attribute("Name").Value;
    //                            prObj.Pos = el.Attribute("Pos").Value;
    //                            if (el.Attribute("Rotate") != null) prObj.Rotate = el.Attribute("Rotate").Value;
    //                            if (el.Attribute("Scale") != null) prObj.Scale = el.Attribute("Scale").Value;
    //                            prObj.EntityClass = el.Attribute("EntityClass").Value;
    //                            prObj.Type = el.Attribute("EntityClass").Value;
    //                            prObj.Id = Guid.NewGuid().ToString();
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
    //    void extractEnvironmentLightEntity(string xmlstr)
    //    {
    //        try
    //        {
    //            if (xmlstr.Length > 0)
    //            {
    //                TextReader tr = new StringReader(xmlstr);
    //                XDocument xml = XDocument.Load(tr);
    //                string layerguid = Guid.NewGuid().ToString();
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
    //                            prObj.Id = Guid.NewGuid().ToString();
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
    //                            prObj.Id = Guid.NewGuid().ToString();
    //                            prObj.Points = el.Element("Points");
    //                            prObj.DisplayFilled = el.Attribute("DisplayFilled").Value;
    //                            prObj.Height = el.Attribute("Height").Value;
    //                            prObj.VisAreaPos = el.Attribute("Pos").Value;
    //                            VisAreaPos = el.Attribute("Pos").Value;
    //                            prObj.FixPoints();
    //                           // PrefabObject prObjFromChunk = areaShapeChunk.GetPrefabObjByName(el.Attribute("Name").Value);
    //                            //if (prObjFromChunk.Name != null)
    //                            //{
    //                            //    prObj.Pos = prObjFromChunk.Pos;
    //                            //    prObj.Rotate = prObjFromChunk.Rotate;
    //                            //    prObj.Height = prObjFromChunk.Height;
    //                            //    prObj.Points = prObjFromChunk.Points;
    //                            //}
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
    //                            prObj.Id = Guid.NewGuid().ToString();
    //                            prObj.Points = el.Element("Points");
    //                            prObj.DisplayFilled = el.Attribute("DisplayFilled").Value;
    //                            prObj.Height = el.Attribute("Height").Value;
    //                            prObj.LightBlending = el.Attribute("LightBlending").Value;
    //                            prObj.LightBlendValue = el.Attribute("LightBlendValue").Value;
    //                            if (VisAreaPos != null) prObj.VisAreaPos = VisAreaPos; else { Console.WriteLine(socpakPath); Console.WriteLine(el.Attribute("Name").Value); };
    //                            if (name == "frnt") Console.WriteLine(VisAreaPos);
    //                            prObj.FixPoints();
    //                            //PrefabObject prObjFromChunk = areaShapeChunk.GetPrefabObjByName(el.Attribute("Name").Value);
    //                            //if (prObjFromChunk.Name != null)
    //                            //{
    //                            //    prObj.Pos = prObjFromChunk.Pos;
    //                            //    prObj.Rotate = prObjFromChunk.Rotate;
    //                            //    prObj.Height = prObjFromChunk.Height;
    //                            //    prObj.Points = prObjFromChunk.Points;
    //                            //}
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
    //            Console.WriteLine(path);
    //            string entryname = "";
    //            foreach (ZipArchiveEntry entry in zip.Entries)
    //            {
    //                //Console.WriteLine(entry.Name);
    //                if (entry.Name == name.ToLower() + ".soc")
    //                    entryname = entry.Name;
    //            }
    //            if (entryname == "")
    //            {
    //                Console.WriteLine("entryname=null");
    //                foreach (ZipArchiveEntry entry in zip.Entries)
    //                {
    //                    Console.WriteLine(entry.Name);
    //                }
    //            }
    //            byte[] asciiBytes = Encoding.ASCII.GetBytes(entryname);
    //            layerGUID = Guid.NewGuid().ToString();
    //            prefabGUID = Guid.NewGuid().ToString();
    //            Console.WriteLine("entryname {0}", entryname);
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
    //                prefabObj.Id = Guid.NewGuid().ToString();
    //                //////////////////////////
    //                uint subchunkType = br.ReadUInt32();
    //                DataStreamSubChunk subchunk = new DataStreamSubChunk(br, subchunkType);
    //                if (subchunkType == 7) continue;
    //                double x = subchunk.pos.x;
    //                double y = subchunk.pos.y;
    //                double z = subchunk.pos.z;
    //                int id = subchunk.id;
    //                CryEngine.Quaternion quat = new CryEngine.Quaternion(subchunk.rotMatrix34);
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
    //                prefabObj.LayerGUID = Guid.NewGuid().ToString();
    //                prefabObjects.Add(prefabObj);

    //            }

    //        }
    //        if (AreaShapeChunkOffset != 0)
    //        {
    //            Console.WriteLine(AreaShapeChunkOffset);
    //            br.BaseStream.Position = AreaShapeChunkOffset;
    //            //areaShapeChunk = new AreaShapeChunk(br, socpakPath);
    //            //foreach(PrefabObject prefabObj in areaShapeChunk.prefabObjects)
    //            //{

    //            //}
    //        }

    //        br.BaseStream.Position = xmlChunkOffset;
    //        string xmlSCOCEntities = new string(br.ReadChars((int)xmlChunkSize)).TrimEnd('\0').TrimEnd(Environment.NewLine.ToCharArray()); ;
    //        br.Close();
    //        //Console.WriteLine(xmlSCOCEntities);
    //        //Console.WriteLine("end");
    //        // extractGeomEntity(xmlSCOCEntities);
    //        // extractLightEntity(xmlSCOCEntities);
    //        // extractEnvironmentLightEntity(xmlSCOCEntities);
    //        // extractVisAreaEntity(xmlSCOCEntities);
    //        // extractPortalEntity(xmlSCOCEntities);
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
    //}
    

}
