#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChilliSource.Core.Extensions
{
	/// <summary>
	/// String builder extensions.
	/// </summary>
	public static class StringBuilderExtensions
	{
        /// <summary>
        /// Formats and appends a line to the current StringBuilder.
        /// </summary>
        /// <returns>The StringBuilder with the updated contents.</returns>
        /// <param name="this">The original StringBuilder object.</param>
        /// <param name="format">Format.</param>
        /// <param name="args">Arguments.</param>
        public static StringBuilder AppendFormattedLine(this StringBuilder @this, string format, params object[] args)
		{
			return @this.AppendFormat(format, args).AppendLine();
		}

        /// <summary>
        /// Appends the output of the specified <paramref name="function"/> to the current string builder, depending on the outcome of the specified <paramref name="predicate"/>
        /// </summary>
        /// <returns>The StringBuilder with the updated contents.</returns>
        /// <param name="this">The original StringBuilder object.</param>
        /// <param name="predicate">Predicate determining if the append should occur.</param>
        /// <param name="function">Function providing the append logic.</param>
        public static StringBuilder AppendWhen(this StringBuilder @this, Func<bool> predicate, Func<StringBuilder, StringBuilder> function)
		{
			return predicate() ? function(@this) : @this;
		}

        /// <summary>
        /// Appends a <paramref name="sequence"/> of items using the specified <paramref name="function"/>
        /// </summary>
        /// <returns>The StringBuilder with the updated contents.</returns>
        /// <param name="this">The original StringBuilder object.</param>
        /// <param name="sequence">The sequence of items to append.</param>
        /// <param name="function">Function providing the append logic.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static StringBuilder AppendSequence<T>(this StringBuilder @this, IEnumerable<T> sequence, Func<StringBuilder, T, StringBuilder> function)
		{
			return sequence.Aggregate(@this, function);
		}
	}
}
