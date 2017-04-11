#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Core.Extensions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class GeometryExtensionTests
    {

        [Test]
        public void ToDegrees_ShouldReturnDegrees()
        {
            float radiants = 10.5f;
            var degrees = radiants.ToDegrees();

            Assert.AreEqual(601, Math.Truncate(degrees));
        }

        [Test]
        public void ToRadiants_ShouldReturnRadiants()
        {
            float degrees = 600.5f;
            var radiants = degrees.ToRadiants();

            Assert.AreEqual(10, Math.Truncate(radiants));
        }
    }
}
