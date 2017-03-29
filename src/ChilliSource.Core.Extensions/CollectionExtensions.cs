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

namespace ChilliSource.Core
{
	public static class CollectionExtensions
	{
		public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list) => new ReadOnlyCollection<T>(list);

		public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> list) => new ReadOnlyCollection<T>(new List<T>(list));

	}
}
