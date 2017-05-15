#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChilliSource.Core.Extensions
{ 
	/// <summary>
	/// Object extensions. 
	/// </summary>
	public static class ObjectExtensions
	{

        /// <summary>
        /// Converts the specified object to a JSON string.
        /// By default Json is not formatted, uses camel casing and converts enums to strings.
        /// If resolver is not specified it defaults to CamelCasePropertyNamesContractResolver.
        /// </summary>
        /// <param name="data">Object to convert.</param>
        /// <returns>JSON string representing the object.</returns>
        public static string ToJson(this object data, Formatting format = Formatting.None, IContractResolver resolver = null)
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

            return ToJson(data, settings);
        }

        /// <summary>
        /// Converts the specified object to a JSON string.
        /// </summary>
        /// <param name="data">Object to convert.</param>
        /// <param name="settings">The custom serializer settings.</param>
        /// <returns>JSON string representing the object.</returns>
        public static string ToJson(this object data, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(data, settings);
        }

        /// <summary>
        /// Converts specified object to a dictionary of objects, keyed by property name
        /// </summary>
        /// <param name="value">Object to convert.</param>
        /// <returns>A dictionary containing key-value pairs for each property and its value.</returns>
        public static Dictionary<string, object> ToDictionary(this object value)
        {
            var result = new Dictionary<string, object>();
            var type = value.GetType();
            foreach (var property in type.GetTypeInfo().GetProperties())
            {
                result.Add(property.Name, property.GetValue(value));
            }
            return result;
        }

    }

}
