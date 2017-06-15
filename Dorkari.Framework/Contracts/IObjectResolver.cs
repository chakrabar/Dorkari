using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dorkari.Framework.Contracts
{
    public interface IObjectResolver //NOTE: This allows (optionally) interceptor policy injection
    {
        T Resolve<T>(string name);
        T Resolve<T>();
        object GetInstance(Type type);
        void Register<TFrom, TTo>(bool allowPolicyInjection = false) where TTo : TFrom;
        void Register<TFrom, TTo>(string mappingName, bool allowPolicyInjection = false) where TTo : TFrom;
        void RegisterInstance<TFrom>(TFrom actualInstance);
        void AutoRegister(IEnumerable<Assembly> assembliesToScan);
    }
}
