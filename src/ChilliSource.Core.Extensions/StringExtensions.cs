#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace ChilliSource.Core.Extensions
{
	/// <summary>
	/// String extensions.
	/// </summary>
	public static class StringExtensions
	{
		#region Sanitise / Remove / Replace
		/// <summary>
		/// Converts a string value to valid file path name.
		/// </summary>
		/// <param name="s">The specified string value.</param>
		/// <returns>A valid file path name.</returns>
		public static string ToFilename(this string s)
		{
			var invalidChars = Path.GetInvalidFileNameChars();
			foreach (var c in invalidChars) s = s.Replace(c.ToString(), "");
			//remove additional periods
			var filename = Path.GetFileNameWithoutExtension(s).Replace(".", "");
			s = filename + Path.GetExtension(s);
			return s.Replace(' ', '_');
		}

		/// <summary>
		/// Converts a string value to valid css class name.
		/// </summary>
		/// <param name="s">The specified string value.</param>
		/// <returns>A valid css class name.</returns>
		public static string ToCssClass(this string s)
		{
			s = s.Replace(' ', '-');
			s = s.Replace('_', '-');
			var validCharacters = new Regex("[^_a-zA-Z0-9-]");
			s = validCharacters.Replace(s, "");
			while (s.Length < 2) s = "_" + s;
			if (Char.IsNumber(s[0])) s = "_" + s;
			return s.ToLower();
		}

		/// <summary>
		/// Transforms a string value so it is url safe as a parameter.
		/// </summary>
		/// <param name="url">The original url string</param>
		/// <param name="stripDashes">MVC 4 doesn't support dashes in the route. Set this to true if the value is to be part of the route.</param>
		/// <returns>A valid url parameter which can be used for SEO (Search Engine optimization).</returns>
		public static string ToSeoUrl(this string url, bool stripDashes = false)
		{
			url = url.Trim().ToLower();
			url = url.Replace(' ', '-');
			url = url.Replace("&", "and");
			url = Regex.Replace(url, @"[^a-z0-9-]", "");
			url = Regex.Replace(url, @"-+", "-");
			if (stripDashes) url = url.Replace("-", "");
			return url;
		}

		/// <summary>
		/// Removes non alpha numeric characters from the specified string.
		/// </summary>
		/// <param name="stringValue">The specified string value.</param>
		/// <returns>A string value with alpha numeric characters only.</returns>
		public static string ToAlphaNumeric(this string stringValue)
		{
			var validCharacters = new Regex("[^a-zA-Z0-9]");
			var result = validCharacters.Replace(stringValue, "");
			return result;
		}

		/// <summary>
		/// Removes non alpha characters from the specified string.
		/// </summary>
		/// <param name="stringValue">The specified string value.</param>
		/// <returns>A string value with alpha characters only.</returns>
		public static string ToAlpha(this string stringValue)
		{
			var validCharacters = new Regex("[^a-zA-Z]");
			var result = validCharacters.Replace(stringValue, "");
			return result;
		}

		/// <summary>
		/// Removes non numeric characters from the specified string.
		/// </summary>
		/// <param name="stringValue">The specified string value.</param>
		/// <returns>A string value with numeric characters only.</returns>
		public static string ToNumeric(this string stringValue)
		{
			var validCharacters = new Regex("[^0-9]");
			var numbers = validCharacters.Replace(stringValue, "");
			return numbers;
		}

        /// <summary>
        /// Removes all the spaces in the string.
        /// </summary>
        /// <returns>A new string with the spaces removed.</returns>
        /// <param name="stringValue">Value.</param>
        public static string RemoveSpaces(this string stringValue)
        {
            return !string.IsNullOrEmpty(stringValue) ? stringValue.Replace(" ", string.Empty) : string.Empty;
        }

        /// <summary>
        /// Removes excess (double) white space characters from the specified string.
        /// </summary>
        /// <param name="stringValue">The specified string value.</param>
        /// <returns>A string value without excess white space character.</returns>
        public static string RemoveExcessWhiteSpaces(this string stringValue)
        {

            if (String.IsNullOrWhiteSpace(stringValue))
            {
                return stringValue;
            }

            return Regex.Replace(stringValue, @"\s+", " ").Trim();
        }
        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string.
        /// </summary>
        /// <param name="str">The specified string value.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace all occurrences of oldValue.</param>
        /// <param name="comparison">Specifies the culture, case, and sort rules to be used by string matching.</param>
        /// <returns>A string that is equivalent to the current string except that all instances of oldValue are replaced with newValue. If oldValue is not found in the current instance, the method returns the current instance unchanged.</returns>
        public static string Replace(this string str, string oldValue, string newValue, StringComparison comparison)
        {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }

        /// <summary>
        /// Returns a new string in which all occurrences of specified characters in the current instance are each replaced with another specified string.
        /// </summary>
        /// <param name="s">The specified string value.</param>
        /// <param name="charString">List of characters to replace</param>
        /// <param name="replace">Replace each character found with this string. Defaults as empty string to remove each character found</param>
        public static string ReplaceAny(this string s, string charString, string replace = "")
        {
            return Regex.Replace(s, $"[{charString}]", replace);
        }

        #endregion

        #region Trim 

        /// <summary>
        /// Removes leading string from the specified string.
        /// </summary>
        /// <param name="target">The specified string value.</param>
        /// <param name="trimString">Leading string to remove.</param>
        /// <returns>The string that remains after leading string removed from the specified string.</returns>
        public static string TrimStart(this string target, string trimString)
		{
			string result = target;
			while (!String.IsNullOrEmpty(trimString) && result.StartsWith(trimString))
			{
				result = result.Substring(trimString.Length);
			}

			return result;
		}

		/// <summary>
		/// Removes trailing string from the specified string.
		/// </summary>
		/// <param name="target">The specified string value.</param>
		/// <param name="trimString">Trailing string to remove.</param>
		/// <returns>The string that remains after trailing string removed from the specified string.</returns>
		public static string TrimEnd(this string target, string trimString)
		{
			string result = target;
			while (!String.IsNullOrEmpty(trimString) && result.EndsWith(trimString))
			{
				result = result.Substring(0, result.Length - trimString.Length);
			}

			return result;
		}

		/// <summary>
		/// Removes characters between the first start character and the first end character from the specified string.
		/// </summary>
		/// <param name="s">The specified string value.</param>
		/// <param name="start">The first start character</param>
		/// <param name="end">The first end character</param>
		/// <returns>The string that remains after characters between the first start character and the first end character removed from the specified string.</returns>
		public static string TrimBetween(this string s, char start, char end)
		{
			var result = s;
			while (true)
			{
				var startPos = result.IndexOf(start);
				var endPos = result.IndexOf(end, startPos > 0 && startPos + 2 < s.Length ? startPos + 1 : 0);
				if (startPos > -1 && endPos > -1 && endPos > startPos)
				{
					result = result.Remove(startPos, 1 + endPos - startPos);
				}
				else
				{ break; }
			}
			return result;
		}

		#endregion

		#region Format / Transform
		/// <summary>
		/// Replaces one or more format items in a specified string with the string representation of a specified object.
		/// </summary>
		/// <typeparam name="T">The type of the object.</typeparam>
		/// <param name="format">The specified string to format.</param>
		/// <param name="source">The object to format.</param>
		/// <returns>A copy of format in which any format items are replaced by the string representation of arg0 when source object is not null, otherwise empty string.</returns>
		public static string FormatIfNotNull<T>(this string format, T? source) where T : struct
		{
			if (source.HasValue) return String.Format(format, source.Value);
			return "";
		}

        /// <summary>
        /// Formats the string with a custom mask format string, where one * may be used as a wildcard
        /// </summary>
        /// <param name="s"></param>
        /// <param name="mask">eg XXXX* or *XXXX or XXX*XXX or XXXX or X</param>
        /// <returns></returns>
        public static string FormatWithMask(this string s, string mask)
        {
            if (String.IsNullOrEmpty(mask)) return "";
            var wildcard = mask.IndexOf('*');
            if (wildcard >= 0)
            {
                var @static = mask.Split('*').ToList();
                return String.Concat
                    (
                        @static[0],
                        String.Concat(s.Skip(@static[0].Length).Take(s.Length - @static[0].Length - @static[1].Length)),
                        @static[1]
                    );
            }
            else
            {
                return mask.Length > 1 ? mask : mask.Repeat(s.Length);
            }
        }

		/// <summary>
		/// Replaces {Placeholder} text in string with values from dictionary by matching place holder text to keys in dictionary.
		/// </summary>
		/// <param name="s">The specified string value.</param>
		/// <param name="dictionary">A System.Collections.Generic.Dictionary to replace.</param>
		/// <param name="removeUnused">True to remove unmatched keys from the specified string, otherwise not.</param>
		/// <returns>A string value with matched keys from dictionary replaced by values.</returns>
		public static string TransformWith(this string s, Dictionary<string, object> dictionary, bool removeUnused = false)
		{
			foreach (var key in dictionary.Keys)
			{
				s = s.Replace("{" + key + "}", dictionary[key] == null ? "" : dictionary[key].ToString());
			}
			if (removeUnused) { s = s.TrimBetween('{', '}'); }
			return s;
		}

		/// <summary>
		/// Replaces {Placeholder} text in string with property values from object.
		/// </summary>
		/// <param name="s">The specified string value.</param>
		/// <param name="transformWith">Properties in this object will be used to replace placeholders.</param>
		/// <returns>A string value replaced by property values from object.</returns>
		public static string TransformWith(this string s, object transformWith, bool removeUnused = false)
		{
            return TransformWith(s, transformWith.ToDictionary(), removeUnused);
		}

		/// <summary>
		/// Repeat a string n times
		/// </summary>
		/// <param name="s"></param>
		/// <param name="count">Number of times to repeat the string. 0 or this returns empty string.</param>
		/// <returns></returns>
		public static string Repeat(this string s, int count)
		{
			if (count <= 0) return String.Empty;
			var dest = new char[s.Length * count];
			for (int i = 0; i < dest.Length; i += 1)
			{
				dest[i] = s[i % s.Length];
			}
			return new string(dest);
		}
		#endregion

		#region Convert To and From

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

		/// <summary>
		/// Gets machine and framework version independent HashCode.
		/// </summary>
		/// <param name="value">The specified string value.</param>
		/// <returns>A hash code.</returns>
		public static int? GetIndependentHashCode(this string value)
		{
			if (value == null) return null;
			unchecked
			{
				int hash = 23;
				foreach (char c in value)
				{
					hash = hash * 31 + c;
				}
				return hash;
			}
		}

		/// <summary>
		/// Gets first not null value from parameter list.
		/// </summary>
		/// <param name="source">The specified string value.</param>
		/// <param name="nullDefaults">The specified string when it is not null, otherwise the first not null value from parameter list.</param>
		/// <returns></returns>
		public static string DefaultTo(this string source, params string[] nullDefaults)
		{
			if (!String.IsNullOrEmpty(source)) return source;

			foreach (string nullDefault in nullDefaults)
			{
				if (!String.IsNullOrEmpty(nullDefault)) return nullDefault;
			}

			return source; //Don't change if "" or null passed in as source
		}

		/// <summary>
		/// Converts specified string to byte array using UTF8 encoding.
		/// </summary>
		/// <param name="s">The specified string value.</param>
		/// <returns>A byte array.</returns>
		public static byte[] ToByteArray(this string s, Encoding encoding = null)
		{
			encoding = encoding ?? new UTF8Encoding();
			return encoding.GetBytes(s);
		}

		/// <summary>
		/// Converts the provided string to a byte array
		/// </summary>
		/// <returns>The byte array.</returns>
		/// <param name="hexString">Hex string.</param>
		public static byte[] ToByteArrayFromHex(this string hexString)
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
        /// Converts a string to System.Collections.Generic.IEnumerable&lt;T&gt by the specified delimiter, with each string in the list applying the defined converter function.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the generic enumerable list.</typeparam>
        /// <param name="source">A string value to convert.</param>
        /// <param name="delimiter">The delimiter to use.</param>
        /// <returns>A System.Collections.Generic.IEnumerable&lt;T&gt.</returns>
        public static IEnumerable<T> ToEnumerable<T>(this string source, string delimiter = ",") where T : IConvertible
        {
            if (String.IsNullOrWhiteSpace(source))
                return new List<T>();

            var list = source.Split(new[] { delimiter }, StringSplitOptions.None);
            return list.Select(x => String.IsNullOrEmpty(x) ? default(T) : (T)Convert.ChangeType(x, typeof(T)));
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns a value indicating whether the specified System.String object occurs within this string.
        /// </summary>
        /// <param name="source">The specified string value.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="comp">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>True if the value parameter occurs within this string, or if value is the empty string (""); otherwise, false.</returns>
        public static bool Contains(this string source, string value, StringComparison comp)
        {
            if (source == null) return false;
            return source.IndexOf(value, comp) >= 0;
        }

        #endregion

    }
}
