using Dorkari.Framework.DependencyResolver.Web;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Dorkari.Framework.DependencyResolver
{
    public class UnityResolverBuilder
    {
        public static UnityDependencyResolver Build()
        {
            IUnityContainer container = new UnityContainer();
            
            var resolver = new UnityDependencyResolver(container);
            var assembliesForAutoRegister = TypesScanner.RegisterTypes(resolver, "Dorkari"); //NOTE: Example dll name prefix
            resolver.AutoRegister(assembliesForAutoRegister);

            RegisterPolicies(container);

            return resolver;
        }

        private static void RegisterPolicies(IUnityContainer container) //IF you want interceptor policy injection
        {
            container = container.AddNewExtension<Interception>();
            //register all policies one by one here
            //by assembly or namespace or type etc. (see readme)
        }
    }
}
