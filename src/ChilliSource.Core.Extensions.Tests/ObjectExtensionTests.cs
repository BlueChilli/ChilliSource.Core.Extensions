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

namespace Tests
{
	public class ObjectExtensionTests
    {

		[Fact]
		public void ToJson_ShouldReturnJsonRepresentationOfObject()
		{
            var test = new { Id = 1, Name = "Bob", IsActive = false, Status = StringExtensionTests.TestEnum.Test1, DateCreated = new DateTime(2001, 10, 1) };

            var result = test.ToJson();
            Assert.Equal("{\"id\":1,\"name\":\"Bob\",\"isActive\":false,\"status\":\"Test1\",\"dateCreated\":\"2001-10-01T00:00:00\"}", result);

            result = test.ToJson(new JsonSerializerSettings { DateFormatString = "yyyy-mm-dd" });
            Assert.Equal("{\"Id\":1,\"Name\":\"Bob\",\"IsActive\":false,\"Status\":1,\"DateCreated\":\"2001-00-01\"}", result);

        }

    }
}
