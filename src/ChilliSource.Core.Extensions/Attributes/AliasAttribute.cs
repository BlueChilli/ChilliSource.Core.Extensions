using System;
namespace ChilliSource.Core.Extensions
{
	/// <summary>
	/// Attaches an alias to an Enum value.
	/// The Enum value can be obtained from the alias by using ModelEnumExtensions.GetFromAlias&lt;Enum&gt;(alias) .
	/// </summary>
	public class AliasAttribute : Attribute
	{
		/// <summary>
		/// Alias name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChilliSource.Core.Extensions.AliasAttribute"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		public AliasAttribute(string name)
		{
			Name = name;
		}
	}
}
