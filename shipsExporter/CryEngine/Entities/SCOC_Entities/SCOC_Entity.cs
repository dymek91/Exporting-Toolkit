using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using shipsExporter;

namespace CryEngine
{ 
    class SCOC_Entity
    { 
        public string Name { get; set; }
        public string Material { get; set; }
        public string Pos { get; set; }
        public string Rotate { get; set; }
        public string Scale { get; set; }
        public string EntityClass { get; set; }
        public string EntityId { get; set; }
        public string EntityGuid { get; set; }
        public string EntityCryGUID { get; set; }
        public string CastShadowMinSpec { get; set; }
        public string Layer { get; set; }
        public string LayerGUID { get; set; }

        public XElement Entity { get; set; }
        public XElement Properties { get; set; }//

        PropertiesDataCore propertiesDataCore;

        public SCOC_Entity(XElement elEntity)
        {
            Entity = elEntity;
            if (Entity.Element("Properties") != null) Properties = Entity.Element("Properties");

            if (elEntity.Attribute("Name") != null) Name = elEntity.Attribute("Name").Value;
            if (elEntity.Attribute("Material") != null) Material = elEntity.Attribute("Material").Value;
            if (elEntity.Attribute("Pos") != null) Pos = elEntity.Attribute("Pos").Value;
            if (elEntity.Attribute("Rotate") != null) Rotate = elEntity.Attribute("Rotate").Value;
            if (elEntity.Attribute("Scale") != null) Scale = elEntity.Attribute("Scale").Value;
            if (elEntity.Attribute("EntityClass") != null) EntityClass = elEntity.Attribute("EntityClass").Value;
            if (elEntity.Attribute("EntityId") != null) EntityId = elEntity.Attribute("EntityId").Value;
            if (elEntity.Attribute("EntityGuid") != null) EntityGuid = elEntity.Attribute("EntityGuid").Value;
            if (elEntity.Attribute("EntityCryGUID") != null) EntityCryGUID = elEntity.Attribute("EntityCryGUID").Value;
            if (elEntity.Attribute("CastShadowMinSpec") != null) CastShadowMinSpec = elEntity.Attribute("CastShadowMinSpec").Value;
            if (elEntity.Attribute("Layer") != null) Layer = elEntity.Attribute("Layer").Value;
            LayerGUID = GuidUtility.GenLayerID(Layer);

            if (elEntity.Element("PropertiesDataCore") != null) propertiesDataCore = new PropertiesDataCore(elEntity.Element("PropertiesDataCore"));
        }
         
