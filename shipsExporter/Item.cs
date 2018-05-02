using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Item()
        {
                name="";
            geometry = "";
            intrface = "";
            itemclass = "";
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
    }
    public class PrefabAttachment
    {
        public string attachmentPoint;
        public string prefabLibrary;
        public string prefabName;
    }
}
