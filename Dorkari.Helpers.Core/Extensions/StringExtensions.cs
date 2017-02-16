using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Dorkari.Helpers.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string pattern, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            return source.IndexOf(pattern, comparison) >= 0;
        }

        public static bool NullSafeEquals(this string source, string other, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return ((source == null && other == null) || source.Equals(other, comparison));
        }

        public static bool NullAndBlankSpaceSafeEquals(this string source, string other, StringComparison comparison = StringComparison.OrdinalIgnoreCase, params char[] ignoreChars)
        {
            return ((source == null && other == null) || 
                (source != null && other != null && source.StringReplace(" ", "").Equals(other.StringReplace(" ", ""), comparison)));
        }

        public static bool EqualsWithoutIgnoreChars(this string source, string other, params char[] ignoreChars)
        {
            if (source == null && other == null)
                return true;
            if (source == null || other == null)
                return false;

            if (ignoreChars != null && ignoreChars.Length > 0)
            {
                foreach (var ch in ignoreChars.ToList())
                {
                    source = source.Replace(ch.ToString(), "");
                    other = other.Replace(ch.ToString(), "");
                }
            }
            return source.Equals(other, StringComparison.OrdinalIgnoreCase);
        }

        public static string StringReplace(this string source, string pattern, string text)
        {
            if (source == null || pattern == null || text == null)
                return source;
            Regex regex = new Regex(pattern);
            return regex.Replace(source, text);
        }

        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            T result;
            return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
        }

        public static string ToTitleCase(this string source)
        {
            TextInfo ti = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo; // new CultureInfo("en-US", false).TextInfo;
            return source == null ? null : ti.ToTitleCase(source.ToLower());
        }

        public static string TrimExtraSpaces(this string source)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                if (i > 0 && source[i] == ' ' && source[i - 1] == ' ')
                {
                    continue;
                }
                result.Append(source[i]);
            }
            return result.ToString();
        }
    }
}
