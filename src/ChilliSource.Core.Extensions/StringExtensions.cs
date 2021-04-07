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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace ChilliSource.Core.Extensions
{
    /// <summary>
    /// String extensions.
    /// </summary>
    public static class StringExtensions
    {
        #region Sentence formating
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

        #endregion

        #region Sanitise / Remove / Replace

        /// <summary>
        /// Truncate a string to a maximum length. If you want to including a truncating string use Truncate function from Humanzier.
        /// </summary>
        /// <param name="s">The specified string value.</param>
        /// <param name="maxLength">Maxlength of string to be returned</param>
        /// <returns>A string truncated to maxlength if applicable</returns>
        public static string TruncateHard(this string s, int maxLength)
        {
            return s.Substring(0, Math.Min(s.Length, maxLength));
        }

        /// <summary>
        /// Converts a string value to valid file path name.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <returns>A valid file path name.</returns>
        public static string ToFilename(this string value)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var c in invalidChars)
            {
                value = value.Replace(c.ToString(), "");
            }
            //remove additional periods
            var filename = Path.GetFileNameWithoutExtension(value).Replace(".", "");
            value = filename + Path.GetExtension(value);
            return value.Replace(' ', '_');
        }

        /// <summary>
        /// Converts a string value to valid CSS class name.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <returns>A valid CSS class name.</returns>
        public static string ToCssClass(this string value)
        {
            value = value.Replace(' ', '-');
            value = value.Replace('_', '-');

            var invalidCharacters = new Regex("[^_a-zA-Z0-9-]");
            value = invalidCharacters.Replace(value, "");

            while (value.Length < 2)
            {
                value = "_" + value;
            }

            if (Char.IsNumber(value[0]))
            {
                value = "_" + value;
            }

            return value.ToLower();
        }

        /// <summary>
        /// Transforms a string value so it is url-safe as a parameter.
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

            if (stripDashes)
            {
                url = url.Replace("-", "");
            }
            return url;
        }

        /// <summary>
        /// Removes non-alphanumeric characters from the specified string.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <returns>A string value with alpha numeric characters only.</returns>
        public static string ToAlphaNumeric(this string value)
        {
            var invalidCharacters = new Regex("[^a-zA-Z0-9]");
            var result = invalidCharacters.Replace(value, "");
            return result;
        }

        /// <summary>
        /// Removes non-alpha characters from the specified string.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <returns>A string value with alpha characters only.</returns>
        public static string ToAlpha(this string value)
        {
            var invalidCharacters = new Regex("[^a-zA-Z]");
            var result = invalidCharacters.Replace(value, "");
            return result;
        }

        /// <summary>
        /// Removes non-numeric characters from the specified string.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <returns>A string value with numeric characters only.</returns>
        public static string ToNumeric(this string value)
        {
            var invalidCharacters = new Regex("[^0-9]");
            var numbers = invalidCharacters.Replace(value, "");
            return numbers;
        }

        /// <summary>
        /// Removes all the spaces in the string.
        /// </summary>
        /// <returns>A new string with the spaces removed.</returns>
        /// <param name="value">Value.</param>
        public static string RemoveSpaces(this string value)
        {
            return !string.IsNullOrEmpty(value) ? value.Replace(" ", string.Empty) : string.Empty;
        }

        /// <summary>
        /// Removes excess (double) white space characters from the specified string.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <returns>A string value without excess white space character.</returns>
        public static string RemoveExcessWhiteSpaces(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return Regex.Replace(value, @"\s+", " ").Trim();
        }
        /// <summary>
        /// Returns a new string in which all occurrences of <paramref name="oldValue"/> in the current instance are replaced with <paramref name="newValue"/>.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace all occurrences of oldValue.</param>
        /// <param name="comparisonSettings">Specifies the culture, case, and sort rules to be used by string matching.</param>
        /// <returns>A string that had all occurences of <paramref name="oldValue"/> replaced with <paramref name="newValue"/>.</returns>
        public static string Replace(this string value, string oldValue, string newValue, StringComparison comparisonSettings)
        {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = value.IndexOf(oldValue, comparisonSettings);
            while (index != -1)
            {
                sb.Append(value.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = value.IndexOf(oldValue, index, comparisonSettings);
            }
            sb.Append(value.Substring(previousIndex));

            return sb.ToString();
        }

        /// <summary>
        /// Returns a new string in which the occurence of every character in <paramref name="characters"/> is replaced by <paramref name="replacement"/>.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <param name="characters">List of characters to replace</param>
        /// <param name="replacement">Replacement string. Defaults to empty string to remove each character found</param>
        public static string ReplaceAny(this string value, string characters, string replacement = "")
        {
            return Regex.Replace(value, $"[{characters}]", replacement);
        }

        #endregion

        #region Trim 

        /// <summary>
        /// Removes all occurences of <paramref name="trimValue"/> from the start of the specified <paramref name="value"/>
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <param name="trimValue">Leading string to remove.</param>
        /// <returns>The string that remains after leading string occurences have been removed from the specified string.</returns>
        public static string TrimStart(this string value, string trimValue)
        {
            string result = value;
            while (!String.IsNullOrEmpty(trimValue) && result.StartsWith(trimValue))
            {
                result = result.Substring(trimValue.Length);
            }

            return result;
        }

        /// <summary>
        /// Removes all occurences of <paramref name="trimValue"/> from the end of the specified <paramref name="value"/>
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <param name="trimValue">Trailing string to remove.</param>
        /// <returns>The string that remains after trailing string occurences have been removed from the specified string.</returns>
        public static string TrimEnd(this string value, string trimValue)
        {
            string result = value;
            while (!String.IsNullOrEmpty(trimValue) && result.EndsWith(trimValue))
            {
                result = result.Substring(0, result.Length - trimValue.Length);
            }

            return result;
        }

        /// <summary>
        /// Removes characters between the first occurence of the <paramref name="start"/> character and the first occurence of the <paramref name="end"/> character from the specified string.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <param name="start">The start character</param>
        /// <param name="end">The end character</param>
        /// <returns>The string that remains after the character removal.</returns>
        public static string TrimBetween(this string value, char start, char end)
        {
            var result = value;
            while (true)
            {
                var startPos = result.IndexOf(start);
                var endPos = result.IndexOf(end, startPos > 0 && startPos + 2 < value.Length ? startPos + 1 : 0);

                if (startPos > -1 && endPos > -1 && endPos > startPos)
                {
                    result = result.Remove(startPos, 1 + endPos - startPos);
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        #endregion

        #region Format / Transform
        /// <summary>
        /// Replaces the format placeholders in <paramref name="format"/> with the string representation of the <paramref name="object"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="format">The format string.</param>
        /// <param name="object">The object to format.</param>
        /// <returns>The formatted string when <paramref name="object"/> is not null, otherwise empty string.</returns>
        public static string FormatIfNotNull<T>(this string format, T? @object) where T : struct
        {
            if (@object.HasValue)
            {
                return String.Format(format, @object.Value);
            }
            return "";
        }

        /// <summary>
        ///  Replaces the format placeholders in <paramref name="format"/> with <paramref name="value"/>
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="value">The string replacement value.</param>
        /// <returns>The formatted string when <paramref name="value"/> is not null or empty, otherwise empty string.</returns>
        public static string FormatIfNotNull(this string format, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return String.Format(format, value);
            }
            return "";
        }

        /// <summary>
        /// Formats the specified <paramref name="value"/> with a custom <paramref name="mask"/> format string, where one * may be used as a wildcard
        /// </summary>
        /// <param name="value">The original string value</param>
        /// <param name="mask">eg XXXX* or *XXXX or XXX*XXX or XXXX or X</param>
        /// <returns>The formatted string</returns>
        public static string FormatWithMask(this string value, string mask)
        {
            if (String.IsNullOrEmpty(mask))
            {
                return "";
            }

            var wildcard = mask.IndexOf('*');
            if (wildcard >= 0)
            {
                var @static = mask.Split('*').ToList();
                return String.Concat
                    (
                        @static[0],
                        String.Concat(value.Skip(@static[0].Length).Take(value.Length - @static[0].Length - @static[1].Length)),
                        @static[1]
                    );
            }
            else
            {
                return mask.Length > 1 ? mask : mask.Repeat(value.Length);
            }
        }

        /// <summary>
        /// Replaces {Placeholder} text in <paramref name="value"/> with values from <paramref name="dictionary"/> by matching the placeholder text to keys in the dictionary.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <param name="dictionary">A System.Collections.Generic.IDictionary with replacement values.</param>
        /// <param name="removeUnused">Determines whether to remove unmatched {Placeholder} tags from the specified string.</param>
        /// <returns>The string with replaced values.</returns>
        public static string TransformWith(this string value, IDictionary<string, object> dictionary, bool removeUnused = false)
        {
            foreach (var key in dictionary.Keys)
            {
                value = value.Replace("{" + key + "}", dictionary[key] == null ? "" : dictionary[key].ToString());
            }

            if (removeUnused)
            {
                value = value.TrimBetween('{', '}');
            }
            return value;
        }

        /// <summary>
        /// Replaces {Placeholder} text in in <paramref name="value"/> with the property values of the <paramref name="transformWith"/> object.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <param name="transformWith">Object whose property values will be used as replacements for the placeholders matching the property names.</param>
        /// <param name="removeUnused">Determines whether to remove unmatched {Placeholder} tags from the specified string.</param>
        /// <returns>The string with replaced values.</returns>
        public static string TransformWith(this string value, object transformWith, bool removeUnused = false)
        {
            return TransformWith(value, transformWith.ToDictionary(), removeUnused);
        }

        /// <summary>
        /// Repeats the <paramref name="value"/> string <paramref name="count"/> times
        /// </summary>
        /// <param name="value">String value to be repeated</param>
        /// <param name="count">Number of times to repeat the string. A zero value results in an empty string being returned.</param>
        /// <returns>The repeated string</returns>
        public static string Repeat(this string value, int count)
        {
            if (count <= 0)
            {
                return String.Empty;
            }

            var strBuilder = new StringBuilder(value.Length * count);
            for (int i = 0; i < count; i++)
            {
                strBuilder.Append(value);
            }

            return strBuilder.ToString();
        }
        #endregion

        #region Convert To and From

        /// <summary>
        /// Converts the <paramref name="jsonValue"/> string to an object of type <typeparamref name="T"/>.
        /// By default Json is not formatted, uses camel casing and converts enums to strings.
        /// </summary>
        /// <typeparam name="T">The type of the resulting object.</typeparam>
        /// <param name="jsonValue">The JSON string.</param>
        /// <param name="resolver">The JSON serialization contract resolver to use</param>
        /// <returns>A new object of type <typeparamref name="T"/> representing the JSON string.</returns>        
        public static T FromJson<T>(this string jsonValue, Formatting format = Formatting.None, IContractResolver resolver = null)
        {
            if (resolver == null)
            {
                resolver = new CamelCasePropertyNamesContractResolver();
            }

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = format
            };
            settings.Converters.Add(new StringEnumConverter());

            return FromJson<T>(jsonValue, settings);
        }

        /// <summary>
        /// Converts the <paramref name="jsonValue"/> string to an object of type <typeparamref name="T"/> using custom deserialization settings.
        /// </summary>
        /// <typeparam name="T">The type of the object to convert.</typeparam>
        /// <param name="jsonValue">The JSON string.</param>
        /// <param name="settings">The custom serializer settings.</param>
        /// <returns>A new object of type <typeparamref name="T"/> representing JSON string.</returns>
        public static T FromJson<T>(this string jsonValue, JsonSerializerSettings settings)
        {
            if (jsonValue == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(jsonValue, settings);
        }

        /// <summary>
        /// Returns a MemoryStream with the bytes representing the <paramref name="value"/> encoded as <paramref name="encoding"/>
        /// </summary>
        /// <returns>The stream.</returns>
        /// <param name="value">Input string.</param>
        /// <param name="encoding">Encoding. Default is UTF8</param>
        public static Stream ToStream(this string value, Encoding encoding = null)
        {
            return new MemoryStream((encoding ?? Encoding.UTF8).GetBytes(value ?? ""));
        }

        /// <summary>
        /// Returns a machine and framework version independent hash code for the specified string <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <returns>The hash code representing the specified <paramref name="value"/>.</returns>
        public static int? GetIndependentHashCode(this string value)
        {
            if (value == null)
            {
                return null;
            }

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
        /// Returns either the <paramref name="value"/> or the first non-null default value from <paramref name="nullDefaults"/>
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <param name="nullDefaults">The list of null default values.</param>
        /// <returns>Either the <paramref name="value"/> or the first non-null default value from <paramref name="nullDefaults"/></returns>
        public static string DefaultTo(this string value, params string[] nullDefaults)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return value;
            }

            foreach (string nullDefault in nullDefaults)
            {
                if (!String.IsNullOrEmpty(nullDefault))
                {
                    return nullDefault;
                }
            }

            return value; //Don't change if "" or null passed in as source
        }

        /// <summary>
        /// Converts specified <paramref name="value"/> string to byte array using the specified <paramref name="encoding"/>.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <returns>A byte array.</returns>
        public static byte[] ToByteArray(this string value, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            return encoding.GetBytes(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="hexValue"/> string to a byte array
        /// </summary>
        /// <param name="hexValue">Hex string to convert.</param>
        /// <returns>The byte array.</returns>
        public static byte[] ToByteArrayFromHex(this string hexValue)
        {
            if (string.IsNullOrEmpty(hexValue))
            {
                return null;
            }

            int NumberChars = hexValue.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexValue.Substring(i, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// Splits the specified <paramref name="value"/> using the specified <paramref name="delimiter"/> and returns a System.Collections.Generic.IEnumerable&lt;T&gt; by changing the type of each element from string to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the generic enumerable list.</typeparam>
        /// <param name="value">The string value to convert.</param>
        /// <param name="delimiter">The delimiter that separates the individual items in the original string.</param>
        /// <returns>A System.Collections.Generic.IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> ToEnumerable<T>(this string value, string delimiter = ",") where T : IConvertible
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return new List<T>();
            }

            var list = value.Split(new[] { delimiter }, StringSplitOptions.None);
            return list.Select(x => String.IsNullOrEmpty(x) ? default(T) : (T)Convert.ChangeType(x, typeof(T)));
        }


        /// <summary>
        /// Converts the specified string to the type of object.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="s">The specified string value.</param>
        /// <returns>The strongly typed object representing the converted text.</returns>
        public static T To<T>(this string s)
        {
            var type = typeof(T);
            if (String.IsNullOrEmpty(s))
            {
                return default(T);
            }

            Type valueType;
            if (IsNullableType(type, out valueType))
            {
                return (T)ToNullableValueType(valueType, s);
            }

            return (T)Convert.ChangeType(s, type);
        }

        private static bool IsNullableType(Type theType, out Type valueType)
        {
            valueType = null;
            if (theType.GetTypeInfo().IsGenericType &&
            theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                var args = theType.GetTypeInfo().GetGenericArguments();
                if (args.Length > 0)
                    valueType = args[0];
            }

            return valueType != null;
        }

        //(e.g ValueType = int)
        // returns Nullable<int>
        private static object ToNullableValueType(Type valueType, string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                return TypeDescriptor.GetConverter(valueType).ConvertFrom(s);
            }
            return null;
        }

        /// <summary>
        /// Converts the specified string to the type of System.Nullable&lt;T&gt; generic type.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="s">The specified string value.</param>
        /// <returns>The System.Nullable&lt;T&gt; representing the converted text.</returns>
        public static Nullable<T> ToNullable<T>(this string s) where T : struct
        {
            return (Nullable<T>)ToNullableValueType(typeof(T), s);
        }

        /// <summary>
        /// Convert (if needed) unvalidated urls to form that will pass System.ComponentModel.DataAnnotationsUrlAttribute
        /// </summary>
        /// <param name="url">>The specified url value (can be null)</param>
        /// <returns>url</returns>
        public static string ToUrl(this string url)
        {
            if (String.IsNullOrEmpty(url)) return url;
            if (url.StartsWith("//")) return $"https:{url}";
            var validProtocols = new List<string> { "http://", "https://", "file://", "ftp://" };
            if (validProtocols.Any(x => url.StartsWith(x))) return url;
            return $"https://{url}";
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns a value indicating whether <paramref name="searchValue"/> occurs within <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The specified string value.</param>
        /// <param name="searchValue">The string to search for.</param>
        /// <param name="comparison">Specifies how the string comparison should be performed.</param>
        /// <returns>True if <paramref name="value"/> is either an empty string or contains <paramref name="searchValue"/>; false otherwise.</returns>
        public static bool Contains(this string value, string searchValue, StringComparison comparison)
        {
            if (value == null)
            {
                return false;
            }

            return value.IndexOf(searchValue, comparison) >= 0;
        }

        /// <summary>
        /// Determine if a string is comprised from the set of numeric characters or not
        /// </summary>
        /// <param name="s">The specified string value.</param>
        /// <returns>True if <paramref name="s"/> is comprised from the set of numeric characters; false otherwise.</returns>
        public static bool IsNumeric(this string s)
        {
            return long.TryParse(s, out _);
        }

        /// <summary>
        /// Determine if a string is comprised from the set of alphanumeric characters or not
        /// </summary>
        /// <param name="s">The specified string value.</param>
        /// <returns>True if <paramref name="s"/> is comprised from the set of alphanumeric characters; false otherwise.</returns>
        public static bool IsAlphaNumeric(this string s)
        {
            if (String.IsNullOrEmpty(s)) return false;

            string pattern = @"^[a-zA-Z0-9]*$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(s);
        }


        #endregion   
    }
}
