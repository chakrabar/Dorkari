using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dorkari.Helpers.Serialization
{
    public class BinarySerializationHelper
    {
        //public MemoryStream SerializeData<T>(T data, bool isMinified = true)
        //{
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        BinaryFormatter binaryFormatter = new BinaryFormatter();
        //        binaryFormatter.Serialize(memoryStream, data);
        //        return memoryStream;
        //    }
        //}

        public string SerializeData<T>(T data, bool isMinified = true)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, data);
                string value = Convert.ToBase64String(memoryStream.ToArray());
                return value;
            }
        }

        public T DeserializeData<T>(string data)
        {
            byte[] bytesData = Convert.FromBase64String(data);
            using (MemoryStream memorystreamd = new MemoryStream(bytesData))
            {
                BinaryFormatter bf = new BinaryFormatter();
                T deserializedData = (T)bf.Deserialize(memorystreamd);
                return deserializedData;
            }
        }
    }
}
