#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using Xunit;
using ChilliSource.Core.Extensions;
using System.IO;

namespace Tests
{
	public class StringExtensionTests
    { 

        #region Sanitise / Remove / Replace
        [Fact]
        public void ToFilename_ShouldReturnValidFilename()
        {
            var result = "myfile.txt".ToFilename();
            Assert.Equal("myfile.txt", result);

            var result2 = "|my file\r\n.txt.docx".ToFilename();
            Assert.Equal("my_filetxt.docx", result2);
        }

        [Fact]
        public void ToCssClass_ShouldReturnValidCssClass()
        {
            var result = "dark123".ToCssClass();
            Assert.Equal("dark123", result);

            var result2 = "9DARK 1_2_3&*()#%$".ToCssClass();
            Assert.Equal("_9dark-1-2-3", result2);
        }

        [Fact]
        public void ToSeoUrl_ShouldReturnValidUrlParameter()
        {
            var result = "dark123".ToSeoUrl();
            Assert.Equal("dark123", result);

            var result2 = "DARK & stormy 1*2(3".ToSeoUrl();
            Assert.Equal("dark-and-stormy-123", result2);
        }

        [Fact]
        public void ToAlphaNumeric_ShouldReturnValidAlphaNumeric()
        {
            var result = "dark123".ToAlphaNumeric();
            Assert.Equal("dark123", result);

            var result2 = "DARK & stormy 1*2(3".ToAlphaNumeric();
            Assert.Equal("DARKstormy123", result2);
        }

        [Fact]
        public void ToAlpha_ShouldReturnValidAlpha()
        {
            var result = "dark".ToAlpha();
            Assert.Equal("dark", result);

            var result2 = "DARK & stormy 1*2(3".ToAlpha();
            Assert.Equal("DARKstormy", result2);
        }

        [Fact]
        public void ToNumeric_ShouldReturnValidNumeric()
        {
            var result = "123".ToNumeric();
            Assert.Equal("123", result);

            var result2 = "DARK & stormy 1*2(3".ToNumeric();
            Assert.Equal("123", result2);
        }

        [Fact]
        public void RemoveSpaces_ShouldReturnEmptyString_WhenGivenNull()
        {
            string input = null;
            var result = input.RemoveSpaces();
            Assert.Equal("", result);
        }

        [Fact]
        public void RemoveSpaces_ShouldRemoveSpaces_WhenGivenStringWithSpaces()
        {
            var result = "test1 test2 test3".RemoveSpaces();
            Assert.Equal("test1test2test3", result);

            var result2 = "test1test2test3".RemoveSpaces();
            Assert.Equal("test1test2test3", result2);
        }

        [Fact]
        public void RemoveExcessWhiteSpaces_ShouldRemoveExcessSpaces_WhenGivenStringWithExcessSpaces()
        {
            var result = "test1 test2  test3".RemoveExcessWhiteSpaces();
            Assert.Equal("test1 test2 test3", result);

            var result2 = "test1test2test3".RemoveExcessWhiteSpaces();
            Assert.Equal("test1test2test3", result2);
        }

        [Fact]
        public void Replace_ShouldReplaceX_WhenGivenStringHasX()
        {
            var result = "test1 test2 test3".Replace("TEST2", "test4", StringComparison.OrdinalIgnoreCase);
            Assert.Equal("test1 test4 test3", result);

            var result2 = "test1 test2 test3".Replace("TEST4", "test5", StringComparison.OrdinalIgnoreCase);
            Assert.Equal("test1 test2 test3", result2);
        }

        [Fact]
        public void ReplaceAny_ShouldReplaceX_WhenGivenStringHasX()
        {
            var result = "test1 test2 test3".ReplaceAny("123", "X");
            Assert.Equal("testX testX testX", result);

            var result2 = "test1 test2 test3".ReplaceAny("XYZ", "ABC");
            Assert.Equal("test1 test2 test3", result2);
        }

        #endregion

        #region Trim

        [Fact]
        public void TrimStart_ShouldTrimFromStartOfString_WhenPresent()
        {
            var result = "www.google.com".TrimStart("www.");
            Assert.Equal("google.com", result);

            var result2 = "google.com".TrimStart("www.");
            Assert.Equal("google.com", result2);
        }

        [Fact]
        public void TrimEnd_ShouldTrimFromEndOfString_WhenPresent()
        {
            var result = "www.google.com".TrimEnd(".com");
            Assert.Equal("www.google", result);

            var result2 = "www.google.com".TrimEnd("www.");
            Assert.Equal("www.google.com", result2);
        }

        [Fact]
        public void TrimBetween_ShouldTrimFromMiddleOfString_WhenPresent()
        {
            var result = "www(google)com".TrimBetween('(', ')');
            Assert.Equal("wwwcom", result);

            var result3 = "www.google.com".TrimBetween('.', '.');
            Assert.Equal("wwwcom", result3);

            var result2 = "www.google.com".TrimBetween('x', 'z');
            Assert.Equal("www.google.com", result2);
        }

        #endregion

        #region Format /Transform
        [Fact]
        public void FormatWith_ShouldFormatString()
        {
            var result = "test{0} test{1} test{2}".FormatWith(1, "A", 2.3);
            Assert.Equal("test1 testA test2.3", result);
        }
        #endregion

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
		public void ToByteArray_ShouldReturnByteArray_WhenHexStringIsValid()
		{
			var result = "68656C6C6F2068657820776F726C64".ToByteArrayFromHex();
			Assert.Equal("68656C6C6F2068657820776F726C64", result.ToHexString());
		}

		[Fact]
		public void ToByteArray_ShouldThrowException_WhenHexStringIsNotValid()
		{
			Assert.Throws(typeof(FormatException), () => "invalid hex string".ToByteArrayFromHex());
		}

		[Fact]
		public void ToByteArray_ShouldReturnNull_WhenHexStringIsNullOrEmpty()
		{
			string input = null;
			var result = input.ToByteArrayFromHex();
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
