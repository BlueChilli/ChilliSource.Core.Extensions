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
using System.Collections.Generic;

namespace Tests
{
	public class CollectionExtensionsTests
    {

		[Fact]
		public void AddOrUpdate_ShouldAddItem_WhenDoesntExist()
		{
            var input = new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(1, 1), new KeyValuePair<int, int>(2, 2), new KeyValuePair<int, int>(3, 3) };
            input.AddOrUpdate(new KeyValuePair<int, int>(4, 4), (Entity) => Entity.Key);
            Assert.Contains(new KeyValuePair<int, int>(4, 4), input);
        }

        [Fact]
        public void AddOrUpdate_ShouldUpdateItem_WhenDoesExist()
        {
            var input = new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(1, 1), new KeyValuePair<int, int>(2, 2), new KeyValuePair<int, int>(3, 3) };
            input.AddOrUpdate(new KeyValuePair<int, int>(3, 4), (Entity) => Entity.Key);
            Assert.Contains(new KeyValuePair<int, int>(3, 4), input);
            Assert.DoesNotContain(new KeyValuePair<int, int>(3, 3), input);
        }

    }
}
