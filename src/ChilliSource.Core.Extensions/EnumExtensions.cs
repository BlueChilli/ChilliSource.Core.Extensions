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
		/// Loops through all values of an enum and returns them as an IEnumerable
		/// Usage: var values = GetValues&lt;MyEnumType&gt;();
		/// </summary>
		/// <returns>The values.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static IEnumerable<T> GetValues<T>(bool excludeObsolete = true) where T : struct
        {
            var values = Enum.GetValues(typeof(T)).Cast<Enum>();
            if (excludeObsolete)
            {
                values = values.Where(v => v.GetAttribute<ObsoleteAttribute>() == null);
            }
            return values.Cast<T>();
        }

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
		/// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
		/// </summary>
		/// <typeparam name="T">Type of the object converted.</typeparam>
		/// <param name="e">The specified enumeration value.</param>
		/// <param name="value">A string containing the name or value to convert.</param>
		/// <returns>An object of type enumType whose value is represented by value.</returns>
		public static T Parse<T>(this Enum e, string value) where T : struct
		{
            T result;
            if (Enum.TryParse<T>(value, out result))
            {
                return result;
            }
            return default(T);
		}

        /// <summary>
        /// Checks whether the specified enumeration value is in the enumeration parameter list.
        /// </summary>
        /// <typeparam name="T">The type of object to check.</typeparam>
        /// <param name="e">The specified enumeration.</param>
        /// <param name="list">The enumeration parameter list</param>
        /// <returns>True when the specified enumeration value is in the enumeration parameter list, otherwise false.</returns>
        public static bool Contains<T>(this T e, params T[] list) where T : struct, IConvertible, IFormattable
        {
            return Contains<T>(e, (list as IEnumerable<T>) ?? Enumerable.Empty<T>());
        }

        /// <summary>
        /// Checks whether the specified enumeration value is in the System.Collections.Generic.IEnumerable%lt;T&gt; list.
        /// </summary>
        /// <typeparam name="T">The type of object to check.</typeparam>
        /// <param name="e">The specified enumeration.</param>
        /// <param name="list">System.Collections.Generic.IEnumerable%lt;T&gt; list.</param>
        /// <returns>True when the specified enumeration value is in the System.Collections.Generic.IEnumerable%lt;T&gt; list, otherwise false.</returns>
        public static bool Contains<T>(this T e, IEnumerable<T> list) where T : struct, IConvertible, IFormattable
        {
            Type t = e.GetType();
            var enumValue = e as Enum;

            bool isFlags = t.GetTypeInfo().GetCustomAttribute<FlagsAttribute>() != null;

            foreach (T item in list)
            {
                if (e.Equals(item) || (isFlags && enumValue.HasFlag(item as Enum))) return true;
            }
            return false;
        }

        /// <summary>
        /// Return the next enum in sequence
        /// </summary>
        public static T Next<T>(this T src) where T : struct
        {
            CheckTIsEnum(src);

            var values = GetValues<T>().ToList();
            int j = values.IndexOf(src) + 1;
            return (values.Count == j) ? values[0] : values[j];
        }

        /// <summary>
        /// Return the previous enum in sequence
        /// </summary>
        public static T Previous<T>(this T src) where T : struct
        {
            CheckTIsEnum(src);

            var values = GetValues<T>().ToList();
            int j = values.IndexOf(src) -1;
            return (j < 0) ? values.Last() : values[j];
        }

        #region Flag helpers
        /// <summary>
        /// Appends flag value to the specified enumeration value.
        /// </summary>
        /// <typeparam name="T">Type of object to append flag value.</typeparam>
        /// <param name="type">The specified enumeration value.</param>
        /// <param name="enumFlag">Enumeration flag to append.</param>
        /// <returns>The object with flag value appended.</returns>
        public static T AddFlag<T>(this Enum type, T enumFlag) where T : struct, IConvertible, IFormattable
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
        public static T RemoveFlag<T>(this Enum type, T enumFlag) where T : struct, IConvertible, IFormattable
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
        /// Converts enumeration type with Flags attribute to System.Collections.Generic.List&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Type of the object converted.</typeparam>
        /// <param name="e">>The specified enumeration type.</param>
        /// <returns>A System.Collections.Generic.List&lt;T&gt;.</returns>
        public static List<T> ToFlagsList<T>(this T e) where T : struct, IConvertible, IFormattable //The compiler doesnt allow [where T: System.Enum]
        {
            var enumValue = e as Enum;
            return GetValues<T>().Where(flag => enumValue.HasFlag(flag as Enum)).ToList();
        }

        public static T ToFlags<T>(this List<T> flagsList) where T : struct, IConvertible, IFormattable
        {
            T obj1 = default(T);
            foreach (T obj2 in flagsList)
                obj1 = (T)(ValueType)((int)(ValueType)obj1 | (int)(ValueType)obj2);
            return obj1;
        }


        #endregion

        #region Attributes helpers
        /// <summary>
        /// Returns the Enum attribute with type T of the provided Enum item <paramref name="value"/>
        /// </summary>
        /// <returns>The attribute.</returns>
        /// <param name="value">Enum item</param>
        /// <typeparam name="T">The type of the attribute to return</typeparam>
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var memberInfo = value.GetType().GetTypeInfo().GetDeclaredField(value.ToString());

            if (memberInfo != null)
            {
                return memberInfo.GetCustomAttribute<T>();
            }

            return null;
        }

        /// <summary>
        /// Returns the Enum attributes with type T of the provided Enum item <paramref name="value"/>
        /// </summary>
        /// <returns>The attribute.</returns>
        /// <param name="value">Enum item</param>
        /// <typeparam name="T">The type of the attribute to return</typeparam>
        public static IEnumerable<T> GetAttributes<T>(this Enum value) where T : Attribute
        {
            var memberInfo = value.GetType().GetTypeInfo().GetDeclaredField(value.ToString());

            if (memberInfo != null)
            {
                return memberInfo.GetCustomAttributes<T>();
            }

            return null;
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
            var attributes = e.GetAttributes<DataAttribute>();
            if (attributes.Count() > 0)
            {
                foreach (var a in attributes)
                {
                    if (a.Name == name) return (T)a.Value;
                }
            }
            return default(T);
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
		public static int GetOrder<TEnum>(TEnum value) where TEnum : struct, IConvertible, IFormattable
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
        #endregion

        private static void CheckTIsEnum<T>(T e) where T : struct
        {
            if (!typeof(T).GetTypeInfo().IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));
        }
    }

}
