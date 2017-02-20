using System;
using System.Collections.Generic;
using System.Linq;

namespace Dorkari.Helpers.Core.Extensions
{
    public static class CollectionComparer
    {
        //same as collection.Skip(startIndex).Take(count);
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

        //this is O(nlogn), uses CompareTo() comparison
        public static bool HasSameElementOrderAs<T>(this List<T> collection, List<T> second) where T : IComparable<T>
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

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].CompareTo(second[i]) != 0)
                    return false;
            }
            return true;
        }

        //this is O(n), uses hash comparison //TODO: the name sucks
        public static bool HasSameElementSetAs<T>(this List<T> collection, List<T> second) where T : IEqualityComparer<T>
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

        public static bool HasSubSequence<T>(this List<T> main, List<T> second)
        {
            if (main == null || second == null)
                return false;

            var startIndex = main.IndexOf(second.First());
            while (startIndex >= 0)
            {
                if (main.Count - startIndex < second.Count)
                    return false;
                var nonMatch = false;
                for (int i = 0; i < second.Count; i++)
                {
                    if (!main[i + startIndex].Equals(second[i]))
                    {
                        main = main.Skip(startIndex + 1).ToList();
                        startIndex = main.IndexOf(second.First());
                        nonMatch = true;
                        break;
                    }
                }
                if (!nonMatch)
                    return true;
            }
            return false;
        }
    }
}
