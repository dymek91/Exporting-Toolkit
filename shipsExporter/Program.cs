using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;


namespace shipsExporter
{
    class Program
    {
        static string PromtPaints(ShipImplementation ship)
        {
            string selectedPaint = null;
            if (ship.paints.Count > 0)
            {
                Console.WriteLine("Available paints:");
                foreach (Paint pt in ship.paints)
                {
                    Console.WriteLine("{0}|{1}", pt.name, pt.material);
                }
                Console.WriteLine("Enter paint name:");
                selectedPaint = Convert.ToString(Console.ReadLine()).Trim();
            }
            return selectedPaint;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Loading...");
            ShipsExporter exp = new ShipsExporter();
            exp.Init();
            Console.WriteLine("Loaded {0} iterfaces", exp.ItemIterfacesCount());
            Console.WriteLine("Loaded {0} items", exp.ItemsCount());
          //  Console.WriteLine("Loaded {0} loadouts", exp.LoadoutsCount());
            Console.WriteLine("Loaded {0} ships", exp.ShipsCount());
            Console.WriteLine("------------------------------------------");

            if (exp.SuccessfullyLoaded)
            {
                foreach (ShipDefinition ship in exp.GetShips())
                {
                    if (ship.GetShipImplementation() != null)
                    {
                        //Console.WriteLine(ship.GetShipName());
                        EntityPrefab_CE54_Modded prefab = new EntityPrefab_CE54_Modded(exp);
                        XDocument xml = prefab.GenerateShip(ship);
                        string vehicleType = ship.GetVehicleType();
                        string manufacturer = ship.GetManufacturer();
                        Directory.CreateDirectory("exported/prefabs/" + vehicleType + "/" + manufacturer);
                        xml.Save("exported/prefabs/" + vehicleType + "/" + manufacturer + "/" + ship.GetShipName() + ".xml");
                        Console.WriteLine("SAVED: {0}", "exported/prefabs/" + vehicleType + "/" + manufacturer + "/" + ship.GetShipName() + ".xml");
                    }
                }
                Console.WriteLine("ALL EXPORTED");
            }
            else
            {
                Console.WriteLine("Copy Data.p4k/Data/ObjectContainers to data/ObjectContainers");
                Console.WriteLine("Copy Data.p4k/Data/Game.dcb to data/Game.dcb");
                Console.WriteLine("Copy Data.p4k/Data/Scripts to data/Scripts");
            }
            Console.Read();
        }
        ///////////
         
    }
}
