using System;
using System.Linq;

namespace Dorkari.Helpers.Core.Utilities
{
    public class StringHelper
    {
        public static string JoinRemovingBlanks(string separator, params string[] args)
        {
            var nonEmptyValues = args.Where(a => !string.IsNullOrWhiteSpace(a));
            return nonEmptyValues.Count() == 0 ? string.Empty : string.Join(separator, nonEmptyValues);
        }

        public static string UseBlankIfEmpty(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }

        public static string UseOtherIfEmpty(string value, string other)
        {
            return string.IsNullOrWhiteSpace(value) ? other : value;
        }

        public static string RemoveQuotes(string value)
        {
            return value.Replace("\"", "");
        }

        public static string UnescapeNonPrintables(string value)
        {
            return System.Text.RegularExpressions.Regex.Unescape(value);
        }

        public static bool AreNonNullEqual(string str, string match, StringComparison compareType = StringComparison.InvariantCultureIgnoreCase)
        {
            return !string.IsNullOrWhiteSpace(str) && !string.IsNullOrWhiteSpace(match) && str.Trim().Equals(match.Trim(), compareType);
        }

        public static bool AreEqualWithoutSpace(string str, string match, StringComparison compareType = StringComparison.InvariantCultureIgnoreCase)
        {
            return !string.IsNullOrWhiteSpace(str) && !string.IsNullOrWhiteSpace(match) && str.Replace(" ", "").Trim().Equals(match.Replace(" ", "").Trim(), compareType);
        }

        public static bool AreNotEqual(string str, string match, StringComparison compareType = StringComparison.InvariantCultureIgnoreCase)
        {
            return string.IsNullOrWhiteSpace(str) || !str.Trim().Equals(match.Trim(), compareType);
        }
    }
}
