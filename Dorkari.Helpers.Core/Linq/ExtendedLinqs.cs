using System;
using System.Collections.Generic;
using System.Linq;

namespace Dorkari.Helpers.Core.Linq
{
    public static class ExtendedLinqs
    {
        private static Random rn = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rn.Next(n + 1);
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

        public static void AddOrCreatet<T>(this ICollection<T> collection, T item)
        {
            if (collection == null)
                collection = new List<T>();
            collection.Add(item);
        }

        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> collection, Func<T, string> selector, bool includeAllEmpty = false) //TODO: Func<T, U>
        {
            if (collection == null)
                throw new ArgumentException("collection");
            if (selector == null)
                throw new ArgumentException("propertySelector");

            var tempGroupKeyIndex = 0;
            return collection.GroupBy(c => includeAllEmpty 
                                           ? (string.IsNullOrEmpty(selector(c)) 
                                                ? (++tempGroupKeyIndex).ToString() 
                                                : selector(c)) 
                                           : selector(c))
                             .Select(grp => grp.First());
        }

        public static TSource MaxBy<TSource, TProperty>(this IEnumerable<TSource> collection, Func<TSource, TProperty> selector)
            where TSource : class //TODO: required?
            where TProperty : IComparable<TProperty>
        {
            if (selector == null)
                throw new ArgumentException("propertySelector");
            if (collection == null || collection.Count() == 0)
                return null;

            TSource maxItem = null;
            foreach (var item in collection)
            {
                if (maxItem == null || selector(item).CompareTo(selector(maxItem)) > 0)
                {
                    maxItem = item;
                }
            }
            return maxItem;
        }

        public static TSource MinBy<TSource, TProperty>(this IEnumerable<TSource> collection, Func<TSource, TProperty> selector)
            where TSource : class //, struct
            where TProperty : IComparable<TProperty>
        {
            if (selector == null)
                throw new ArgumentException("propertySelector");
            if (collection == null || collection.Count() == 0)
                return null;

            TSource minItem = null;
            foreach (var item in collection)
            {
                if (minItem == null || selector(item).CompareTo(selector(minItem)) < 0)
                {
                    minItem = item;
                }
            }
            return minItem;
        }

        //update: moving this extension to List, to follow the convention of Linq. Not to cause side effects in query (well, this becomes a List method now)
        public static IEnumerable<T> ForEachMatch<T>(this List<T> list, Func<T, bool> predicate, Action<T> action)
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

        public static IEnumerable<U> SelectWhere<T, U>(this IEnumerable<T> collection, Func<T, U> selector, Func<T, bool> predicate)
        {
            if (collection == null)
                throw new ArgumentException("collection");
            if (predicate == null)
                throw new ArgumentException("predicate");
            if (selector == null)
                throw new ArgumentException("selector");
            foreach (var item in collection)
            {
                if (predicate(item))
                {
                    yield return selector(item);
                }
            }
        }

        public static U SelectFirstOrDefault<T, U>(this IEnumerable<T> collection, Func<T, U> selector, Func<T, bool> predicate = null)
        {
            if (collection == null)
                throw new ArgumentException("collection");
            if (selector == null)
                throw new ArgumentException("selector");

            var result = default(U);
            foreach (var item in collection)
            {
                if (predicate == null || predicate(item))
                {
                    result = selector(item);
                    break;
                }
            }
            return result;
        }

        public static IEnumerable<T> Merge<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first == null && second == null)
                return null;
            if (first == null)
                return second;
            if (second == null)
                return first;

            var list = new List<T>();
            list.AddRange(first);
            list.AddRange(second);

            return list.AsEnumerable();
        }
    }
}
