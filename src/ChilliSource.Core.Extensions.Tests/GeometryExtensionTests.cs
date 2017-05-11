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
			float radiants = 10.5f;
			var degrees = radiants.ToDegrees();

			Assert.Equal(601, Math.Truncate(degrees));

            double radiants2 = 10.5;
            var degrees2 = radiants2.ToDegrees();

            Assert.Equal(601, Math.Truncate(degrees2));

        }

		[Fact]
		public void ToRadiants_ShouldReturnRadiants()
		{
			float degrees = 600.5f;
			var radiants = degrees.ToRadiants();

			Assert.Equal(10, Math.Truncate(radiants));

            double degrees2 = 600.5;
            var radiants2 = degrees2.ToRadiants();

            Assert.Equal(10, Math.Truncate(radiants2));
        }
	}
}
