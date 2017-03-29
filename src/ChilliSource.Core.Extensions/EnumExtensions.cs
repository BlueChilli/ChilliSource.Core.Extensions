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


namespace ChilliSource.Core
{
	public static class EnumExtensions
	{
		/// <summary>
		/// Returns the Enum attribute with type T of the provided Enum item <paramref name="value"/>
		/// </summary>
		/// <returns>The attribute.</returns>
		/// <param name="value">Enum item</param>
		/// <typeparam name="T">The type of the attribute to return</typeparam>
		public static T GetAttributeOfType<T>(this Enum value) where T : Attribute
		{
			var typeInfo = value.GetType().GetTypeInfo();
			var memberInfo = typeInfo.DeclaredMembers.FirstOrDefault(x => x.Name == value.ToString());

			if (memberInfo != null)
			{
				return memberInfo.GetCustomAttribute<T>();
			}

			return null;
		}

		/// <summary>
		/// Loops through all values of an enum and returns them as an IEnumerable
		/// Usage: var values = GetValues<MyEnumType>();
		/// </summary>
		/// <returns>The values.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static IEnumerable<T> GetValues<T>() => Enum.GetValues(typeof(T)).Cast<T>();

	}

}
