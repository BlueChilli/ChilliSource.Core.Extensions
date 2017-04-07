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
	public static class StringExtensions
	{
        #region Sanitise
        /// <summary>
        /// Converts a string value to valid file path name.
        /// </summary>
        /// <param name="s">The specified string value.</param>
        /// <returns>A valid file path name.</returns>
        public static string ToFileName(this string s)
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
        /// <param name="stripDashes">MVC 4 doesn't support dashes in the route. Set this to true if the value is to be part of the route.</param>
        /// <returns>A valid url can be used for SEO (Search Engine optimization).</returns>
        public static string ToSeoUrl(this string s, bool stripDashes = false)
        {
            s = s.Trim().ToLower();
            s = s.Replace(' ', '-');
            s = s.Replace("&", "and");
            s = Regex.Replace(s, @"[^a-z0-9-]", "");
            s = Regex.Replace(s, @"-+", "-");
            if (stripDashes) s = s.Replace("-", "");
            return s;
        }

        /// <summary>
        /// Removes non alpha numeric characters from the specified string.
        /// </summary>
        /// <param name="s">The specified string value.</param>
        /// <returns>A string value with alpha numeric characters only.</returns>
        public static string ToAlphaNumeric(this string s)
        {
            var validCharacters = new Regex("[^a-zA-Z0-9]");
            var result = validCharacters.Replace(s, "");
            return result;
        }

        /// <summary>
        /// Removes non alpha characters from the specified string.
        /// </summary>
        /// <param name="s">The specified string value.</param>
        /// <returns>A string value with alpha characters only.</returns>
        public static string ToAlpha(this string s)
        {
            var validCharacters = new Regex("[^a-zA-Z]");
            var result = validCharacters.Replace(s, "");
            return result;
        }

        /// <summary>
        /// Removes non numeric characters from the specified string.
        /// </summary>
        /// <param name="s">The specified string value.</param>
        /// <returns>A string value with numeric characters only.</returns>
        public static string ToNumeric(this string s)
        {
            var validCharacters = new Regex("[^0-9]");
            var numbers = validCharacters.Replace(s, "");
            return numbers;
        }

        /// <summary>
        /// Removes punctuation characters from the specified string.
        /// </summary>
        /// <param name="source">The specified string value.</param>
        /// <returns>A string value without punctuation characters.</returns>
        public static string ExcludePunctuations(this string source)
        {
            if (String.IsNullOrWhiteSpace(source))
                return source;

            var r = Regex.Replace(source, @"[\p{P}\p{S}]", " ");

            return r.RemoveSpaces();
        }
        #endregion

        #region Truncate / Trim
        /// <summary>
        /// Truncates the specified string to maximum number of characters.
        /// </summary>
        /// <param name="s">The specified string value.</param>
        /// <param name="maxlength">Maximum number of characters.</param>
        /// <returns>A string value truncated.</returns>
        public static string Truncate(this string s, int maxlength)
        {
            if (String.IsNullOrEmpty(s)) return s;
            if (s.Length > maxlength) return s.Substring(0, maxlength);
            return s;
        }

        /// <summary>
        /// Truncates the specified string to maximum number of characters, and appends the second string.
        /// </summary>
        /// <param name="s">The specified string value.</param>
        /// <param name="maxlength">Maximum number of characters.</param>
        /// <param name="s2">The second string to append.</param>
        /// <param name="buffer">Additional number of characters.</param>
        /// <returns>A string value truncated and appended with the second string.</returns>
        public static string TruncateWith(this string s, int maxlength, string s2, int buffer = 0)
        {
            if (String.IsNullOrEmpty(s)) return s;
            if (s.Length > maxlength + buffer) s = s.Substring(0, maxlength) + s2;
            return s;
        }

        /// <summary>
        /// Removes all white space characters from the specified string.
        /// </summary>
        /// <param name="source">The specified string value.</param>
        /// <returns>A string value without white space character.</returns>
        public static string TrimExcessWhiteSpaces(this string source)
        {

            if (String.IsNullOrWhiteSpace(source))
                return source;

            return Regex.Replace(source, @"\s+", " ").Trim();
        }

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
                var endPos = result.IndexOf(end);
                if (startPos > -1 && endPos > -1 && endPos > startPos)
                    result = result.Remove(startPos, 1 + endPos - startPos);
                else
                    break;
            }
            return result;
        }

        public static string RemoveSpaces(this string value)
        {
            return !string.IsNullOrEmpty(value) ? value.Replace(" ", string.Empty) : string.Empty;
        }
        #endregion

        #region Format / Transform / Replace etc
        /// <summary>
        /// Replaces the format item in a specified string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="format">The specified string to format.</param>
        /// <param name="source">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.</returns>
        public static string FormatWith(this string format, params object[] source)
        {
            //handle placeholders encoded in urls
            var matches = Regex.Matches(format, "%7B\\d+%7D", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                format = format.Replace(match.Value, match.Value.Replace("%7B", "{", StringComparison.OrdinalIgnoreCase).Replace("%7D", "}", StringComparison.OrdinalIgnoreCase));
            }
            return String.Format(format, source);
        }

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
            if (removeUnused) s = s.TrimBetween('{', '}');
            return s;
        }

        /// <summary>
        /// Replaces {Placeholder} text in string with property values from object.
        /// </summary>
        /// <param name="s">The specified string value.</param>
        /// <param name="transformWith">Properties in this object will be used to replace placeholders.</param>
        /// <returns>A string value replaced by property values from object.</returns>
        public static string TransformWith(this string s, object transformWith)
        {
            Type t = transformWith.GetType();
            foreach (PropertyInfo property in t.GetProperties())
            {
                var value = property.GetValue(transformWith, null);
                if (value == null) value = "";
                s = s.Replace("{" + property.Name + "}", value.ToString());
                s = s.Replace("%7B" + property.Name + "%7D", value.ToString());
            }

            return s;
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
            return Regex.Replace(s, "[{0}]".FormatWith(charString), replace);
        }

        /// <summary>
        /// Inverts the order of each word in the specified string.
        /// </summary>
        /// <param name="source">The specified string value.</param>
        /// <returns>A string value with each word inverted.</returns>
        public static string ReverseWord(this string source)
        {
            if (String.IsNullOrWhiteSpace(source))
                return source;

            var words = Regex.Split(source.TrimExcessWhiteSpaces(), @"\s+");

            var reversedWords = words.Reverse();

            return String.Join(" ", reversedWords).Trim();
        }

        /// <summary>
        /// Replaces characters between start index to end index in the specified string with mask character.
        /// </summary>
        /// <param name="s">The specified string value.</param>
        /// <param name="mask">Mask character.</param>
        /// <param name="startFrom">The start index.</param>
        /// <param name="endFrom">The end index.</param>
        /// <returns>A string value masked with the mask character.</returns>
        public static string Mask(this string s, char mask = 'X', int startFrom = 0, int endFrom = 4)
        {
            if (String.IsNullOrEmpty(s)) return s;

            var c = s.ToCharArray();
            endFrom = c.Length - endFrom - 1;
            for (var i = 0; i < c.Length; i++)
            {
                if (i < startFrom || i > endFrom) continue;
                c[i] = mask;
            }
            return new String(c);
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
        /// Formats the specified string to sentence case.
        /// </summary>
        /// <param name="s">The specified string to format.</param>
        /// <param name="splitByUppercase">True to add space before upper case character, otherwise not.</param>
        /// <returns>A string value formatted to sentence case.</returns>
        public static string ToSentenceCase(this string s, bool splitByUppercase = false)
        {
            string input = splitByUppercase ? s.SplitByUppercase() : s;

            if (input.Length == 0)
                return input;

            return input.Substring(0, 1) + input.Substring(1).ToLower();
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
        public static byte[] ByteArray(this string s)
        {
            var encoding = new UTF8Encoding();
            return encoding.GetBytes(s);
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
        #endregion

        #region String based helper functions
        /// <summary>
        /// Concatenates the members of a string array, using the specified separator between each member.
        /// </summary>
        /// <param name="seperator">The string to use as a separator. separator is included in the returned string only if values has more than one element.</param>
        /// <param name="strings">A string array that contains the strings to concatenate.></param>
        /// <returns>A string that consists of the members of values delimited by the separator string. If values has no members, the method returns System.String.Empty.</returns>
        public static string JoinIfNotNull(string seperator, params string[] strings)
        {
            var result = "";
            foreach (var item in strings)
            {
                if (!String.IsNullOrEmpty(item))
                    result += seperator + item;
            }
            return result.TrimStart(seperator.ToCharArray());
        }

        /// <summary>
        /// Checks whether all elements in the specified string array are null or empty.
        /// </summary>
        /// <param name="sources">The specified string array to check.</param>
        /// <returns>True if all elements in the specified string array are null or empty, otherwise false.</returns>
        public static bool IsAllNullOrEmpty(params string[] sources)
        {
            foreach (string s in sources) if (!String.IsNullOrEmpty(s)) return false;
            return true;
        }

        /// <summary>
        /// Returns the string representing the object which is not null.
        /// </summary>
        /// <param name="source">The source object to check.</param>
        /// <param name="nullDefault">The default string when source object is null.</param>
        /// <returns>The default string when source object is null, otherwise the string representing the source object.</returns>
        public static string DefaultTo(object source, string nullDefault)
        {
            return (source == null) ? nullDefault : DefaultTo(source.ToString(), nullDefault);
        }

        /// <summary>
        /// Replaces format item in a specified string.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="source">The specified string to format.</param>
        /// <returns>A copy of format in which the format item replaced.</returns>
        public static string FormatIfNotNull(string format, string source)
        {
            if (String.IsNullOrEmpty(source)) return "";
            return String.Format(format, source);
        }

        public static string ValueOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value) ? value : "";
        }

        public static string ValueOrReplacement(this string value, string replacementValue = "")
        {
            return !string.IsNullOrEmpty(value) ? value : replacementValue;
        }

        /// <summary>
        /// Adds space before upper case character in the specified string.
        /// </summary>
        /// <param name="s">The specified string to process.</param>
        /// <returns>A string value with space before upper case character.</returns>
        public static string SplitByUppercase(this string s)
        {
            MatchCollection mc = Regex.Matches(s, @"(\p{Nd}+)|(\P{Lu}+)|(\p{Lu}+\p{Ll}*)");
            string result = "";
            foreach (Match m in mc)
            {
                result += m.ToString() + " ";
            }
            return result.TrimEnd(' ');
        }

        /// <summary>
        /// Capitalises the specified string. 
        /// </summary>
        /// <param name="s">The specified string.</param>
        /// <param name="allWords">True to capitalise all word in the string, false to capitalise the first word only.</param>
        /// <returns>A capitalised string value.</returns>
        public static string Capitalise(this string s, bool allWords = false)
        {
            if (String.IsNullOrEmpty(s)) return s;

            if (allWords)
            {
                return String.Join(" ", s.Split(' ').Select(x => x.Substring(0, 1).ToUpper() + x.Substring(1)));
            }
            return s.Substring(0, 1).ToUpper() + s.Substring(1);
        }

        /// <summary>
        /// Returns a value indicating whether the specified System.String object occurs within this string.
        /// </summary>
        /// <param name="source">The specified string value.</param>
        /// <param name="toCheck">The string to seek.</param>
        /// <param name="comp">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>True if the value parameter occurs within this string, or if value is the empty string (""); otherwise, false.</returns>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            if (source == null) return false;
            return source.IndexOf(toCheck, comp) >= 0;
        }
        #endregion
    }
}
