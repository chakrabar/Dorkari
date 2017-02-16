using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dorkari.Helpers.Serialization
{
    public class BinarySerializationHelper : IBinarySerializer
    {
        public byte[] SerializeData<T>(T data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, data);
                return memoryStream.ToArray();
                //return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public T DeserializeData<T>(byte[] data)
        {
            //byte[] bytesData = Convert.FromBase64String(data);
            using (MemoryStream memorystreamd = new MemoryStream(data))
            {
                BinaryFormatter bf = new BinaryFormatter();
                T deserializedData = (T)bf.Deserialize(memorystreamd);
                return deserializedData;
            }
        }
    }
}
