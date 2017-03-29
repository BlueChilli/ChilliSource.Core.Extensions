#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.IO;
using System.Text;

namespace ChilliSource.Core
{
	public static class StringExtensions
	{
		public static string ValueOrEmpty(this string value)
		{
			return !string.IsNullOrEmpty(value) ? value : "";
		}

		public static string ValueOrReplacement(this string value, string replacementValue = "")
		{
			return !string.IsNullOrEmpty(value) ? value : replacementValue;
		}

		public static string RemoveSpaces(this string value)
		{
			return !string.IsNullOrEmpty(value) ? value.Replace(" ", string.Empty) : string.Empty;
		}

		public static byte[] ToByteArray(this string hexString)
		{
			if (string.IsNullOrEmpty(hexString))
			{
				return null;
			}

			int NumberChars = hexString.Length;
			byte[] bytes = new byte[NumberChars / 2];
			for (int i = 0; i < NumberChars; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
			}
			return bytes;
		}


		/// <summary>
		/// Returns a MemoryStream with the bytes representing the <paramref name="inputString"/> encoded as <paramref name="encoding"/>
		/// </summary>
		/// <returns>The stream.</returns>
		/// <param name="inputString">Input string.</param>
		/// <param name="encoding">Encoding. Default is UTF8</param>
		public static Stream ToStream(this string inputString, Encoding encoding = null)
		{
			return new MemoryStream((encoding ?? Encoding.UTF8).GetBytes(inputString ?? ""));
		}
	}
}
