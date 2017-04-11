#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Core.Extensions;
using System.Linq;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class EnumExtensionsTests
	{
		enum TestEnum
		{
			[System.ComponentModel.Description("Test1 Description")]
			Test1,
			[System.ComponentModel.Description("Test2 Description")]
			Test2
		}

		[Test]
		public void GetAttributeOfType_ShouldReturnAttribute_IfTypeExists()
		{
			var result = TestEnum.Test1.GetAttributeOfType<System.ComponentModel.DescriptionAttribute>();

			Assert.NotNull(result);
			Assert.AreEqual(typeof(System.ComponentModel.DescriptionAttribute), result.GetType());
			Assert.AreEqual("Test1 Description", result.Description);

		}

		[Test]
		public void GetValues_ShouldReturnListOfValuesForEnumType()
		{
			var result = EnumExtensions.GetValues<TestEnum>();
			Assert.NotNull(result);
			Assert.True(result.Count() > 0);
			Assert.AreEqual(TestEnum.Test1, result.First());

		}


	}
}
