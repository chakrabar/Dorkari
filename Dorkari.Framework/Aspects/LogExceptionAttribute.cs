using Dorkari.Framework.Logging;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace Dorkari.Framework.Aspects
{
    [PSerializable]
    public class LogExceptionAttribute : OnExceptionAspect //Using needs ref. to PostSharp
    {
        public override void OnException(MethodExecutionArgs args)
        {
            MethodLogger.LogMethodException(args);
        }
    }
}
