using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace shipsExporter
{
    public class Loadout
    {
        public Loadout parentLoadout;
        public List<ItemPort> itemPorts = new List<ItemPort>();
        public Quality quality;
        public string name;
        public string path;

        public Loadout()
        {

        }
        public Loadout(string filePath)
        { 
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            if (File.Exists(filePath))
            {
                XDocument xmlDoc = XDocumentHelper.Load(filePath);
                XElement element = xmlDoc.Element("Loadout");
                Load(element, fileName, filePath);
            }
        }
        public Loadout(XElement element, string loadoutName, string loadoutPath)
        { 
            Load(element, loadoutName, loadoutPath);
        }
        public Loadout(XDocument xmlDoc,string loadoutName,string loadoutPath)
        { 
            XElement element = xmlDoc.Element("Loadout");
            Load(element,  loadoutName,  loadoutPath);
        }
        void Load(XElement el, string loadoutName, string loadoutPath)
        {
            XElement element = el;
            if (element != null)
            {
                name = loadoutName;
                path = loadoutPath;

                if (element.Element("Loadouts") != null)
                {
                    if (element.Element("Loadouts").Element("Loadout") != null)
                    {
                        XElement parentLoudoutEl = element.Element("Loadouts").Element("Loadout");
                        string parentLoudoutPath = "./data/" + parentLoudoutEl.Attribute("loadout").Value;

                        XDocument xml = XDocumentHelper.Load(parentLoudoutPath);
                        string name = Path.GetFileNameWithoutExtension(parentLoudoutPath); ;

                        parentLoadout = new Loadout(xml, name, parentLoudoutPath);
                    }
                }
                if (element.Element("Items") != null)
                    itemPorts.AddRange(LoadItemPorts(element.Element("Items")));
                if (element.Element("Quality") != null)
                {
                    quality = new Quality();
                    XElement qualityEl = element.Element("Quality");
                    if (qualityEl.Attribute("wear") != null) quality.wear = qualityEl.Attribute("wear").Value;
                    if (qualityEl.Attribute("dirt") != null) quality.dirt = qualityEl.Attribute("dirt").Value;
                }
            }
        }

        public ItemPort GetItemPort(string itemPortName)
        {
            ItemPort itemPort = new ItemPort();
            foreach (ItemPort ip in itemPorts)
            {
                if (ip.portName == itemPortName) itemPort = ip;
            }
            return itemPort;
        }
        List<ItemPort> LoadItemPorts(XElement itemsEl)
        {
            List<ItemPort> itemPorts = new List<ItemPort>();

            foreach (XElement el in itemsEl.Elements("Item"))
            {
                ItemPort itemport = new ItemPort();
                if (el.Attribute("portName") != null) itemport.portName = el.Attribute("portName").Value;
                if (el.Attribute("itemName") != null) itemport.itemName = el.Attribute("itemName").Value;
                if (el.Attribute("tag") != null) itemport.tag = el.Attribute("tag").Value;
                if (el.Attribute("wear") != null) itemport.wear = el.Attribute("wear").Value;
                if (el.Attribute("dirt") != null) itemport.dirt = el.Attribute("dirt").Value;

                if (el.Element("Items") != null)
                    itemport.itemPorts.AddRange(LoadItemPorts(el.Element("Items")));

                itemPorts.Add(itemport);
            }

            return itemPorts;
        }
    }
    public class ItemPort
    {
        public List<ItemPort> itemPorts = new List<ItemPort>();
        public string portName;
        public string itemName;
        public string helperName;
        public string tag;
        public string wear;
        public string dirt;
        public ItemPort()
        {
              portName="";
              itemName = "";
        }
    }
    public class Quality
    {
        public string wear;
        public string dirt;
    }
}
