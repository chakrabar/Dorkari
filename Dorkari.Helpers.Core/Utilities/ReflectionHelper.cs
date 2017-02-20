using Dorkari.Helpers.Core.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dorkari.Helpers.Core.Utilities
{
    public class ReflectionHelper
    {
        public static object GetNonPublicStaticFieldValue(Type type, string fieldName)
        {
            var field = type.GetField(fieldName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            return field.GetValue(null);
        }

        public static object GetNestedPropertyValue(object obj, string nestedDottedPropertyName)
        {
            foreach (String part in nestedDottedPropertyName.Split('.'))
            {
                if (obj == null)
                    return null;

                PropertyInfo info = obj.GetType().GetProperty(part);
                if (info == null)
                    return null;

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static object GeyPropertyValue(object obj, string propName)
        {
            var property = obj.GetType().GetProperty(propName, System.Reflection.BindingFlags.IgnoreCase |
                                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
            var propType = property.PropertyType;
            return Convert.ChangeType(property.GetValue(obj), propType);
        }

        public static string GetCallingMethodName(string message, [CallerMemberName] string callerName = "")
        {
            return callerName + " " + message;
        }

        public static string GetCallingMethodDetails()
        {
            var callingStackFrame = new StackTrace().GetFrame(1);
            var callingMethod = callingStackFrame.GetMethod().Name;
            var callingType = callingStackFrame.GetMethod().DeclaringType.Name;
            return callingMethod + "() from Type '" + callingType + "' called me!";
        }

        public static string GetConcatenatedProprtyValues(object obj, string separator = ",")
        {
            return string.Join(separator, obj.GetType()
                                        .GetProperties()
                                        .Select(prop => prop.GetValue(obj).ToString()));
        }

        public static TDestination MapTo<TDestination>(object source) where TDestination : new()
        {
            if (source == null)
                return default(TDestination);

            var destination = Activator.CreateInstance<TDestination>();

            return MapProperties<TDestination>(source, destination);
        }

        public static TDestination MapTo<TDestination>(object source, TDestination destination) 
        {
            if (source == null || destination == null)
                return default(TDestination);
            
            return MapProperties<TDestination>(source, destination);
        }

        private static T MapProperties<T>(object source, T destination)
        {
            var sourceProperties = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
            var destinationProperties = destination.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

            foreach (var destProp in destinationProperties)
            {
                var matchingSrcProp = sourceProperties
                    .FirstOrDefault(sp => sp.Name.Equals(destProp.Name) && destProp.PropertyType.IsAssignableFrom(sp.PropertyType));
                if (matchingSrcProp != null)
                {
                    destProp.SetValue(destination, matchingSrcProp.GetValue(source));
                }
            }
            return destination;
        }

        public static T CreateInstance<T>(bool populateConstructorParameters = false)
        {
            var objectType = typeof(T);
            var instance = CreateInstance(objectType, populateConstructorParameters);
            return (T)instance;
        }

        //TODO: this needs more testing, specially populateAllParameters = true
        public static object CreateInstance(Type objectType, bool populateAllParameters = false)
        {
            var constructors = objectType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var defaultConstructor = constructors.FirstOrDefault(ctor => ctor.GetParameters().Length == 0); //static, public
            if (defaultConstructor != null) //there is a parameterless constructor for the type
                return Activator.CreateInstance(objectType);

            var ctorWithMinimumParameters = constructors.MinBy(ctor => ctor.GetParameters().Length);

            if (ctorWithMinimumParameters == null)
                return Default(objectType); //there is NO constructor for this type

            var ctorParameters = ctorWithMinimumParameters.GetParameters();
            var totalCtorParameters = ctorParameters.Length;

            var ctorArguments = new object[totalCtorParameters];

            for (int i = 0; i < totalCtorParameters; i++)
            {
                var paramType = ctorParameters[i].ParameterType;
                ctorArguments[i] = (!populateAllParameters) // || IsSimpleType(paramType)
                    ? Default(paramType) : CreateInstance(paramType, populateAllParameters);
            }

            var instance = ctorWithMinimumParameters.Invoke(ctorArguments);
            return instance;
        }

        public static object InvokeAsGeneric(Func<object> func, Type type)
        {
            return func.Method
                .GetGenericMethodDefinition()
                .MakeGenericMethod(type)
                .Invoke(null, null);
        }

        //from http://stackoverflow.com/questions/2442534/how-to-test-if-type-is-primitive >> https://gist.github.com/jonathanconway/3330614
        public static bool IsSimpleType(Type type)
        {
            return
                type.IsValueType ||
                type.IsPrimitive ||
                new Type[] { 
				    typeof(String),
				    typeof(Decimal),
				    typeof(DateTime),
				    typeof(DateTimeOffset),
				    typeof(TimeSpan),
				    typeof(Guid)
			    }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
        }

        public static object Default(Type type)
        {
            if (type.IsPointer)
                return null;
            Func<object> getDefault = Default<object>;
            return InvokeAsGeneric(getDefault, type);
        }

        private static T Default<T>()
        {
            return default(T);
        }


        //TODO: SORT
        public static Tuple<bool, Type> IsGenericIEnumerable(object obj)
        {
            try
            {
                if (obj != null || obj.GetType() != typeof(string))
                {
                    var type = obj.GetType();
                    if (type.GetInterfaces()
                        .Any(i => i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        var argType = type.GetGenericArguments()[0];
                        return new Tuple<bool, Type>(true, argType);
                    }
                }
            }
            catch (Exception)
            {
                //throw;
            }
            return new Tuple<bool, Type>(false, null);
        }

        public static object InvokeMethodFromCodebase(string className, string methodName, object[] input = null)
        {
            var type = ReflectionHelper.GetTypeFromCodebase(className);
            var instance = CreateInstance(type);
            if (instance != null)
            {
                var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (method != null && !method.IsAbstract)
                    return method.Invoke(instance, input);
            }
            throw new ApplicationException(string.Format("No implemented method: {0} found in type: {1}", methodName, className));
        }

        public static Type GetTypeFromCodebase(string typeName, SearchOption searchOption = SearchOption.TopDirectoryOnly, string assemblyNamePrefix = "")
        {
            var binPath = Assembly.GetExecutingAssembly().CodeBase;
            string localPath = new Uri(binPath).LocalPath;
            var directory = new DirectoryInfo(Path.GetDirectoryName(localPath));

            IEnumerable<FileInfo> assembliesToScan = directory.GetFiles("*.dll", searchOption);
            if (!string.IsNullOrWhiteSpace(assemblyNamePrefix))
                assembliesToScan = assembliesToScan.Where(a => a.Name.StartsWith(assemblyNamePrefix));

            foreach (var assembly in assembliesToScan)
            {
                var loadedAssembly = Assembly.LoadFrom(assembly.FullName);
                var types = loadedAssembly.GetTypes().Where(t => t.FullName.Equals(typeName));

                if (types.IsNotEmpty())
                    return types.First();
            }
            throw new ApplicationException(string.Format("No implemented type found for: {0}", typeName));
        }

        private static List<Type> GetAssignableTypesFromCodebase<TBase>(string assemblyNamePrefix = "")
        {
            var implementations = new List<Type>();

            var binPath = Assembly.GetExecutingAssembly().CodeBase;
            string localPath = new Uri(binPath).LocalPath;
            var directory = new DirectoryInfo(Path.GetDirectoryName(localPath));

            IEnumerable<FileInfo> assembliesToScan = directory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);
            if (!string.IsNullOrWhiteSpace(assemblyNamePrefix))
                assembliesToScan = assembliesToScan.Where(a => a.Name.StartsWith(assemblyNamePrefix));

            var baseType = typeof(TBase);
            foreach (var assembly in assembliesToScan)
            {
                var loadedAssembly = Assembly.LoadFrom(assembly.FullName);
                var implementingTypes = loadedAssembly.GetTypes().Where(type => baseType.IsAssignableFrom(type) && !type.IsInterface);

                if (implementingTypes.IsNotEmpty())
                {
                    implementations.AddRange(implementingTypes);
                }
            }
            return implementations;
        }
    }
}
