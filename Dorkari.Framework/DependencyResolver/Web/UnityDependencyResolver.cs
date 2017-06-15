using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using HTTP = System.Web.Http.Dependencies;
using MVC = System.Web.Mvc;

namespace Dorkari.Framework.DependencyResolver.Web
{
    public class UnityDependencyResolver : UnityObjectResolver, HTTP.IDependencyResolver, MVC.IDependencyResolver
    {
        public UnityDependencyResolver(IUnityContainer container) : base(container)
        {
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return base.Container.Resolve(serviceType);
            }
            catch (ResolutionFailedException rfe)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return Container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException rfe)
            {
                return new List<object>();
            }
        }

        public HTTP.IDependencyScope BeginScope()
        {
            var child = Container.CreateChildContainer();
            return new UnityDependencyResolver(child);
        }

        public void Dispose()
        {
            base.Container.Dispose();
        }
    }
}
