using Dorkari.Framework.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dorkari.Framework.DependencyResolver
{
    public class TypesScanner
    {
        public static IEnumerable<Assembly> RegisterTypes(IObjectResolver resolver, string dllNamePrefix) //common starting name of DLLs
        {
            var binPath = Assembly.GetExecutingAssembly().CodeBase;
            string localPath = new Uri(binPath).LocalPath;
            var directory = new DirectoryInfo(Path.GetDirectoryName(localPath));

            var assembliesForAutoRegister = new List<Assembly>();
            var assembliesToScan = directory.GetFiles("*.dll", SearchOption.TopDirectoryOnly)
                                            .Where(a => a.Name.StartsWith(dllNamePrefix));

            var registryType = typeof(ITypeRegistry);
            foreach (var assembly in assembliesToScan)
            {
                var loadedAssembly = Assembly.LoadFrom(assembly.FullName);
                assembliesForAutoRegister.Add(loadedAssembly);

                var registries = loadedAssembly.GetTypes()
                    .Where(type => registryType.IsAssignableFrom(type) && !type.IsInterface);

                if (registries != null)
                {
                    foreach (var registry in registries)
                    {
                        var registryObject = Activator.CreateInstance(registry) as ITypeRegistry;
                        registryObject.AddRegistries(resolver);
                    }
                }
            }
            return assembliesForAutoRegister;
        }
    }
}
