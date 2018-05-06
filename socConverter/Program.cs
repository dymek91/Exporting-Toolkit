using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipsExporter;
using System.IO;

namespace socConverter
{
    class Program
    {
        class Exp :Exporter
        {
            override public void Init()
            {
                try
                {
                    Console.WriteLine("Loading items.");
                    LoadItemIterfaces("./data/Scripts/Entities/Items");
                    LoadItems("./data/Scripts/Entities/Items");
                    Console.WriteLine("Loading DataForge.");
                    LoadDataForge("./data/Game.dcb");
                    Console.WriteLine("Loading items from DataForge.");
                    LoadItemsFromDataforge("./data/Game.xml"); 
                }
                catch (FormatException e)
                {
                    Console.Write("[ERROR] ");
                    Console.WriteLine(e.Message);
                    SuccessfullyLoaded = false;
                }
            }
        }
        static void Main(string[] args)
        {
            Exp exp = new Exp();
            exp.Init();

            string ocsPath = "./data/ObjectContainers";
            string ocsExportedPath = "exported/prefabs/ObjectContainers";
            Directory.CreateDirectory(ocsExportedPath);
            if (Directory.Exists(ocsPath))
            {
                foreach (string file in Directory.GetFiles(ocsPath, "*.socpak", SearchOption.AllDirectories))
                {
                    //Console.WriteLine(file);
                    string localPath = file.TrimStart(ocsPath.ToCharArray());
                    string saveFilePath = ocsExportedPath + Path.ChangeExtension( localPath,"xml");
                    Directory.CreateDirectory(Path.GetDirectoryName( saveFilePath));

                    ObjectContainer soc = new ObjectContainer(file, "", exp.items);
                    Prefab prefab = new Prefab(soc);

                    string xml = prefab.GetXml(); 
                    File.WriteAllText(saveFilePath, xml);
                    Console.WriteLine("SAVED: {0}", saveFilePath);
                }
                Console.WriteLine("ALL EXPORTED");
            }
            else
            {
                Console.WriteLine("[ERROR] NOT FOUND: {0}", ocsPath);
                Console.WriteLine("Copy Data.p4k/Data/ObjectContainers to data/ObjectContainers");
            }
            Console.Read();
        }
    }
}
