using System;
using System.Collections.Generic;
using System.Text;

namespace ChilliSource.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool AddOrSkipIfExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                return false;

            dictionary.Add(key, value);
            return true;
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue, TValue2>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue2> dictionary2, bool overwrite = false)
            where TValue2 : TValue
        {
            foreach (var key in dictionary2.Keys)
            {
                if (!dictionary.ContainsKey(key))
                    dictionary.Add(key, dictionary2[key]);
                else if (overwrite)
                    dictionary[key] = dictionary2[key];
            }

            return dictionary;
        }

        /// <summary>
        /// Check if dictionary contains value for specified key
        /// </summary>
        /// <param name="dictionary">Dictionary to perform check on</param>
        /// <param name="key">Key to look up in dictionary. Returns false if key not found.</param>
        /// <param name="value">Value to check. Empty string is considered same as null string.</param>
        /// <returns>Return true if dictionary contains value for specified key, false otherwise</returns>
        public static bool HasValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                var dictionaryValue = dictionary[key];
                if (dictionaryValue == null)
                {
                    if (value == null) return true;
                    var stringValue = value as string;
                    return stringValue == String.Empty;
                }
                return dictionaryValue.Equals(value);
            }
            return false;
        }
    }
}
