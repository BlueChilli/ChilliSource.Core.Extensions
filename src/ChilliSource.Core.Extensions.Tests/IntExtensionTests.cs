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

namespace Tests
{
    public class IntExtensionTests
    {

        [Fact]
        public void Ordinal_ShouldReturnOrdinal_WhenGivenInteger()
        {
            int input = 1;
            var result = input.Ordinal();
            Assert.Equal("1st", result);

            input = 2;
            result = input.Ordinal();
            Assert.Equal("2nd", result);

            input = 3;
            result = input.Ordinal();
            Assert.Equal("3rd", result);

            input = 10;
            result = input.Ordinal();
            Assert.Equal("10th", result);

            input = 21;
            result = input.Ordinal();
            Assert.Equal("21st", result);

            input = 0;
            result = input.Ordinal();
            Assert.Equal("0", result);
        }

        [Fact]
        public void ToWords_ShouldReturnWords_WhenGivenInteger()
        {
            int input = 999;
            var result = input.ToWords();
            Assert.Equal("nine hundred and ninety-nine", result);

            input = 21;
            result = input.ToWords();
            Assert.Equal("twenty-one", result);

            input = 0;
            result = input.ToWords();
            Assert.Equal("zero", result);

            input = -1234;
            result = input.ToWords();
            Assert.Equal("minus one thousand two hundred and thirty-four", result);

            input = 23000000;
            result = input.ToWords();
            Assert.Equal("twenty-three million", result);

        }

    }
}
