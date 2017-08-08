using Dorkari.Framework.Web.Helpers.Contracts;
using System.Web;
using System.Web.SessionState;

namespace Dorkari.Framework.Web.Helpers
{
    public class WebSessionHelper : IStateHelper
    {
        private HttpSessionState _Session
        {
            get
            {
                var httpContext = HttpContext.Current;
                return httpContext?.Session;
            }
        }
        public bool ContainsKey(string stateKey)
        {
            return (_Session != null && _Session[stateKey] != null);
        }

        public T GetData<T>(string stateKey)
        {
            if (_Session == null)
                return default(T);
            var dataFromSession = _Session[stateKey];
            return dataFromSession == null ? default(T) : (T)dataFromSession;
        }

        public void AddData(string stateKey, object data)
        {
            if (_Session != null)
                _Session.Add(stateKey, data);
        }

        public void RemoveData(string stateKey)
        {
            _Session.Remove(stateKey); //TODO: need to check existence?
        }

        public void ClearAll()
        {
            _Session.Clear();
        }
    }
}