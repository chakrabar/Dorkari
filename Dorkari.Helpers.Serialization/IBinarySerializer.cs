
namespace Dorkari.Helpers.Serialization
{
    interface IBinarySerializer
    {
        T DeserializeData<T>(byte[] data);
        byte[] SerializeData<T>(T data);
    }
}
