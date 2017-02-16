using System;
using JSON = Newtonsoft.Json.JsonConvert;

namespace Dorkari.Helpers.Serialization
{
    public class JsonHelper : ITextSerializer
    {
        public string SerializeData<T>(T data, bool isMinified = true)
        {
            var serializedData = isMinified ?
                JSON.SerializeObject(data, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.Converters.StringEnumConverter()) :
                JSON.SerializeObject(data, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter());
            return serializedData;
        }

        public T DeserializeData<T>(string jsonData)
        {
            try
            {
                return JSON.DeserializeObject<T>(jsonData);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
