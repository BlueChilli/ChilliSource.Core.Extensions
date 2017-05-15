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
	public class GeometryExtensionTests
	{

		[Fact]
		public void ToDegrees_ShouldReturnDegrees()
		{
			var radians = 10.5f;
			var degrees = radians.ToDegrees();

			Assert.Equal(601, Math.Truncate(degrees));

            var radians2 = 10.5;
            var degrees2 = radians2.ToDegrees();

            Assert.Equal(601, Math.Truncate(degrees2));

        }

		[Fact]
		public void ToRadians_ShouldReturnRadians()
		{
			var degrees = 600.5f;
			var radians = degrees.ToRadians();

			Assert.Equal(10, Math.Truncate(radians));

            var degrees2 = 600.5;
            var radians2 = degrees2.ToRadians();

            Assert.Equal(10, Math.Truncate(radians2));
        }
	}
}
