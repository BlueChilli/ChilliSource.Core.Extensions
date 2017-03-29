#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Core;
using Xunit;

namespace Tests
{
	public class DateTimeExtensionTests
	{
		[Fact]
		public void ChangeTime_ShouldReturnDateTimeWithNewTimeComponents()
		{
			var result = DateTime.Now.ChangeTime(22, 3, 5);
			Assert.Equal(22, result.Hour);
			Assert.Equal(3, result.Minute);
			Assert.Equal(5, result.Second);
		}

		[Fact]
		public void RemoveTimeZone_ShouldReturnUTCDate()
		{
			var result = DateTime.Now.RemoveTimeZone();
			Assert.Equal(DateTimeKind.Unspecified, result.Kind);
		}

	}
}
