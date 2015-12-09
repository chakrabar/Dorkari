using System.IO;
using System.Xml.Serialization;

namespace Dorkari.Helpers.Serialization
{
    public class XmlHelper
    {
        public T DeserializeData<T>(string data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(data);
            T result = (T)serializer.Deserialize(reader);
            return result;
        }

        public string SerializeData<T>(T data, bool isMinified = true)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, data);
            var xmlString = sw.ToString();
            return xmlString;
        }
    }
}
