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
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tests
{
	public class DictionaryExtensionTests
    {

		[Fact]
		public void HasValueWorks()
		{
            var test = new Dictionary<int, string> { { 1, "Jim" }, { 2, "Jane" }, { 3, "Fred" } };

            Assert.True(test.HasValue(1, "Jim"));
            Assert.True(test.HasValue(2, "Jane"));
            Assert.True(test.HasValue(3, "Fred"));

            Assert.False(test.HasValue(2, "Jim"));
            Assert.False(test.HasValue(1, "Jane"));
            Assert.False(test.HasValue(int.MinValue, null));
            Assert.False(test.HasValue(1, null));
        }

    }
}
