using Dorkari.Framework.Contracts;
using Microsoft.Practices.Unity.InterceptionExtension;
using PostSharp.Aspects;
using System;
using System.Reflection;

namespace Dorkari.Framework.Logging
{
    public static class MethodLogger
    {
        static readonly ILogger _logger;

        static MethodLogger()
        {
            _logger = new FileLogger(); //TODO: to be injected
        }

        public static void LogMethodInvokation(MethodBase method)
        {
            _logger.LogInfo("Method Invoked", string.Format("Method {0} invoked.", GetMethodString(method)));
        }

        public static void LogMethodCompletion(MethodBase method, long elapsedMilliseconds)
        {
            _logger.LogInfo("Method Returned", string.Format("Method {0} returned. Total milliseconds = {1}", GetMethodString(method), elapsedMilliseconds));
        }

        public static void LogMethodException(MethodExecutionArgs args)
        {
            var argDetails = "Arguments: ";
            if (args.Arguments.Count > 0)
            {
                for (int i = 0; i < args.Arguments.Count; i++)
                {
                    argDetails += string.Format("{0} {1} ",
                        args.Arguments[i] ?? "null",
                        Environment.NewLine);
                }
            }
            var title = string.Format("Exception thrown from " + GetMethodString(args.Method));
            _logger.LogException(title, argDetails, args.Exception);
        }

        public static void LogMethodException(IMethodInvocation input, Exception ex)
        {
            var args = "Arguments: ";
            if (input.Arguments.Count > 0)
            {
                for (int i = 0; i < input.Arguments.Count; i++)
                {
                    args += string.Format("Argument {0} with value - {1} {2} ",
                        input.Arguments.GetParameterInfo(i).Name,
                        input.Arguments[i] ?? "null",
                        Environment.NewLine);
                }
            }
            var title = string.Format("Exception thrown from " + GetMethodString(input.MethodBase));
            _logger.LogException(title, args, ex);
        }

        static string GetMethodString(MethodBase method)
        {
            return string.Format("{0} => {1}", method.ReflectedType, method.ToString());
        }
    }
}
