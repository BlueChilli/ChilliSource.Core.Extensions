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
            var input = new List<int> { 1, 2 };
            input.AddOrUpdate(3, (Entity) => Entity);
            Assert.Contains(3, input);
        }

    }
}
