#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Linq;

namespace ChilliSource.Core
{
	public static class ByteArrayExtensions
	{
		/// <summary>
		/// Builds a hexadecimal string representing the provided <paramref name="bytes" />
		/// </summary>
		/// <returns>The hex string.</returns>
		/// <param name="bytes">Byte data</param>
		public static string ToHexString(this byte[] bytes)
		{

			return bytes == null ? null : string.Concat(bytes.Select(x => x.ToString("X2")).ToArray());

		}
	}
}
