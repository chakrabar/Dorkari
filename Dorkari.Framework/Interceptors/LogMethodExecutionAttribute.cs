using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Dorkari.Framework.Interceptors
{
    public class LogMethodExecutionAttribute : HandlerAttribute //NOTE: This will used for method decoration, ref Unity.Interception.dll
    {
        private readonly int _order;

        public LogMethodExecutionAttribute(int order)
        {
            _order = order;
        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new MethodExecutionCallHandler() { Order = _order };
        }
    }
}
