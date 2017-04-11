#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using NUnit.Framework;
using ChilliSource.Core.Extensions;
using System.IO;

namespace Tests
{
	[TestFixture]
	public class StringExtensionTests
	{
		[Test]
		public void ValueOrEmpty_ShouldReturnValue_WhenValueIsNotEmpty()
		{
			var result = "test".ValueOrEmpty();
			Assert.AreEqual("test", result);
		}

		[Test]
		public void ValueOrEmpty_ShouldReturnEmptyString_WhenValueIsEmpty()
		{
			var result = "".ValueOrEmpty();
			Assert.AreEqual(string.Empty, result);
		}

		[Test]
		public void ValueOrReplacement_ShouldReturnValue_WhenValueIsNotEmpty()
		{
			var result = "test".ValueOrReplacement();
			Assert.AreEqual("test", result);
		}

		[Test]
		public void ValueOrReplacement_ShouldReturnReplacementValue_WhenValueIsEmpty()
		{
			var result = "".ValueOrReplacement("replacement");
			Assert.AreEqual("replacement", result);
		}

		[Test]
		public void RemoveSpaces_ShouldRemoveSpaces_WhenGivenStringWithSpaces()
		{
			var result = "test1 test2 test3".RemoveSpaces();
			Assert.AreEqual("test1test2test3", result);
		}

		[Test]
		public void RemoveSpaces_ShouldReturnEmptyString_WhenGivenNull()
		{
			string input = null;
			var result = input.RemoveSpaces();
			Assert.AreEqual("", result);
		}

		[Test]
		public void ToByteArray_ShouldReturnByteArray_WhenHexStringIsValid()
		{
			var result = "68656C6C6F2068657820776F726C64".ToByteArray();
			Assert.AreEqual("68656C6C6F2068657820776F726C64", result.ToHexString());
		}

		[Test]
		public void ToByteArray_ShouldThrowException_WhenHexStringIsNotValid()
		{
			Assert.Throws(typeof(FormatException), () => "invalid hex string".ToByteArray());
		}

		[Test]
		public void ToByteArray_ShouldReturnNull_WhenHexStringIsNullOrEmpty()
		{
			string input = null;
			var result = input.ToByteArray();
			Assert.IsNull(result);
		}

		[Test]
		public void ToStream_ShouldReturnStream()
		{
			var result = "test string".ToStream();
			var reader = new StreamReader(result);

			Assert.AreEqual("test string", reader.ReadToEnd());
		}
	}
}
