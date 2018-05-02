using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using shipsExporter;
using System.Xml.Linq;

namespace charactersExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading...");
            CharactersExporter exporter = new CharactersExporter();
            exporter.Init();
            Console.WriteLine("Loaded {0} items", exporter.ItemsCount());
            Console.WriteLine("Loaded {0} loadouts", exporter.LoadoutsCount());

            if (exporter.SuccessfullyLoaded)
            {
                foreach (Loadout loadout in exporter.Loadouts)
                {
                    CDF cdf = new CDF(loadout, exporter, "objects/characters/human/male_v7/export/bhm_skeleton_v7.chr");
                    XDocument xml = cdf.GenerateCharacter();

                    string saveFolder = loadout.path;
                    string savePath;
                    saveFolder = saveFolder.Replace("\\", "/");
                    int trimLength = "./data/Scripts/Loadouts/".Length;
                    saveFolder = saveFolder.Substring(trimLength);
                    saveFolder = "exported/cdfs/" + saveFolder;
                    savePath = Path.ChangeExtension(saveFolder, "cdf");
                    saveFolder = Path.GetDirectoryName(savePath);
                    Directory.CreateDirectory(saveFolder);
                    xml.Save(savePath);
                    Console.WriteLine("SAVED: {0}", savePath);
                }
                Console.WriteLine("ALL EXPORTED");
            }
            else
            { 
                Console.WriteLine("Copy Data.p4k/Data/Game.dcb to data/Game.dcb");
                Console.WriteLine("Copy Data.p4k/Data/Scripts to data/Scripts");
            }
            Console.Read();
        }
    }
}
