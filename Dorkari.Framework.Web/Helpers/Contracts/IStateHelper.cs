namespace Dorkari.Framework.Web.Helpers.Contracts
{
    public interface IStateHelper
    {
        bool ContainsKey(string stateKey);
        T GetData<T>(string stateKey);
        void AddData(string stateKey, object data);
        void RemoveData(string stateKey);
        void ClearAll();
    }
}