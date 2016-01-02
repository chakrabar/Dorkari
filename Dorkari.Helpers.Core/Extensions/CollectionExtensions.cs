using System;
using System.Collections.Generic;
using System.Linq;

namespace Dorkari.Helpers.Core.Extensions
{
    public static class CollectionExtensions
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
            where TSource : class
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

        public static IEnumerable<T> SubList<T>(this IEnumerable<T> collection, int startIndex, int count)
        {
            if (collection == null)
                throw new ArgumentException("collection");
            if (startIndex < 0 || startIndex >= collection.Count())
                throw new ArgumentException("Invalid value", "startIndex");
            if (count < 0)
                throw new ArgumentException("Invalid value", "count");

            var index = 0;
            foreach (var item in collection)
            {
                var current = index;
                index++;
                if (current >= startIndex && current < startIndex + count)
                    yield return item;
                else if (current >= startIndex + count)
                    yield break;
            }
        }

        public static bool HasSameUniqueElementsAs<T>(this IEnumerable<T> collection, IEnumerable<T> second) where T : IEquatable<T>
        {
            if (collection == null && second == null)
                return true;
            if (collection == null || second == null)
                return false;
            if (ReferenceEquals(collection, second))
                return true;

            var uniqueFirst = collection.Distinct();
            var uniqueSecond = second.Distinct();

            foreach (var item in collection)
            {
                if (second.Any(s => s.Equals(item)))
                    continue;
                return false;
            }
            return true;
        }

        //this is O(nlogn), uses reference comparison
        public static bool HasSameElementReferencesAs<T>(this List<T> collection, List<T> second) where T : IComparable<T>
        {
            if (collection == null && second == null)
                return true;
            if (collection == null || second == null)
                return false;            
            if (ReferenceEquals(collection, second))
                return true;
            if (collection.Count() != second.Count())
                return false;

            collection.Sort();
            second.Sort();

            return collection.SequenceEqual(second);
        }

        //this is O(n), uses hash comparison
        public static bool HasSameElementsAs<T>(this List<T> collection, List<T> second) where T : IEqualityComparer<T>
        {
            if (collection == null && second == null)
                return true;
            if (collection == null || second == null)
                return false;
            if (ReferenceEquals(collection, second))
                return true;
            if (collection.Count() != second.Count())
                return false;

            var countingDictForCollection = GetCountingDictionary<T>(collection);
            var countingDictForSecond = GetCountingDictionary<T>(second);

            foreach (var item in countingDictForCollection)
            {
                int count = 0;
                if (countingDictForSecond.TryGetValue(item.Key, out count))
                {
                    if (count != item.Value)
                        return false;
                }
                else
                    return false;
            }
            return true;
        }

        private static Dictionary<T, int> GetCountingDictionary<T>(List<T> collection) where T : IEqualityComparer<T>
        {
            var checkDict = new Dictionary<T, int>();
            foreach (var item in collection)
            {
                if (checkDict.ContainsKey(item))
                {
                    checkDict[item] = checkDict[item] + 1;
                }
                else
                {
                    checkDict.Add(item, 1);
                }
            }
            return checkDict;
        }
    }
}
