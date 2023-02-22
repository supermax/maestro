using System;
using System.Collections.Generic;

namespace SuperMaxim.Core.Extensions
{
    /// <summary>
    /// Collection Extensions
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Convert array to array of objects
        /// </summary>
        /// <param name="source">The source array</param>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <returns>The array of objects</returns>
        public static object[] ToObjectArray<T>(this T[] source)
        {
            var copy = new object[source.Length];
            Array.Copy(source, copy, source.Length);
            return copy;
        }

        /// <summary>
        /// Iterate thru the collection and execute the given action
        /// </summary>
        /// <param name="collection">The source collection</param>
        /// <param name="action">The action to execute during the iteration</param>
        /// <typeparam name="T">The type of the item</typeparam>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        /// <summary>
        /// Iterate thru the array and execute the given action
        /// </summary>
        /// <remarks>
        /// Using more effective for(i) loop instead of the expensive enumerator
        /// </remarks>
        /// <param name="collection">The source array</param>
        /// <param name="action">The action to execute during the iteration</param>
        /// <typeparam name="T">The type of the item</typeparam>
        public static void ForEach<T>(this T[] collection, Action<T> action)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < collection.Length; i++)
            {
                action(collection[i]);
            }
        }

        /// <summary>
        /// Iterate thru the list and execute the given action
        /// </summary>
        /// <remarks>
        /// Using more effective for(i) loop instead of the expensive enumerator
        /// </remarks>
        /// <param name="collection">The source list</param>
        /// <param name="action">The action to execute during the iteration</param>
        /// <typeparam name="T">The type of the item</typeparam>
        public static void ForEach<T>(this IList<T> collection, Action<T> action)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < collection.Count; i++)
            {
                action(collection[i]);
            }
        }

        /// <summary>
        /// Checks if string is null or empty
        /// </summary>
        /// <param name="source">The source string</param>
        /// <returns>"True" if string is null or empty</returns>
        public static bool IsNullOrEmpty(this string source)
        {
            var isEmpty = string.IsNullOrEmpty(source);
            return isEmpty;
        }

        /// <summary>
        /// Check if collection is null or empty
        /// </summary>
        /// <param name="source">The source collection</param>
        /// <typeparam name="T">The item type</typeparam>
        /// <returns>"true" in case array us null or empty</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            if(source == null)
            {
                return true;
            }
            return source.Count < 1;
        }

        /// <summary>
        /// Returns 1st element in the array
        /// </summary>
        /// <param name="collection">The source array</param>
        /// <typeparam name="T">The type of the array elements</typeparam>
        /// <returns>1st element</returns>
        public static T First<T>(this T[] collection)
        {
            if (collection.IsNullOrEmpty())
            {
                return default;
            }
            var first = collection[0];
            return first;
        }

        /// <summary>
        /// Returns 1st element in the array
        /// </summary>
        /// <param name="collection">The source array</param>
        /// <typeparam name="T">The type of the array elements</typeparam>
        /// <returns>1st element</returns>
        public static T First<T>(this IList<T> collection)
        {
            if (collection.IsNullOrEmpty())
            {
                return default;
            }
            var first = collection[0];
            return first;
        }

        /// <summary>
        /// Returns last element in the array
        /// </summary>
        /// <param name="collection">The source array</param>
        /// <typeparam name="T">The type of the array elements</typeparam>
        /// <returns>Last element</returns>
        public static T Last<T>(this T[] collection)
        {
            if (collection.IsNullOrEmpty())
            {
                return default;
            }
            var last = collection[^1];
            return last;
        }

        /// <summary>
        /// Returns last element in the array
        /// </summary>
        /// <param name="collection">The source array</param>
        /// <typeparam name="T">The type of the array elements</typeparam>
        /// <returns>Last element</returns>
        public static T Last<T>(this IList<T> collection)
        {
            if (collection.IsNullOrEmpty())
            {
                return default;
            }
            var last = collection[^1];
            return last;
        }
    }
}
