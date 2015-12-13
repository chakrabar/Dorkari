using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Dorkari.Helpers.Core.Utilities
{
    public class Poller
    {
        const int _DefaultRtryLimit = 5;
        List<Type> _allowedExceptionTypes; //all are allowed if this is not set | mutually exclusive to _forbiddenExceptionTypes
        List<Type> _forbiddenExceptionTypes; //none are forbidden if this is not set | mutually exclusive to _allowedExceptionTypes
        int _retryLimit;
        int _waitMilliSeconds;

        public Poller()
        {
            _allowedExceptionTypes = new List<Type>();
            _forbiddenExceptionTypes = new List<Type>();
            _retryLimit = _DefaultRtryLimit;
        }

        public Poller WithException<E>() where E : Exception
        {
            if (_forbiddenExceptionTypes.Count > 0)
                throw new ArgumentException("Cannot add allowed exception if forbidden exceptions are present");
            this._allowedExceptionTypes.Add(typeof(E));
            return this;
        }

        public Poller StopOnException<E>() where E : Exception //TODO: implement in Execute()
        {
            if (_allowedExceptionTypes.Count > 0)
                throw new ArgumentException("Cannot add forbidden exception if allowed exceptions are present");
            this._forbiddenExceptionTypes.Add(typeof(E));
            return this;
        }

        public Poller WithRetries(int times)
        {
            this._retryLimit = times > 0 ? times : _DefaultRtryLimit;
            return this;
        }

        public Poller WithWait(int milliSeconds)
        {
            this._waitMilliSeconds = milliSeconds > 0 ? milliSeconds : 0;
            return this;
        }

        public T Execute<T>(Func<T> methodToInvoke)
        {
            var timesToRetry = _retryLimit;
            do
            {
                try
                {
                    return methodToInvoke();
                }
                catch (Exception ex)
                {
                    if (_allowedExceptionTypes.Count == 0 || _allowedExceptionTypes.Any(ae => ae.IsAssignableFrom(ex.GetType())))
                    {
                        if (_waitMilliSeconds > 0 && timesToRetry > 1)
                            Thread.Sleep(_waitMilliSeconds);
                        timesToRetry--;
                    }
                    else
                        throw;
                }
            } while (timesToRetry > 0);
            
            return default(T);
        }

        public void Execute(Action methodToInvoke) //TODO: reduce duplication
        {
            var timesToRetry = _retryLimit;
            do
            {
                try
                {
                    methodToInvoke();
                    break;
                }
                catch (Exception ex)
                {
                    if (_allowedExceptionTypes.Count == 0 || _allowedExceptionTypes.Any(ae => ae.IsAssignableFrom(ex.GetType())))
                    {
                        if (_waitMilliSeconds > 0 && timesToRetry > 1)
                            Thread.Sleep(_waitMilliSeconds);
                        timesToRetry--;
                    }
                    else
                        throw;
                }
            } while (timesToRetry > 0);

            return;
        }
    }
}
