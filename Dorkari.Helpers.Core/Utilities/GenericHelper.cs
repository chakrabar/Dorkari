using System;

namespace Dorkari.Helpers.Core.Utilities
{
    public static class GenericHelper
    {
        public static T SelectByPredicate<T>(this T value, Func<T, bool> predicate, T alternativeValue)
        {
            return predicate(value) ? value : alternativeValue;
        }

        public static T UseOtherIfNull2<T>(T value, T other) where T : class
        {
            return value.SelectByPredicate(x => x != null, other);
        }

        public static T IfNullThen<T>(this T value, T other) where T : class
        {
            return SelectByPredicate(value, x => x != null, other);
        }
    }
}
