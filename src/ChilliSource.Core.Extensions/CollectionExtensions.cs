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
	public static class CollectionExtensions
	{
		public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list) => new ReadOnlyCollection<T>(list);

		public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> list) => new ReadOnlyCollection<T>(new List<T>(list));

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
                throw new ArgumentException("item is null");

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
    }
}
