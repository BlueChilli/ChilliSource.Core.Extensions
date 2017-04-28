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
using System.Linq;

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

        [Fact]
        public void DistincyBy_ShouldReturnUniqueItems()
        {
            var list = new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(1, 1), new KeyValuePair<int, int>(1, 2), new KeyValuePair<int, int>(2, 3) };
            var result = list.DistinctBy(x => x.Key).ToList();

            Assert.True(result.Count == 2);
            Assert.Contains<KeyValuePair<int, int>>(list, x => x.Key == 1 && x.Value == 1);
            Assert.Contains<KeyValuePair<int, int>>(list, x => x.Key == 2 && x.Value == 3);
        }

        [Fact]
        public void FirstOrNew_ShouldReturnNewWhenNotFound()
        {
            List<KeyValuePair<int, int>> emptyList = new List<KeyValuePair<int, int>>();
            var list = new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(1, 1), new KeyValuePair<int, int>(1, 2), new KeyValuePair<int, int>(2, 3) };

            var result = emptyList.FirstOrNew();
            var result2 = list.FirstOrNew(x => x.Key == 3);
            var result3 = list.FirstOrNew(x => x.Key == 1);

            Assert.Equal<KeyValuePair<int, int>>(result, new KeyValuePair<int, int>());
            Assert.Equal<KeyValuePair<int, int>>(result2, new KeyValuePair<int, int>());
            Assert.Equal<KeyValuePair<int, int>>(result3, new KeyValuePair<int, int>(1, 1));
        }

        [Fact]
        public void FirstOrDefaultTo_ShouldReturnDefaultWhenNotFound()
        {
            List<KeyValuePair<int, int>> emptyList = new List<KeyValuePair<int, int>>();
            var list = new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(1, 1), new KeyValuePair<int, int>(1, 2), new KeyValuePair<int, int>(2, 3) };
            var @default = new KeyValuePair<int, int>(1, 1);

            var result = emptyList.FirstOrDefaultTo(@default);
            var result2 = list.FirstOrDefaultTo(x => x.Key == 3, @default);
            var result3 = list.FirstOrDefaultTo(x => x.Key == 1, @default);

            Assert.Equal<KeyValuePair<int, int>>(result, @default);
            Assert.Equal<KeyValuePair<int, int>>(result2, @default);
            Assert.Equal<KeyValuePair<int, int>>(result3, new KeyValuePair<int, int>(1, 1));
        }

    }
}
