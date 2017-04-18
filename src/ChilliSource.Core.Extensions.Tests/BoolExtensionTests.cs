#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Core.Extensions;
using Xunit;

namespace Tests
{
	public class BoolExtensionTests
    {

		[Fact]
		public void ToInt_ShouldReturnEquivalentInteger_WhenGivenBoolean()
		{
            bool input = true;
			var result = input.ToInt();
			Assert.Equal(1, result);

            input = false;
            result = input.ToInt();
            Assert.Equal(0, result);
        }

		[Fact]
		public void Toggle_ShouldNegateValue_WhenGivenBoolean()
		{
            bool input = true;
            var result = input.Toggle();
            Assert.Equal(false, result);

            input = false;
            result = input.Toggle();
            Assert.Equal(true, result);
        }
	}
}
