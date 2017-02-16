using ProtoBuf;
using System;
using System.IO;

namespace Dorkari.Helpers.Serialization
{
    public class ProtoBufHelper : IBinarySerializer
    {
        public T DeserializeData<T>(byte[] data)
        {
            try
            {
                return Serializer.Deserialize<T>(new MemoryStream(data));
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public T DeserializeData<T>(string filePath)
        {
            try
            {
                var data = File.ReadAllBytes(filePath);
                return Serializer.Deserialize<T>(new MemoryStream(data));
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public bool SerializeData<T>(T data, string filePath)
        {
            try
            {
                using (var file = File.Create(filePath))
                {
                    Serializer.Serialize(file, data);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public byte[] SerializeData<T>(T data)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    Serializer.Serialize(ms, data);
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
