using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipsExporter;

namespace charactersExporter
{
    class CharactersExporter : Exporter
    {
        override public void Init()
        { 
            Console.WriteLine("Loading items.");
            LoadItemIterfaces("./data/Scripts/Entities/Items");
            LoadItems("./data/Scripts/Entities/Items");
            Console.WriteLine("Loading DataForge.");
            LoadDataForge("./data/Game.dcb");
            Console.WriteLine("Loading items from DataForge.");
            LoadItemsFromDataforge("./data/Game.xml");
            //loadouts
            Console.WriteLine("Loading loadouts.");
            LoadLoadouts("./data/Scripts/Loadouts/pu");
            LoadLoadouts("./data/Scripts/Loadouts/Player");
            LoadLoadouts("./data/Scripts/Loadouts/Characters");
        }
    }
}
