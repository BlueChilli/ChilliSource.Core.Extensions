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
using System.Reflection;

namespace ChilliSource.Core.Extensions
{ 
	/// <summary>
	/// Object extensions. 
	/// </summary>
	public static class ObjectExtensions
	{
        /// <summary>
        /// Converts object to a dictionary of objects, keyed by property name
        /// </summary>
        /// <param name="value">Object to convert.</param>
        /// <returns>An dictionary containing each property as key, with property value as value.</returns>
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
