using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using unforge;

namespace shipsExporter
{
    public static class XDocumentHelper
    {

        /// <summary>
        /// Open xml and CryXml file.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static XDocument Load(string uri)
        {
            XDocument xDoc = new XDocument();

            System.Xml.XmlDocument xmlDoc = CryXmlSerializer.ReadFile(uri);

            if(xmlDoc!=null)
            {
                string xmlString = xmlDoc.InnerXml;
                TextReader tr = new StringReader(xmlString);
                xDoc = XDocument.Load(tr);
            }
            else
            {
                xDoc = XDocument.Load(uri);
            }
             
            return xDoc;
        }
    }
}
