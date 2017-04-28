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
        /// Adds the item to specified collection list or updates if exists by the key selector function.
        /// </summary>
        /// <typeparam name="T">The type of the elements of list.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The System.Collections.Generic.List&lt;T&gt;.</param>
        /// <param name="item">The item to add or update.</param>
        /// <param name="keySelector">A function to select the key.</param>
        public static void AddOrUpdate<T, TKey>(this List<T> source, T item, Func<T, TKey> keySelector)
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
        /// Reduces collection to distinct members by a key property.
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

        #region FirstOr New/DefaultTo
        /// <summary>
        /// Returns the first element of a sequence, or a new instance if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable&lt;T&gt; to return the first element of.</param>
        /// <returns>A new instance of TSource when source is empty; otherwise, the first element in source.</returns>
        public static TSource FirstOrNew<TSource>(this IEnumerable<TSource> source)
        {
            var result = source.FirstOrDefault();
            return (result == null) ? Activator.CreateInstance<TSource>() : result;
        }

        /// <summary>
        /// Returns the first element of the sequence that satisfies a condition or a new instance if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable&lt;T&gt; to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A new instance of TSource when source is empty or if no element passes the test specified by predicate; otherwise, the first element in source that passes the test specified by predicate.</returns>
        public static TSource FirstOrNew<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var result = source.FirstOrDefault(predicate);
            return (result == null) ? Activator.CreateInstance<TSource>() : result;
        }

        /// <summary>
        /// Returns the first element of a sequence, or the specified element if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable&lt;T&gt; to return the first element of.</param>
        /// <param name="defaultTo">The specified element.</param>
        /// <returns>The specified element when source is empty; otherwise, the first element in source.</returns>
        public static TSource FirstOrDefaultTo<TSource>(this IEnumerable<TSource> source, TSource defaultTo)
        {
            return (source.Count() == 0) ? defaultTo : source.First();
        }

        /// <summary>
        /// Returns the first element of the sequence that satisfies a condition or the specified element if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable&lt;T&gt; to return the first element of.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="defaultTo">The specified element when source is empty or if no element passes the test specified by predicate; otherwise, the first element in source that passes the test specified by predicate.</param>
        /// <returns></returns>
        public static TSource FirstOrDefaultTo<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultTo)
        {
            return (source.Any(predicate)) ? source.First(predicate) : defaultTo;
        }
        #endregion
    }
}
