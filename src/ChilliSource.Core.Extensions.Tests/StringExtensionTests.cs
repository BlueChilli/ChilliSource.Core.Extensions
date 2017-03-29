#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using Xunit;

using ChilliSource.Core;
using System.IO;

namespace Tests
{
	public class StringExtensionTests
	{
		[Fact]
		public void ValueOrEmpty_ShouldReturnValue_WhenValueIsNotEmpty()
		{
			var result = "test".ValueOrEmpty();
			Assert.Equal("test", result);
		}

		[Fact]
		public void ValueOrEmpty_ShouldReturnEmptyString_WhenValueIsEmpty()
		{
			var result = "".ValueOrEmpty();
			Assert.Equal(string.Empty, result);
		}

		[Fact]
		public void ValueOrReplacement_ShouldReturnValue_WhenValueIsNotEmpty()
		{
			var result = "test".ValueOrReplacement();
			Assert.Equal("test", result);
		}

		[Fact]
		public void ValueOrReplacement_ShouldReturnReplacementValue_WhenValueIsEmpty()
		{
			var result = "".ValueOrReplacement("replacement");
			Assert.Equal("replacement", result);
		}

		[Fact]
		public void RemoveSpaces_ShouldRemoveSpaces_WhenGivenStringWithSpaces()
		{
			var result = "test1 test2 test3".RemoveSpaces();
			Assert.Equal("test1test2test3", result);
		}

		[Fact]
		public void RemoveSpaces_ShouldReturnEmptyString_WhenGivenNull()
		{
			string input = null;
			var result = input.RemoveSpaces();
			Assert.Equal("", result);
		}

		[Fact]
		public void ToByteArray_ShouldReturnByteArray_WhenHexStringIsValid()
		{
			var result = "68656C6C6F2068657820776F726C64".ToByteArray();
			Assert.Equal("68656C6C6F2068657820776F726C64", result.ToHexString());
		}

		[Fact]
		public void ToByteArray_ShouldThrowException_WhenHexStringIsNotValid()
		{
			Assert.Throws(typeof(FormatException), () => "invalid hex string".ToByteArray());
		}

		[Fact]
		public void ToByteArray_ShouldReturnNull_WhenHexStringIsNullOrEmpty()
		{
			string input = null;
			var result = input.ToByteArray();
			Assert.Null(result);
		}

		[Fact]
		public void ToStream_ShouldReturnStream()
		{
			var result = "test string".ToStream();
			var reader = new StreamReader(result);

			Assert.Equal("test string", reader.ReadToEnd());
		}
	}
}
