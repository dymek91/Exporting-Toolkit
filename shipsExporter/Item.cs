using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace shipsExporter
{ 
    class ItemInterface
    {
        public string name;
        public string geometry;
    }
    public class Item
    {
        public string name;
        public string geometry;
        public string intrface;
        public string itemclass;
        public List<PrefabAttachment> PrefabAttachments = new List<PrefabAttachment>();
        public List<ItemPort> itemPorts = new List<ItemPort>();
        public Item()
        {
                name="";
            geometry = "";
            intrface = "";
            itemclass = "";
        }

        /// <summary>
        /// Load item itemports form dataforge entityclassdefinition
        /// </summary>
        /// <param name="elItemPorts"></param>
        public void LoadItemPorts(XElement elItemPorts)
        {
            foreach(XElement elSItemPortDef in elItemPorts.XPathSelectElements("SItemPortCoreParams/Ports/SItemPortDef"))
            {
                ItemPort itemPort = new ItemPort();
                itemPort.itemName = name;
                if (elSItemPortDef.Attribute("Name")!=null)
                {
                    itemPort.portName = elSItemPortDef.Attribute("Name").Value;
                    XElement elHelper = elSItemPortDef.XPathSelectElement("AttachmentImplementation/SItemPortDefAttachmentImplementationBone/Helper/Helper");
                    if (elHelper != null)
                    {
                        itemPort.helperName = elHelper.Attribute("Name").Value;
                    }
                    itemPorts.Add(itemPort);
                }
            }
        }
        public PrefabAttachment getAttachment(string attName)
        {
            PrefabAttachment prefabAtt = new PrefabAttachment();
            foreach (PrefabAttachment pa in PrefabAttachments)
            {
                if (pa.attachmentPoint == attName) prefabAtt = pa;
            }
            return prefabAtt;
        }
        public ItemPort GetItemPort(string portName)
        {
            ItemPort itemPort = null;
            foreach (ItemPort ip in itemPorts)
            {
                if (ip.portName == portName) itemPort = ip;
            }
            return itemPort;
        }
    }
    public class PrefabAttachment
    {
        public string attachmentPoint;
        public string prefabLibrary;
        public string prefabName;
    }
}
