using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace shipsExporter
{

    /// <summary>
    /// From DataForge;  
    /// EntityClassDefinition;
    /// All from:
    /// libs/foundry/records/vehicle/spaceship,
    /// libs/foundry/records/vehicle/groundvehicle
    /// </summary>
    class ShipDefinition
    {
        string shipName; //EntityClassDefinition.###NAME###
        string shipImplemetationPath;//VehicleComponentParams  vehicleDefinition
        string shipLoadoutPath;//SItemPortLoadoutXMLParams loadoutPath

        string modificationName;
        string paintName;

        string manufacturer;
        string vehicleType;

        ShipImplementation shipImplementation;
        Loadout shipLoadout;

        List<Item> itemsList;

        public ShipDefinition(string xmlEntityClassDefinition)
        {

        }
        public ShipDefinition(XElement xmlDoc, string in_manufacturer, string in_vehicleType, List<Item> items)
        {
            itemsList = items;
            manufacturer= in_manufacturer;
            vehicleType= in_vehicleType;

            string[] words =  xmlDoc.Name.ToString().Split('.');
            shipName = words[1];

            foreach (XElement el in xmlDoc.Descendants("VehicleComponentParams"))
            {
                shipImplemetationPath = el.Attribute("vehicleDefinition").Value;
                modificationName = el.Attribute("modification").Value;
                paintName = el.Attribute("defaultPaint").Value; 
            } 
            foreach(XElement el in xmlDoc.Descendants("SItemPortLoadoutXMLParams"))
            {
                shipLoadoutPath = el.Attribute("loadoutPath").Value;
            }

            LoadLoadout(shipLoadoutPath);
            LoadShipImplementation(shipImplemetationPath);
        }
        public Loadout GetLoadout()
        {
            return shipLoadout;
        }
        public string GetShipName()
        {
            return shipName;
        }
        public string GetManufacturer()
        {
            return manufacturer;
        }
        public string GetVehicleType()
        {
            return vehicleType;
        }
        public ShipImplementation GetShipImplementation()
        {
            return shipImplementation;
        }
        public Paint GetShipPaint()
        {
            Paint paint=null;
            paint = shipImplementation.GetPaintByName(paintName);
            return paint;
        }
        void LoadLoadout(string path)
        {
            string filePath = "./data/" +path;
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            if(File.Exists(filePath))
            {
                XDocument xmlDoc = XDocumentHelper.Load(filePath);
                shipLoadout = new Loadout(xmlDoc, fileName, filePath);
            }
        }
        void LoadShipImplementation(string path)
        {
            string filePath = "./data/" + path;
            string fileFolder = Path.GetDirectoryName(filePath);
            if (File.Exists(filePath))
            {
                XDocument xmlDoc = XDocumentHelper.Load(filePath);
                shipImplementation = new ShipImplementation(xmlDoc, fileFolder, itemsList);
            }
        }

    }
}
