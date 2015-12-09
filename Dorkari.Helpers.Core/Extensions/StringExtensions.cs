using System;

namespace Dorkari.Helpers.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string pattern, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            return source.IndexOf(pattern, comparison) >= 0;
        }
    }
}
