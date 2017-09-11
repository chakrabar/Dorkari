using System;
using System.Collections.Generic;
using System.Linq;

namespace Dorkari.Helpers.Core.Extensions
{
    public static class EnumExtensions
    {
        public static TAttr GetEnumAttribute<TAttr>(this Enum value) where TAttr : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);

            return type.GetField(name)
                       .GetCustomAttributes(false)
                       .OfType<TAttr>()
                       .SingleOrDefault();
        }

        public static IEnumerable<TEnum> GetAllItems<TEnum>(this Enum value)
        {
            foreach (object item in Enum.GetValues(typeof(TEnum)))
            {
                yield return (TEnum)item;
            }
        }

        public static IEnumerable<TEnum> GetAllItems<TEnum>() where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("Passed type is not an Enum");
            foreach (object item in Enum.GetValues(typeof(TEnum)))
            {
                yield return (TEnum)item;
            }
        }
		
		public static TEnum GetEnumValueFromAttributeString<TEnum, TAttr>(string textValue, Func<TEnum, string> attributeValueGetter) where TAttr : Attribute where TEnum : struct
        {
            foreach (var enumVal in GetAllItems<TEnum>())
            {
                if (attributeValueGetter(enumVal) == textValue)
                    return enumVal;
            }
            return default(TEnum);
        }

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
