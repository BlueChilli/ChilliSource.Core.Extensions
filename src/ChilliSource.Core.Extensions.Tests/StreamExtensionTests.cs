#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.IO;
using NUnit.Framework;

using ChilliSource.Core.Extensions;

namespace Tests
{
	[TestFixture]
	public class StreamExtensionTests
	{
		[Test]
		public void ToByteArray_ShouldReturnByteArray_WhenStreamIsNotNull()
		{
			Stream input = "test string".ToStream();
			var resultBytes = input.ToByteArray();

			var resultStream = new MemoryStream(resultBytes);
			var reader = new StreamReader(resultStream);

			Assert.AreEqual("test string", reader.ReadToEnd());
		}

		[Test]
		public void ToByteArray_ShouldReturnNull_WhenStreamIsNull()
		{
			Stream input = null;
			Assert.Null(input.ToByteArray());
		}

	}
}
