using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using unforge;

namespace CryEngine
{

    /// <summary>
    /// content of Chunk_XML
    /// </summary>
    class SCOC_Entities
    {
        byte[] serializedXML;
        string xml;
        bool isSerialized = false;

        public SCOC_Entities(byte[] xmlStream)
        {
            using (MemoryStream stream = new MemoryStream(xmlStream))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    if(IsSerialized(br))
                    {
                        serializedXML = new byte[xmlStream.Length];
                        serializedXML = xmlStream;
                        isSerialized = true;
                    }
                    else
                    {
                        xml = Encoding.UTF8.GetString(xmlStream);
                    }
                } 
            }
            if (isSerialized)
            {
                xml = CryXmlSerializer.ReadBytes(xmlStream).InnerXml;
            }
        }
        public string GetXML()
        {
            return xml;
        }

        bool IsSerialized(BinaryReader br)
        {
            bool isSerialized = false;

            br.BaseStream.Position = 0;
            uint signatureString = br.ReadUInt32();
            if (signatureString == 1484354115)//CryX
            {
                isSerialized = true;
            }
            br.BaseStream.Position = 0;
            return isSerialized;
        }
    }
}
