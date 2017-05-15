#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Linq;

namespace ChilliSource.Core.Extensions
{
	/// <summary>
	/// Describes additional meta-data information in an Enum value.
	/// This can be obtained by the GetData Extension method (e.g TestEnum.Value.GetData&lt;string&gt;("metadata_xyz"))
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class DataAttribute : Attribute
	{
		/// <summary>
		/// Meta-data name
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Meta-data value. Must be a compile-time constant.
		/// </summary>
		public object Value { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChilliSource.Core.Extensions.DataAttribute"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		public DataAttribute(string name, object value)
		{
			Name = name;
			Value = value;
		}
	}




}
