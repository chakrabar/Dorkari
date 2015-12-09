using System;
using System.Collections.Generic;
using System.Linq;

namespace Dorkari.Helpers.Core.Extensions
{
    public static class CollectionExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || collection.Count() == 0;
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Count() > 0;
        }

        public static int NullSafeCount<T>(this IEnumerable<T> collection)
        {
            return collection == null ? 0 : collection.Count();
        }

        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> collection, Func<T, string> propertySelector, bool includeAllEmpty = false) //TODO: Func<T, U>
        {
            if (collection == null)
                throw new ArgumentException("collection");
            if (propertySelector == null)
                throw new ArgumentException("propertySelector");

            var tempGroupKeyIndex = 0;
            return collection.GroupBy(c => includeAllEmpty 
                                           ? (string.IsNullOrEmpty(propertySelector(c)) 
                                                ? (++tempGroupKeyIndex).ToString() 
                                                : propertySelector(c)) 
                                           : propertySelector(c))
                             .Select(grp => grp.First());
        }

        public static TSource MaxBy<TSource, TProperty>(this IEnumerable<TSource> collection, Func<TSource, TProperty> propertySelector)
            where TSource : class
            where TProperty : IComparable<TProperty>
        {
            if (propertySelector == null)
                throw new ArgumentException("propertySelector");
            if (collection == null || collection.Count() == 0)
                return null;

            TSource maxItem = null;
            foreach (var item in collection)
            {
                if (maxItem == null || propertySelector(item).CompareTo(propertySelector(maxItem)) > 0)
                {
                    maxItem = item;
                }
            }
            return maxItem;
        }

        public static TSource MinBy<TSource, TProperty>(this IEnumerable<TSource> collection, Func<TSource, TProperty> propertySelector)
            where TSource : class //, struct
            where TProperty : IComparable<TProperty>
        {
            if (propertySelector == null)
                throw new ArgumentException("propertySelector");
            if (collection == null || collection.Count() == 0)
                return null;

            TSource minItem = null;
            foreach (var item in collection)
            {
                if (minItem == null || propertySelector(item).CompareTo(propertySelector(minItem)) < 0)
                {
                    minItem = item;
                }
            }
            return minItem;
        }

        public static IEnumerable<T> ForEachMatch<T>(this IEnumerable<T> list, Func<T, bool> predicate, Action<T> action)
        {
            if (list == null)
                throw new ArgumentException("list");
            if (predicate == null)
                throw new ArgumentException("predicate");
            if (action == null)
                throw new ArgumentException("action");
            foreach (var item in list)
            {
                if (predicate(item))
                {
                    action(item);
                    yield return item;
                }
            }
        }

        public static bool IsAllUnique<T>(this IEnumerable<T> values) where T : IComparable<T> //http://stackoverflow.com/questions/32935560/assert-uniqueness-of-fields-in-list-c-sharp#32935560
        {
            HashSet<T> hashSet = new HashSet<T>();
            return values.All(x => hashSet.Add(x));
        }

        public static void RemoveAllWithPropertyValue<T>(this List<T> list, string propertyName, object value) //TODO: U value
        {
            if (list == null)
                throw new ArgumentException("list");
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("propertyName");
            if (value == null)
                throw new ArgumentException("value");
            var property = typeof(T).GetProperty(propertyName);
            list.RemoveAll(item => property.GetValue(item, null).Equals(value));
        }
    }
}
