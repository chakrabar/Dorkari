using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;

namespace Dorkari.Helpers.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToXml<T>(this T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringWriter sw = new StringWriter())
            {
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
