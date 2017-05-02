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
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
        public void FormatIfNotNull_ShouldFormatStringWhenParamHasValue()
        {
            int? param = null;
            var result = "A{0}C".FormatIfNotNull(param);
            Assert.Equal("", result);
            param = 2;
            result = "A{0}C".FormatIfNotNull(param);
            Assert.Equal("A2C", result);

            string param2 = null;
            result = "A{0}C".FormatIfNotNull(param2);
            Assert.Equal("", result);
            param2 = "B";
            result = "A{0}C".FormatIfNotNull(param2);
            Assert.Equal("ABC", result);
        }

        [Fact]
        public void FormatWithMask_ShouldWork()
        {
            var value = "12345678";
            Assert.Equal("XXXX5678", value.FormatWithMask("XXXX*"));
            Assert.Equal("1234XXXX", value.FormatWithMask("*XXXX"));
            Assert.Equal("XXX45XXX", value.FormatWithMask("XXX*XXX"));
            Assert.Equal("XXXX", value.FormatWithMask("XXXX"));
            Assert.Equal("XXXXXXXX", value.FormatWithMask("X"));

            var smallValue = "12";
            Assert.Equal("XXXX", smallValue.FormatWithMask("XXXX*"));
            Assert.Equal("XXXX", smallValue.FormatWithMask("*XXXX"));
            Assert.Equal("XXXXXX", smallValue.FormatWithMask("XXX*XXX"));
            Assert.Equal("XXXX", smallValue.FormatWithMask("XXXX"));
            Assert.Equal("XX", smallValue.FormatWithMask("X"));
        }

        [Fact]
        public void TransformWith_Dictionary_ShouldTransformStringPlaceholders_ByKey()
        {
            var dictionary = new Dictionary<string, object> { { "Id", 4 }, { "Name", "Sam" }, { "Age", 21 } };
            var result = "{Name} is {Age}".TransformWith(dictionary);
            Assert.Equal("Sam is 21", result);

            var result2 = "{NotPresent}{Name} is {Age}".TransformWith(dictionary, removeUnused: true);
            Assert.Equal("Sam is 21", result2);

            var result3 = "{Name} is {Age}".TransformWith(new { Name = "Bob", Age = 21 });
            Assert.Equal("Bob is 21", result3);
        }

        [Fact]
        public void Repeat_ReturnsXRepeatedYTimes()
        {
            Assert.Equal("XXX", "X".Repeat(3));
            Assert.Equal("", "X".Repeat(0));
            Assert.Equal("", "".Repeat(10));
            Assert.Equal("1212121212", "12".Repeat(5));
        }

        #endregion

        #region Convert To and From

        public class JsonTest
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }
            public EnumExtensionsTests.TestEnum Status { get; set; }
            public DateTime DateCreated { get; set; }
        }

        [Fact]
        public void FromJson_ShouldReturnTypedObject_RepresentativeOfJson()
        {
            var test1 = "{\"id\":1,\"name\":\"Bob\",\"isActive\":false,\"status\":\"Test2\",\"dateCreated\":\"2001-10-01T00:00:00\"}";
            var result = test1.FromJson<JsonTest>();

            Assert.Equal(1, result.Id);
            Assert.Equal("Bob", result.Name);
            Assert.Equal(false, result.IsActive);
            Assert.Equal(EnumExtensionsTests.TestEnum.Test2, result.Status);
            Assert.Equal(new DateTime(2001, 10, 1), result.DateCreated);

            var test2 = "{\"Id\":1,\"Name\":\"Bob\",\"IsActive\":false,\"Status\":2,\"DateCreated\":\"2001-10-01\"}";
            result = test2.FromJson<JsonTest>();
            Assert.Equal("Bob", result.Name);
            Assert.Equal(false, result.IsActive);
            Assert.Equal(EnumExtensionsTests.TestEnum.Test2, result.Status);
            Assert.Equal(new DateTime(2001, 10, 1), result.DateCreated);

        }

        [Fact]
		public void DefaultTo_ShouldReturnDefault_WhenValueIsNullOrEmpty()
		{
			var result = "test".DefaultTo("");
			Assert.Equal("test", result);
            var result2 = "".DefaultTo("");
            Assert.Equal(string.Empty, result2);
            var result3 = "".DefaultTo("replacement");
            Assert.Equal("replacement", result3);
            var result4 = "".DefaultTo(null, "", "not null", "also not null");
            Assert.Equal("not null", result4);
        }

        [Fact]
        public void GetIndependentHashCode_ShouldHashcode_ForValue()
        {
            string nullString = null;
            string emptyString = String.Empty;
            string s = "1234567890";
            string longstring = "yikes".Repeat(10000);

            var result = nullString.GetIndependentHashCode();
            Assert.Equal(null, result);
            var result2 = emptyString.GetIndependentHashCode();
            Assert.Equal(23, result2);
            var result3 = s.GetIndependentHashCode();
            Assert.Equal(-434371086, result3);
            var result4 = longstring.GetIndependentHashCode();
            Assert.Equal(-2116544745, result4);
        }


        [Fact]
		public void ToByteArrayFromHex_ShouldReturnByteArray_WhenHexStringIsValid()
        { 
		
			var result = "68656C6C6F2068657820776F726C64".ToByteArrayFromHex();
			Assert.Equal("68656C6C6F2068657820776F726C64", result.ToHexString());
		}

		[Fact]
		public void ToByteArrayFromHex_ShouldThrowException_WhenHexStringIsNotValid()
		{
			Assert.Throws(typeof(FormatException), () => "invalid hex string".ToByteArrayFromHex());
		}

		[Fact]
		public void ToByteArrayFromHex_ShouldReturnNull_WhenHexStringIsNullOrEmpty()
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

        [Fact]
        public void ToByteArray_ToString_ShouldReturnOriginalValue()
        {
            var result = "test string".ToByteArray().ToString(new UTF8Encoding());
            Assert.Equal("test string", result);
        }

        [Fact]
        public void ToEnumerable_GivenDelimitedListOfValues_ReturnListOfValues()
        {
            var result = "1,2,3,4,5,6,7,8,9".ToEnumerable<int>();
            Assert.Contains(5, result);
            Assert.Equal(9, result.Count());

            var result2 = "1.0|2.5|3.6|".ToEnumerable<decimal>("|");
            Assert.Contains(2.5M, result2);
            Assert.Contains(0.0M, result2);
            Assert.Equal(4, result2.Count());
        }

        #endregion

        #region Helpers

        [Fact]
        public void Contains_ReturnsWhenValueContainsX_WhenStringComparisonIsPassed()
        {
            var value = "JIM, james, JoHn";
            Assert.True(value.Contains("james", StringComparison.Ordinal));
            Assert.False(value.Contains("jim", StringComparison.Ordinal));
            Assert.True(value.Contains("JIM", StringComparison.Ordinal));
            Assert.True(value.Contains("jim", StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
