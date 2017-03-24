using System;
using System.Text;

namespace Dorkari.Helpers.Core.Utilities
{
    public class ExceptionHelper
    {
        private const string _IsLoggedKey = "IsLoggedThroughDorkari";

        public static string GetExceptionAsString(Exception exception)
        {
            return Environment.NewLine + 
                ">> Message : " + GetExceptionMessage(exception) + Environment.NewLine + 
                ">> Stack Trace : " + GetAggregateExceptionStack(exception);
        }

        public static string GetExceptionMessage(Exception exception, int innerExceptionDepth = 5)
        {
            var currentDepth = 0;
            StringBuilder message = new StringBuilder();
            var innerException = exception;
            while (innerException != null && ++currentDepth <= innerExceptionDepth)
            {
                var innerMessage = currentDepth == 1 ? string.Empty : " ." + Environment.NewLine + "Inner Exception: ";
                if (innerException is AggregateException)
                {
                    message.Append(innerMessage + GetAggregateExceptionMessage(innerException));
                    innerException = null; //pulling all exceptions from InnerExceptionS.
                }
                else
                {
                    message.Append(innerMessage + innerException.Message);
                    innerException = innerException.InnerException;
                }
            }
            return message.ToString();
        }

        public static string GetAggregateExceptionMessage(Exception exception)
        {
            StringBuilder message = new StringBuilder();
            if (exception != null && exception is AggregateException)
            {
                message.Append(exception.Message + Environment.NewLine + "Details: ");
                var ae = exception as AggregateException;
                foreach (var ie in ae.Flatten().InnerExceptions)
                {
                    message.Append(GetExceptionMessage(ie));
                }
            }
            return message.ToString();
        }

        public static string GetAggregateExceptionStack(Exception exception)
        {
            var innerException = exception;
            while (innerException is AggregateException)
            {
                var ex = innerException as AggregateException;
                innerException = ex.InnerException;
            }
            return innerException.StackTrace;
        }

        public static void MarkAsLogged(Exception exception)
        {
            if (exception != null)
            {
                exception.Data[_IsLoggedKey] = true;
            }
        }

        public static bool IsAlreadyLogged(Exception exception)
        {
            try
            {
                if (exception != null)
                {
                    var isAlreadyLogged = exception.Data[_IsLoggedKey];
                    if (isAlreadyLogged != null && (bool)isAlreadyLogged)
                        return true;
                }
            }
            catch (Exception) //for data mismatch and other issues, consider not-logged
            {
            }            
            return false;
        }
    }
}
