using Dorkari.Framework.Contracts;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Reflection;
using Utilities = Dorkari.Helpers.Core.Utilities;

namespace Dorkari.Framework.DependencyResolver
{
    public class UnityObjectResolver : IObjectResolver
    {
        protected readonly IUnityContainer Container;

        public UnityObjectResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.Container = container;
        }

        public void Register<TFrom, TTo>(bool allowPolicyInjection = false) where TTo : TFrom
        {
            if (allowPolicyInjection)
                this.Container.RegisterType<TFrom, TTo>(
                    new InterceptionBehavior<PolicyInjectionBehavior>(),
                    new Interceptor<InterfaceInterceptor>());
            else
                this.Container.RegisterType<TFrom, TTo>();
        }

        public void Register<TFrom, TTo>(string mappingName, bool allowPolicyInjection = false) where TTo : TFrom
        {
            if (allowPolicyInjection)
                this.Container.RegisterType<TFrom, TTo>(
                    mappingName,
                    new InterceptionBehavior<PolicyInjectionBehavior>(),
                    new Interceptor<InterfaceInterceptor>());
            else
                this.Container.RegisterType<TFrom, TTo>(mappingName);
        }

        public void RegisterInstance<TFrom>(TFrom actualInstance)
        {
            this.Container.RegisterInstance<TFrom>(actualInstance);
        }

        public void AutoRegister(IEnumerable<Assembly> assembliesToScan)
        {
            if (assembliesToScan == null)
                Container.RegisterTypes(AllClasses.FromLoadedAssemblies(), WithMappings.FromMatchingInterface, WithName.Default);
            else
                Container.RegisterTypes(AllClasses.FromAssemblies(assembliesToScan), WithMappings.FromMatchingInterface, WithName.Default);
        }

        public T Resolve<T>(string name)
        {
            return Container.Resolve<T>(name);
        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public object GetInstance(Type type)
        {
            Func<object> getInstance = this.Resolve<object>;
            return Utilities.ReflectionHelper.InvokeAsGeneric(getInstance, type);
        }
    }
}
