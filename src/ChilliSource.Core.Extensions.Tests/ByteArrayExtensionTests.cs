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
	public class ByteArrayExtensionTests
	{

		[Test]
		public void ToHexString_ShouldReturnHexString_WhenByteArrayIsNotNull()
		{
			byte[] input = "68656C6C6F2068657820776F726C64".ToByteArray();
			var result = input.ToHexString();
            Assert.AreEqual("68656C6C6F2068657820776F726C64", result);
		}

		[Test]
		public void ToHexString_ShouldReturnNull_WhenByteArrayIsNull()
		{
			byte[] input = null;
			var result = input.ToHexString();
            Assert.Null(result);
		}
	}
}
