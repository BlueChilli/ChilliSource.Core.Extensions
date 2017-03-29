#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using Xunit;
using ChilliSource.Core;
using System.Linq;

namespace Tests
{
	public class EnumExtensionsTests
	{
		enum TestEnum
		{
			[System.ComponentModel.Description("Test1 Description")]
			Test1,
			[System.ComponentModel.Description("Test2 Description")]
			Test2
		}

		[Fact]
		public void GetAttributeOfType_ShouldReturnAttribute_IfTypeExists()
		{
			var result = TestEnum.Test1.GetAttributeOfType<System.ComponentModel.DescriptionAttribute>();

			Assert.NotNull(result);
			Assert.Equal(typeof(System.ComponentModel.DescriptionAttribute), result.GetType());
			Assert.Equal("Test1 Description", result.Description);

		}

		[Fact]
		public void GetValues_ShouldReturnListOfValuesForEnumType()
		{
			var result = EnumExtensions.GetValues<TestEnum>();
			Assert.NotNull(result);
			Assert.True(result.Count() > 0);
			Assert.Equal(TestEnum.Test1, result.First());

		}


	}
}
