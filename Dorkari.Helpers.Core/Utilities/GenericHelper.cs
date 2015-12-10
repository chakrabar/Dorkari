using System;

namespace Dorkari.Helpers.Core.Utilities
{
    public class GenericHelper
    {
        public static T SelectByPredicate<T>(T value, Func<T, bool> predicate, T alternativeValue)
        {
            return predicate(value) ? value : alternativeValue;
        }

        public static T UseOtherIfNull<T>(T value, T other) where T : class
        {
            return SelectByPredicate(value, x => x != null, other);
        }
    }
}
