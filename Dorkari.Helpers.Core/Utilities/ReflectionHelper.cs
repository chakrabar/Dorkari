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

        public static T MapTo<T>(object source) where T : new()
        {
            if (source == null)
                return default(T);

            var destination = Activator.CreateInstance<T>();

            var sourceProperties = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
            var destinationProperties = destination.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

            foreach (var destProp in destinationProperties)
            {
                var matchingSrcProp = sourceProperties.FirstOrDefault(sp => sp.Name.Equals(destProp.Name) 
                                                                        && sp.PropertyType.Equals(destProp.PropertyType));
                if (matchingSrcProp != null)
                {
                    destProp.SetValue(destination, matchingSrcProp.GetValue(source));
                }
            }
            return destination;
        }
    }
}
