using Dorkari.Framework.Logging;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Diagnostics;

namespace Dorkari.Framework.Aspects
{
    [PSerializable]
    [LinesOfCodeAvoided(4)]
    public class LogMethodExecutionAttribute : OnMethodBoundaryAspect //NOTE: Using thisneeds reference to PostSharp
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            MethodLogger.LogMethodInvokation(args.Method);

            args.MethodExecutionTag = Stopwatch.StartNew();
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            var stopwatch = (Stopwatch)args.MethodExecutionTag;
            stopwatch.Stop();

            MethodLogger.LogMethodCompletion(args.Method, stopwatch.ElapsedMilliseconds);
        }
    }
}
