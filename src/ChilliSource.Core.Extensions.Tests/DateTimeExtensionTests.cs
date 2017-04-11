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
	public class DateTimeExtensionTests
	{
		[Test]
		public void ChangeTime_ShouldReturnDateTimeWithNewTimeComponents()
		{
			var result = DateTime.Now.ChangeTime(22, 3, 5);
			Assert.AreEqual(22, result.Hour);
			Assert.AreEqual(3, result.Minute);
			Assert.AreEqual(5, result.Second);
		}

		[Test]
		public void RemoveTimeZone_ShouldReturnUTCDate()
		{
			var result = DateTime.Now.RemoveTimeZone();
			Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
		}

	}
}
