using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq; 

namespace CryEngine
{
    class EntityWithComponent
    {
        public string Type { get; } = "EntityWithComponent";
        public string EntityClass { get; set; } = "Entity";
        public string Layer { get; set; } 
        public string LayerGUID { get; set; }
        public string LinkedTo { get; set; }
        public string AttachmentType { get; set; } = "CharacterBone";
        public string AttachmentTarget { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Pos { get; set; } = "0,0,0";
        public string Rotate { get; set; } = "1,0,0,0";
        public string Scale { get; set; } = "1,1,1";
        public string ColorRGB { get; set; } = "65535";
        public string MatLayersMask { get; set; } = "0";
        public string OutdoorOnly { get; set; } = "0";
        public string CastShadow { get; set; } = "1";
        public string CastShadowMinspec { get; set; } = "1";
        public string DynamicDistanceShadows { get; set; } = "0";
        public string GIMode { get; set; } = "0";
        public string LodRatio { get; set; } = "100";
        public string ViewDistRatio { get; set; } = "100";
        public string HiddenInGame { get; set; } = "0";
        public string RecvWind { get; set; } = "0";
        public string RenderNearest { get; set; } = "0";
        public string NoStaticDecals { get; set; } = "0";
        public string HasEntity { get; set; } = "1";

        public List<Component> Components { get; } = new List<Component>();

        public XElement Get()
        {
            XElement elObject = new XElement("Object");

            elObject.Add(new XAttribute("Type", Type));
            elObject.Add(new XAttribute("EntityClass", EntityClass));
            elObject.Add(new XAttribute("Layer", Layer));
            elObject.Add(new XAttribute("LayerGUID", LayerGUID));
            if (LinkedTo != null)
            {
                elObject.Add(new XAttribute("LinkedTo", LinkedTo));
            }
            if(AttachmentTarget != null)
            { 
                elObject.Add(new XAttribute("AttachmentType", AttachmentType));
                elObject.Add(new XAttribute("AttachmentTarget", AttachmentTarget));
            }
            elObject.Add(new XAttribute("Id", Id));
            elObject.Add(new XAttribute("Name", Name));
            elObject.Add(new XAttribute("Pos", Pos));
            elObject.Add(new XAttribute("Rotate", Rotate));
            elObject.Add(new XAttribute("Scale", Scale));
            elObject.Add(new XAttribute("ColorRGB", ColorRGB));
            elObject.Add(new XAttribute("MatLayersMask", MatLayersMask));
            elObject.Add(new XAttribute("OutdoorOnly", OutdoorOnly));
            elObject.Add(new XAttribute("CastShadow", CastShadow));
            elObject.Add(new XAttribute("CastShadowMinspec", CastShadowMinspec));
            elObject.Add(new XAttribute("DynamicDistanceShadows", DynamicDistanceShadows));
            elObject.Add(new XAttribute("GIMode", GIMode));
            elObject.Add(new XAttribute("LodRatio", LodRatio));
            elObject.Add(new XAttribute("ViewDistRatio", ViewDistRatio));
            elObject.Add(new XAttribute("HiddenInGame", HiddenInGame));
            elObject.Add(new XAttribute("RecvWind", RecvWind));
            elObject.Add(new XAttribute("RenderNearest", RenderNearest));
            elObject.Add(new XAttribute("NoStaticDecals", NoStaticDecals));
            elObject.Add(new XAttribute("HasEntity", HasEntity));

            XElement components = new XElement("Components");
            foreach (Component component in Components)
            {
                components.Add(component.Get());
            }
            elObject.Add(components);

            return elObject;
        }
    }
}
