using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Dorkari.Helpers.Core.Xml
{
    public class XmlHelper
    {
        public static XDocument ReadXMLContent(string xmlFilePath)
        {
            if (string.IsNullOrEmpty(xmlFilePath))
                return null;
            string content = File.ReadAllText(xmlFilePath);
            return XDocument.Parse(content);
        }

        public static string GetFirstNodeValue(XDocument xDoc, string nodeName)
        {
            //incase of multiple nodes with same Name,this one will return value of first node only
            var node = xDoc.Descendants().Where(p => p.Name.LocalName == nodeName).FirstOrDefault();
            if (node != null)
                return node.Value;
            return string.Empty;
        }

        public static string GetFirstChildValueOfFirstParent(XDocument xDoc, string parentNode, string childNode)
        {
            //incase of multiple nodes with same Name,this one will return value of first node only
            var node = xDoc.Descendants().Where(p => p.Name.LocalName == parentNode).FirstOrDefault();
            if (node != null)
            {
                var innerNode = node.Descendants().Where(p => p.Name.LocalName == childNode).FirstOrDefault();
                if (innerNode != null)
                    return innerNode.Value;
            }
            return string.Empty;
        }

        public static string WriteToXML(XDocument XDoc, string directory, string fileName)
        {
            if (!Directory.Exists(directory) || string.IsNullOrEmpty(fileName))
                throw new DirectoryNotFoundException("Directory not found : " + directory + " or filename is empty");

            var filePath = Path.Combine(directory, fileName);
            XDoc.Save(filePath);
            return filePath;
        }
    }
}
