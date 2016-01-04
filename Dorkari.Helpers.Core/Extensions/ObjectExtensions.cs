using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;

namespace Dorkari.Helpers.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToXml<T>(this T obj, string rootName = null, bool noNameSpace = false)
        {
            XmlSerializer serializer = rootName == null ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

            var xmlNs = new XmlSerializerNamespaces();
            if (noNameSpace)
            {
                xmlNs.Add(string.Empty, string.Empty);
            }

            using (StringWriter sw = new StringWriter())
            {
                if (noNameSpace)
                    serializer.Serialize(sw, obj, xmlNs);
                else
                    serializer.Serialize(sw, obj);
                return sw.ToString();
            }
        }

        public static string ToJson<T>(this T obj, bool isIndented = false)
        {
            return JsonConvert.SerializeObject(obj, isIndented ? Formatting.Indented : Formatting.None);
        }

        public static T DeepClone<T>(this T obj)
        {
            var serialized = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}
