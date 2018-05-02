using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipsExporter;
using System.Xml.Linq;
using System.IO;

namespace charactersExporter
{
    /// <summary>
    /// Character Definition File
    /// </summary>
    class CDF
    {
        Loadout loadout;
        CharactersExporter exporter;
        string skeleton; 
         
        public CDF(Loadout charLoadout, CharactersExporter charExporter, string skeletonPath)
        {
            loadout = charLoadout;
            skeleton = skeletonPath;
            exporter = charExporter;
        }

        public XDocument GenerateCharacter()
        {
            XDocument xmlDoc = new XDocument();

            //CharacterDefinition
            XElement characterDefinition = new XElement("CharacterDefinition");
            characterDefinition.Add(new XAttribute("CryXmlVersion", "2"));
            {
                //Model
                XElement model = new XElement("Model");
                model.Add(new XAttribute("File", skeleton));
                characterDefinition.Add(model);

                //AttachmentList
                XElement attachmentList = new XElement("AttachmentList");
                {
                    foreach(ItemPort itemPort in ListAllItemPorts(loadout.itemPorts))
                    {
                        //Attachment
                        XElement attachment = MakeAttachment(itemPort); 
                        attachmentList.Add(attachment);
                    }
                }
                characterDefinition.Add(attachmentList);
            } 
            xmlDoc.Add(characterDefinition);

            return xmlDoc;
        }
        XElement MakeAttachment(ItemPort itemPort)
        {
            Item item = exporter.GetItem(itemPort.itemName);
            string geomExtension = Path.GetExtension( item.geometry);

            string Type;
            if (geomExtension == ".skin")
                Type = "CA_SKIN";
            else
                Type = "CA_BONE";

            string AName = itemPort.portName;
            string Binding = item.geometry;
            string RelRotation = "1,0,0,0";
            string RelPosition = "0,0,0";
            string BoneName = itemPort.portName;
            string Flags = "0";
            string VisFlags = "";

            XElement attachment = new XElement("Attachment");
            attachment.Add(new XAttribute("Type", Type));
            attachment.Add(new XAttribute("AName", AName));
            if(Binding!="") attachment.Add(new XAttribute("Binding", Binding));
            attachment.Add(new XAttribute("Flags", Flags));
            if(Type == "CA_BONE")
            {
                attachment.Add(new XAttribute("RelRotation", RelRotation));
                attachment.Add(new XAttribute("RelPosition", RelPosition));
                attachment.Add(new XAttribute("BoneName", BoneName));
            }

            return attachment;
        }
        List<ItemPort> ListAllItemPorts(List<ItemPort> itemPortsToList)
        {
            List<ItemPort> itemPorts = new List<ItemPort>();

            itemPorts.AddRange(itemPortsToList.GetRange(0, itemPortsToList.Count));
            foreach(ItemPort itemPort in itemPortsToList)
            {
                itemPorts.AddRange(ListAllItemPorts(itemPort.itemPorts));
            }

            return itemPorts;
        }
    }
}
