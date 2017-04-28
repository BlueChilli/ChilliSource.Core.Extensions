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
	public class LinqExtensionTests
    {

        class Collection1
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public static List<Collection1> GetMe()
            {
                return new List<Collection1> { new Collection1 { Id = 1, Name = "Bob" }, new Collection1 { Id = 2, Name = "Jim" }, new Collection1 { Id = 3, Name = "Sam" }, new Collection1 { Id = 4, Name = "Sue" } };
            }
        }

        class Collection2
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public static List<Collection2> GetMe()
            {
                return new List<Collection2> { new Collection2 { Id = 1, Description = "Blue" }, new Collection2 { Id = 2, Description = "Red" }, new Collection2 { Id = 3, Description = "Orange" }, new Collection2 { Id = 5, Description = "Black" } };
            }

        }

        [Fact]
		public void LeftOuterJoin_ReturnsJoin_OnKey()
		{
            var result = Collection1.GetMe().LeftOuterJoin(Collection2.GetMe(), x => x.Id, y => y.Id, (i, j) => new { Id = i.Id, i?.Name, j?.Description });

            Assert.True(result.Count() == 4);

            var bob = result.First();
            Assert.True(bob.Id == 1 && bob.Name == "Bob" && bob.Description == "Blue");

            var sue = result.Last();
            Assert.True(sue.Id == 4 && sue.Name == "Sue" && sue.Description == null);
        }

        [Fact]
        public void RightOuterJoin_ReturnsJoin_OnKey()
        {
            var result = Collection1.GetMe().RightOuterJoin(Collection2.GetMe(), x => x.Id, y => y.Id, (i, j) => new { Id = j.Id, i?.Name, j?.Description });

            Assert.True(result.Count() == 4);

            var bob = result.First();
            Assert.True(bob.Id == 1 && bob.Name == "Bob" && bob.Description == "Blue");

            var black = result.Last();
            Assert.True(black.Id == 5 && black.Name == null && black.Description == "Black");
        }

    }
}
