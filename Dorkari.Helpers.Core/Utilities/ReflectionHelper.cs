using System;
using System.Diagnostics;
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

        public static string GetCallingMethodName(string Message, [CallerMemberName] string callerName = "")
        {
            return callerName + " " + Message;
        }

        public static string GetCallingMethodDetails()
        {
            var callingStackFrame = new StackTrace().GetFrame(1);
            var callingmethod = callingStackFrame.GetMethod().Name;
            var callingType = callingStackFrame.GetMethod().DeclaringType.Name;
            return callingmethod + "() from Type '" + callingType + "' called me!";
        }
    }
}
