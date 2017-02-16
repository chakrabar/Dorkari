
namespace Dorkari.Helpers.Serialization
{
    interface ITextSerializer
    {
        T DeserializeData<T>(string jsonData);
        string SerializeData<T>(T data, bool isMinified = true);
    }
}
