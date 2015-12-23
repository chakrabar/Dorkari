using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Dorkari.Helpers.Core.Utilities
{
    public class Poller
    {
        const int _DefaultRtryLimit = 2;
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

        public Poller StopOnException<E>() where E : Exception //TODO: check implementation in Execute()
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

        public PollResult<T> Execute<T>(Func<T> methodToInvoke)
        {
            T retval = default(T);
            var pollResult = Execute(() =>
            {
                retval = methodToInvoke();
            });
            return new PollResult<T>(pollResult, retval);
        }

        public PollResult Execute(Action methodToInvoke)
        {
            var timesToRetry = _retryLimit;
            var result = new PollResult();
            do
            {
                try
                {
                    result.Attempts += 1;
                    methodToInvoke();
                    break;
                }
                catch (Exception ex)
                {
                    result.Exceptions.Add(ex);
                    if ((_allowedExceptionTypes.Count == 0 && _forbiddenExceptionTypes.Count == 0) 
                        || _allowedExceptionTypes.Any(ae => ae.IsAssignableFrom(ex.GetType()))
                        || _forbiddenExceptionTypes.Any() && !_forbiddenExceptionTypes.Any(fe => fe.IsAssignableFrom(ex.GetType())))
                    {
                        if (_waitMilliSeconds > 0 && timesToRetry > 1)
                            Thread.Sleep(_waitMilliSeconds);
                        timesToRetry--;
                    }
                    else
                        throw;
                }
            } while (timesToRetry > 0);

            return result;
        }
    }

    public class PollResult
    {
        public int Attempts { get; set; }
        public List<Exception> Exceptions { get; set; }

        public PollResult()
        {
            Exceptions = new List<Exception>();
        }
    }

    public class PollResult<T> : PollResult
    {
        public T Result { get; set; }

        public PollResult(PollResult parent, T val)
        {
            Attempts = parent.Attempts;
            Exceptions = parent.Exceptions;
            Result = val;
        }
    }
}
