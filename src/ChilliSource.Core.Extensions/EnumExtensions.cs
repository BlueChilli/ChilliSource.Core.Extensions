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
	/// Enum extensions.
	/// </summary>
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
		/// Usage: var values = GetValues&lt;MyEnumType&gt;();
		/// </summary>
		/// <returns>The values.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static IEnumerable<T> GetValues<T>() => Enum.GetValues(typeof(T)).Cast<T>();


		/// <summary>
		/// Converts the value of the specified enumeration to a 32-bit signed integral string.
		/// </summary>
		/// <param name="e">The specified enumeration value.</param>
		/// <returns>The integral string value of the specified enumeration.</returns>
		public static string ToValueString(this Enum e)
		{
			return Convert.ToInt32(e).ToString();
		}

		/// <summary>
		/// Appends flag value to the specified enumeration value.
		/// </summary>
		/// <typeparam name="T">Type of object to append flag value.</typeparam>
		/// <param name="type">The specified enumeration value.</param>
		/// <param name="enumFlag">Enumeration flag to append.</param>
		/// <returns>The object with flag value appended.</returns>
		public static T AddFlag<T>(this Enum type, T enumFlag)
		{
			try
			{
				return (T)(object)((int)(object)type | (int)(object)enumFlag);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("Could not append flag value {0} to enum {1}", enumFlag, typeof(T).Name), ex);
			}
		}

		/// <summary>
		/// Removes flag value form the specified enumeration value.
		/// </summary>
		/// <typeparam name="T">Type of object to remove flag value.</typeparam>
		/// <param name="type">The specified enumeration value.</param>
		/// <param name="enumFlag">Enumeration flag to remove.</param>
		/// <returns>The object with flag value removed.</returns>
		public static T RemoveFlag<T>(this Enum type, T enumFlag)
		{
			try
			{
				return (T)(object)((int)(object)type & ~(int)(object)enumFlag);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("Could not remove flag value {0} from enum {1}", enumFlag, typeof(T).Name), ex);
			}
		}

		/// <summary>
		/// Checks whether the specified enumeration value is same as the string value.
		/// </summary>
		/// <param name="e">The specified enumeration value.</param>
		/// <param name="value">The string value to match.</param>
		/// <returns>True when the specified enumeration value is same as the string value, otherwise false.</returns>
		public static bool Match(this Enum e, string value)
		{
			return (Enum.Parse(e.GetType(), value) == e);
		}

		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
		/// </summary>
		/// <typeparam name="T">Type of the object converted.</typeparam>
		/// <param name="e">The specified enumeration value.</param>
		/// <param name="value">A string containing the name or value to convert.</param>
		/// <returns>An object of type enumType whose value is represented by value.</returns>
		public static T Parse<T>(this Enum e, string value)
		{
			return (T)Enum.Parse(typeof(T), value);
		}

		/// <summary>
		/// Gets additional data set by DataAttribute in an Enum value
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="e">Enum value</param>
		/// <param name="name">meta-data name</param>
		/// <returns>Data value stored in the DataAttribute</returns>
		public static T GetData<T>(this Enum e, string name)
		{
			return (T)GetEnumData(e, name);
		}

		/// <summary>
		/// Gets additional data set by DataAttribute in an Enum value
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Enum value</param>
		/// <param name="name">>meta-data name</param>
		/// <returns>Data value stored in the DataAttribute</returns>
		public static object GetEnumData<TEnum>(TEnum value, string name)
		{
			var fi = value.GetType().GetTypeInfo().GetDeclaredField(value.ToString());

			DataAttribute[] attributes = (DataAttribute[])fi.GetCustomAttributes(typeof(DataAttribute), false);

			if (attributes.Length > 0)
			{
				foreach (var a in attributes)
				{
					if (a.Name == name) return a.Value;
				}
			}
			return null;
		}

		/// <summary>
		/// Sort list elements using a custom order attribute in Enum propery
		/// e.g : set Order on Enum values like: 
		/// public enum ResponseToEvent
		/// {
		/// [Order(1)]
		///  Going,
		/// [Order(3)]
		///  NotGoing,
		/// [Order(2)]
		/// Maybe
		/// }
		/// then order list by Enum property list.OrderBy(c => ModelEnumExtensions.GetOrder(c.ResponseToEvent))
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int GetOrder<TEnum>(TEnum value) where TEnum : struct
		{
			int order;

			if (!GetWithOrder<TEnum>.Values.TryGetValue(value, out order))
			{
				order = int.MaxValue;
			}

			return order;
		}

		private static class GetWithOrder<TEnum>
		{
			public static readonly Dictionary<TEnum, int> Values;

			static GetWithOrder()
			{
				var values = new Dictionary<TEnum, int>();

				var fields = typeof(TEnum).GetTypeInfo().GetFields(BindingFlags.Static | BindingFlags.Public);

				int unordered = int.MaxValue - 1;

				for (int i = fields.Length - 1; i >= 0; i--)
				{
					FieldInfo field = fields[i];

					var order = (OrderAttribute)field.GetCustomAttributes(typeof(OrderAttribute), false).FirstOrDefault();

					int order2;

					if (order != null)
					{
						order2 = order.Order;
					}
					else
					{
						order2 = unordered;
						unordered--;
					}

					values[(TEnum)field.GetValue(null)] = order2;
				}

				Values = values;
			}
		}
	}

}
