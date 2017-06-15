using Dorkari.Framework.Logging;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Diagnostics;

namespace Dorkari.Framework.Interceptors
{
    public class MethodExecutionCallHandler : ICallHandler //NOTE: This will be used for policy injection
    {
        public int Order { get; set; }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            MethodLogger.LogMethodInvokation(input.MethodBase);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //Invoke the next behavior in the pipeline
            var result = getNext()(input, getNext);

            stopwatch.Stop();

            if (result.Exception == null)
            {
                MethodLogger.LogMethodCompletion(input.MethodBase, stopwatch.ElapsedMilliseconds);
            } //else there is Exception which is handled in separate handler

            return result;
        }
    }
}
