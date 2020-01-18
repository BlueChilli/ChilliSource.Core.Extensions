#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ChilliSource.Core.Extensions
{
    /// <summary>
    /// Collection extensions.
    /// </summary>
    public static class CollectionExtensions
    {

        #region ReadOnly
        /// <summary>
        /// Returns a read-only version of the specified IList.
        /// </summary>
        /// <returns>The read only collection.</returns>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list) => new ReadOnlyCollection<T>(list);

        /// <summary>
        /// Returns a read-only version of the specified IEnumerable.
        /// </summary>
        /// <returns>The read only collection.</returns>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> list) => new ReadOnlyCollection<T>(new List<T>(list));
        #endregion

        /// <summary>
        /// Adds the <paramref name="item"/> to the list. If the item already exists it will be updated in the list using the <paramref name="keySelector"/>
        /// function to determine the correct key for the item
        /// </summary>
        /// <typeparam name="T">The type of the elements of list.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The System.Collections.Generic.List&lt;T&gt;.</param>
        /// <param name="item">The item to add or update.</param>
        /// <param name="keySelector">A function to select the key.</param>
        public static void AddOrUpdate<T, TKey>(this IList<T> source, T item, Func<T, TKey> keySelector)
        {
            if (item == null)
            {
                throw new ArgumentException("item is null");
            }

            var itemKey = keySelector(item);
            for (int i = 0; i < source.Count; i++)
            {
                var otherKey = keySelector(source[i]);
                if ((otherKey == null && itemKey == null) || (otherKey?.Equals(itemKey) == true))
                {
                    source[i] = item;
                    return;
                }
            }

            source.Add(item);
        }

        /// <summary>
        /// Reduces collection to distinct members based on a key, which is selected using the <paramref name="keySelector"/> function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the generic enumerable list.</typeparam>
        /// <typeparam name="TKey">The type of the key property.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable&lt;T&gt;.</param>
        /// <param name="keySelector">A function to determine uniqueness for the distinct operation.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Converts a list of T to a delimited string, by having each T.ToString() executed for each T
        /// </summary>
        /// <typeparam name="T">The type of the elements of the generic enumerable list.</typeparam>
        /// <param name="collection">The generic enumerable list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>A delimited string</returns>
        public static string ToDelimitedString<T>(this IEnumerable<T> collection, string delimiter = ",", string formatSpecifier = "")
        {
            if (collection == null)
            {
                return String.Empty;
            }

            Func<T, string> formatter;
            if (String.IsNullOrEmpty(formatSpecifier))
            {
                formatter = (T e) => e.ToString();
            }
            else
            {
                formatter = (T e) => String.Format($"{{0:{formatSpecifier}}}", e);
            }

            return String.Join(delimiter, collection.Select(formatter));
        }

        #region FirstOr New/DefaultTo
        /// <summary>
        /// Returns the first element of a sequence, or a new instance if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable&lt;T&gt; to return the first element of.</param>
        /// <returns>A new instance of TSource when source is empty; otherwise, the first element in source.</returns>
        public static TSource FirstOrNew<TSource>(this IEnumerable<TSource> source)
        {
            return source.FirstOrDefaultTo(() => Activator.CreateInstance<TSource>());
        }

        /// <summary>
        /// Returns the first element of the sequence that satisfies a condition, or a new instance if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable&lt;T&gt; to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A new instance of TSource when source is empty or if no element passes the test specified by predicate; 
        /// otherwise the first element in source that passes the test specified by <paramref name="predicate"/>.</returns>
        public static TSource FirstOrNew<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var enumerable = source?.Where(predicate);
            return enumerable.FirstOrNew();
        }

        /// <summary>
        /// Returns the first element of a sequence, or the specified <paramref name="defaultTo"/> element if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable&lt;T&gt; to return the first element of.</param>
        /// <param name="defaultTo">The specified element.</param>
        /// <returns>The specified  <paramref name="defaultTo"/> element when source is empty; otherwise, the first element in <paramref name="source"/>.</returns>
        public static TSource FirstOrDefaultTo<TSource>(this IEnumerable<TSource> source, TSource defaultTo)
        {
            return source.FirstOrDefaultTo(() => defaultTo);
        }

        /// <summary>
        /// Returns the first element of a sequence, or the specified <paramref name="defaultTo"/> element if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable&lt;T&gt; to return the first element of.</param>
        /// <param name="defaultValueFactory">A default value factory delegate.</param>
        /// <returns>A value created by <paramref name="defaultValueFactory"/> delegate when source is empty; otherwise, the first element in <paramref name="source"/>.</returns>
        public static TSource FirstOrDefaultTo<TSource>(this IEnumerable<TSource> source, Func<TSource> defaultValueFactory)
        {
            if (source != null)
            {
                foreach (var element in source)
                {
                    //returns first
                    return element;
                }
            }

            return defaultValueFactory();
        }

        /// <summary>
        /// Returns the first element of the sequence that satisfies a condition, or the specified <paramref name="defaultTo"/> element if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable&lt;T&gt; to return the first element of.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="defaultTo">The default element to be returned when the source IEnumerable is empty or no element passes the test specified by <paramref name="predicate"/></param>
        /// <returns></returns>
        public static TSource FirstOrDefaultTo<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultTo)
        {
            var enumerable = source?.Where(predicate);
            return enumerable.FirstOrDefaultTo(defaultTo);
        }
        #endregion
    }
}
