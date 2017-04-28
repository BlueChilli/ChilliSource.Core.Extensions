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
using System.Text;
using System.Collections.Generic;

namespace Tests
{
	public class StringBuilderExtensionTests
    {

		[Fact]
		public void AppendFormattedLine_ShouldReturnAppendFormattedLine()
		{
            var sb = new StringBuilder();
            sb.AppendFormattedLine("{0}", 1234);

            Assert.Equal(sb.ToString(), "1234\r\n");
        }

        [Fact]
        public void AppendWhen_ShouldAppendWhenTrue()
        {
            var sb = new StringBuilder();

            sb.AppendWhen(() => true, y => y.Append("1234"));
            Assert.Equal(sb.ToString(), "1234");

            sb.AppendWhen(() => false, y => y.Append("5678"));
            Assert.Equal(sb.ToString(), "1234");
        }

        [Fact]
        public void AppendSequence_ShouldAppendSequence_UsingFunc()
        {
            var sb = new StringBuilder();

            sb.AppendSequence(new List<int> { 1, 2, 3, 4 }, (y, z) => y.Append(z));
            Assert.Equal(sb.ToString(), "1234");
        }

    }
}
