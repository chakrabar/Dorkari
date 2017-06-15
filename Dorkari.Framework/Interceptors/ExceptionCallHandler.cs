using Dorkari.Framework.Logging;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Dorkari.Framework.Interceptors
{
    public class ExceptionCallHandler : ICallHandler //NOTE: This will be used for policy injection
    {
        public int Order { get; set; }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var result = getNext()(input, getNext);

            if (result.Exception != null)
            {
                MethodLogger.LogMethodException(input, result.Exception);
            }

            return result;
        }
    }
}
