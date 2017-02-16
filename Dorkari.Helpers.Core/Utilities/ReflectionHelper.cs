using Dorkari.Helpers.Core.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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

        public static T CreateInstance<T>()
        {
            var objectType = typeof(T);
            var instance = CreateInstance(objectType);
            return (T)instance;
        }

        public static object CreateInstance(Type objectType) //bool populateProperties = false
        {
            var constructors = objectType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var defaultConstructor = constructors.SingleOrDefault(ctor => ctor.GetParameters().Length == 0);
            if (defaultConstructor != null)
                return Activator.CreateInstance(objectType);

            var ctorWithMinimumParameters = constructors.MinBy(ctor => ctor.GetParameters().Length);

            if (ctorWithMinimumParameters == null)
                return Default(objectType); //no constructor

            var ctorParameters = ctorWithMinimumParameters.GetParameters();
            var totalCtorParameters = ctorParameters.Length;

            var ctorArguments = new object[totalCtorParameters];

            for (int i = 0; i < totalCtorParameters; i++)
            {
                var paramType = ctorParameters[i].ParameterType;
                //if (populateConstructorParameters) //complexity of Interface type parameters
                //{
                //    var thisMethod = typeof(ReflectionHelper)
                //                    .GetMethod("CreateInstance", BindingFlags.Static)
                //                    .MakeGenericMethod(paramType);
                //    ctorArguments[i] = thisMethod.Invoke(null, new object[] { true });
                //}
                //else
                ctorArguments[i] = Default(paramType);
            }

            //var instance = Activator.CreateInstance(objectType, ctorArguments); //this blows up when there is confusion with parameter types
            var instance = ctorWithMinimumParameters.Invoke(ctorArguments);
            return instance;
        }

        public static object Default(Type type)
        {
            Func<object> getDefault = Default<object>;
            //return getDefault.Method
            //    .GetGenericMethodDefinition()
            //    .MakeGenericMethod(type)
            //    .Invoke(null, null);
            return InvokeAsGeneric(getDefault, type);
        }

        private static object InvokeAsGeneric(Func<object> func, Type type)
        {
            return func.Method
                .GetGenericMethodDefinition()
                .MakeGenericMethod(type)
                .Invoke(null, null);
        }

        private static T Default<T>()
        {
            return default(T);
        }
    }
}