        public PrefabObject GetGeomAsPrefabObject(List<Item> itemsList)
        { 
            PrefabObject prefabObj = null;

            if (propertiesDataCore!=null)
            {
                prefabObj = new PrefabObject();
                prefabObj.Id = GuidUtility.GenID();
                prefabObj.Name = Name;
                prefabObj.Layer = Layer;
                prefabObj.LayerGUID = LayerGUID;
                if (Pos != null) prefabObj.Pos = Pos;
                if (Rotate != null) prefabObj.Rotate = Rotate;
                if (Scale != null) prefabObj.Scale = Scale;
                if (Material != null) prefabObj.Material = Material;

                if(Entity.Attribute("Geometry") != null)
                {
                    prefabObj.Geometry = Entity.Attribute("Geometry").Value;
                    prefabObj.Type = "GeomEntity";
                    prefabObj.EntityClass = "GeomEntity";
                }
                else if (propertiesDataCore.Geometry!="")
                { 
                    prefabObj.Geometry = propertiesDataCore.Geometry;
                    prefabObj.Type = "GeomEntity";
                    prefabObj.EntityClass = "GeomEntity";
                    if (propertiesDataCore.Material != "")
                    {
                        prefabObj.Material = propertiesDataCore.Material;
                    }
                    if(Material!=null)
                    {
                        prefabObj.Material = Material;
                    }
                }
                else if(propertiesDataCore.Loadout!=null)
                {
                    Item entityItem = null;
                    foreach (Item it in itemsList)
                    {
                        if (it.name == EntityClass) entityItem = it;
                    }
                    if (entityItem != null)
                    {
                        if (entityItem.geometry != "")
                        {
                            prefabObj.Geometry = entityItem.geometry;
                            prefabObj.Type = "GeomEntity";
                            prefabObj.EntityClass = "GeomEntity";

                            foreach(ItemPort loadoutItemPort in propertiesDataCore.Loadout.itemPorts)
                            { 
                                //Item childItem = exporter.GetItem(loadoutItemPort.itemName);
                                Item childItem = null;
                                foreach (Item it in itemsList)
                                {
                                    if (it.name == loadoutItemPort.itemName) childItem = it;
                                }
                                if (childItem!=null)
                                {
                                    if (childItem.geometry != "")
                                    {
                                        PrefabObject childObj = new PrefabObject();

                                        ItemPort targetItemPort = entityItem.GetItemPort(loadoutItemPort.portName);
                                        if (targetItemPort != null)
                                        {
                                            string targetJointName = targetItemPort.helperName;

                                            childObj.Geometry = childItem.geometry;
                                            childObj.Id = GuidUtility.GenID();
                                            childObj.Name = loadoutItemPort.itemName;
                                            childObj.Layer = Layer;
                                            childObj.LayerGUID = LayerGUID;
                                            childObj.Pos = "0,0,0";
                                            childObj.Rotate = "1,0,0,0";
                                            childObj.Type = "GeomEntity";
                                            childObj.EntityClass = "GeomEntity";
                                            childObj.ParentId = prefabObj.Id;
                                            childObj.AttachmentType = "CharacterBone";
                                            if (targetJointName!=null)
                                            {
                                                childObj.AttachmentTarget = targetJointName;
                                            }
                                            else
                                            {
                                                childObj.AttachmentTarget = targetItemPort.portName;
                                            }

                                            prefabObj.attachments.Add(childObj);
                                        }
                                    }
                                }
                            } 
                        }
                    }
                }
                else
                {
                    Item entityItem = null;
                    foreach (Item it in itemsList)
                    {
                        if (it.name == EntityClass) entityItem = it;
                    }
                    if (entityItem != null)
                    {
                        if (entityItem.geometry != "")
                        {
                            prefabObj.Geometry = entityItem.geometry;
                            prefabObj.Type = "GeomEntity";
                            prefabObj.EntityClass = "GeomEntity";

                            prefabObj.Name = Name;
                            prefabObj.Layer = Layer;
                            prefabObj.LayerGUID = LayerGUID;
                            if (Pos != null) prefabObj.Pos = Pos;
                            if (Rotate != null) prefabObj.Rotate = Rotate;
                            if (Scale != null) prefabObj.Scale = Scale;
                            if (Material != null) prefabObj.Material = Material; 
                        }
                    }
                }
            }
            else
            {
                if(EntityClass== "AnimObject")
                {
                    prefabObj = new PrefabObject();
                    prefabObj.Id = GuidUtility.GenID();

                    prefabObj.Type = "AnimObject";
                    prefabObj.EntityClass = "AnimObject";

                    prefabObj.Name = Name;
                    prefabObj.Layer = Layer;
                    prefabObj.LayerGUID = LayerGUID;
                    if (Pos != null) prefabObj.Pos = Pos;
                    if (Rotate != null) prefabObj.Rotate = Rotate;
                    if (Scale != null) prefabObj.Scale = Scale;
                    if (Material != null) prefabObj.Material = Material;

                    prefabObj.Properties = Properties;
                }
                if (EntityClass == "GeomEntity")
                {

                }
            }

            //delete %level% geoms
            if (prefabObj != null)
            {
                if (prefabObj.Geometry != null)
                {
                    if (prefabObj.Geometry.StartsWith("%level%", StringComparison.OrdinalIgnoreCase))
                    {
                        prefabObj.Geometry = null;
                    }
                }
            }

            return prefabObj;
        } 

        private class PropertiesDataCore
        {
            XElement propertiesDataCore;
            public Loadout Loadout { get; }
            SGeometryDataParams sGeometryDataParams;

            public string Geometry { get; } = "";
            public string Material { get; } = "";

            public PropertiesDataCore(XElement inPropertiesDataCore)
            {
                propertiesDataCore = inPropertiesDataCore;

                if(propertiesDataCore.Element("EntityComponentDefaultLoadout") !=null)
                {
                    string sdfsdf = "dfg";
                }
                //load loadout
                XElement elLoudout = propertiesDataCore.XPathSelectElement("EntityComponentDefaultLoadout/loadout");
                if(elLoudout!=null)
                {
                    string loadoutPath = null;
                    if(elLoudout.Attribute("loadoutPath")!=null) loadoutPath=elLoudout.Attribute("loadoutPath").Value;
                    if(loadoutPath!=null)
                    {
                        loadoutPath = "./data/" + loadoutPath;
                        Loadout = new Loadout(loadoutPath);
                        if (Loadout.name == null) Loadout = null;
                    }
                }

                sGeometryDataParams = new SGeometryDataParams(propertiesDataCore);
                Geometry = sGeometryDataParams.Geometry;
                Material = sGeometryDataParams.Material;
            }

            private class SGeometryDataParams
            {
                public string Geometry { get; set; } = "";
                public string Material { get; set; } = "";

                public SGeometryDataParams(XElement inPropertiesDataCore)
                {
                    XElement sGeometryDataParams = inPropertiesDataCore.XPathSelectElement("EntityGeometryResource/Geometry/Geometry");

                    if (sGeometryDataParams != null)
                    {
                        XElement elGeometry = sGeometryDataParams.Element("Geometry");
                        XElement elMaterial = sGeometryDataParams.Element("Material");
                         
                        if (elGeometry != null)
                        {
                            Geometry = elGeometry.Attribute("path").Value;
                        }

                        if (elMaterial != null)
                        {
                            Material = elMaterial.Attribute("path").Value;
                        }
                    }
                }
            }
        }
    }
}
