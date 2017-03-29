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

namespace ChilliSource.Core
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendFormattedLine(this StringBuilder @this, string format, params object[] args)
        {
            return @this.AppendFormat(format, args).AppendLine();
        }

		public static StringBuilder AppendWhen(this StringBuilder @this, Func<bool> predicate, Func<StringBuilder, StringBuilder> function)
        {
            return predicate() ? function(@this) : @this;
        }

		public static StringBuilder AppendSequence<T>(this StringBuilder @this, IEnumerable<T> sequence, Func<StringBuilder, T, StringBuilder> function)
        {
			return sequence.Aggregate(@this, function);
        }
    }
}
