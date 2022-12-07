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
	public class UriExtensionTests
    {

		[Fact]
		public void AsFriendlyName_ShouldReturnHostOfUri()
		{
            var uri = new Uri("https://www.mysite.com/something/1?else=true");

            var result = uri.AsFriendlyName();
            Assert.Equal("mysite.com", result);
        }

        [Fact]
        public void Root_ShouldReturnRootOfUri()
        {
            var uri = new Uri("https://www.mysite.com/something/1?else=true");

            var result = uri.Root();
            Assert.Equal("https://www.mysite.com", result);
        }

        [Fact]
        public void Base_ShouldReturnBaseOfUri()
        {
            var uri = new Uri("https://www.mysite.com/something/1?else=true");

            var result = uri.Base();
            Assert.Equal("https://www.mysite.com/something/1", result);
        }

        [Fact]
        public void Append_ShouldReturnPathsAppended()
        {
            var uri1 = new Uri("https://www.mysite.com/something/");
            var result1 = uri1.Append("1", "2", "/3/");
            Assert.Equal("https://www.mysite.com/something/1/2/3/", result1.ToString());

            var uri2 = new Uri("https://www.mysite.com/something/1?else=true");
            var result2 = uri2.Append("2", "/3/");
            Assert.Equal("https://www.mysite.com/something/1/2/3/?else=true", result2.ToString());
        }

        [Fact]
        public void WithTrailingSlash_ShouldReturnWithTrailingSlash()
        {
            var uri = new Uri("https://www.mysite.com/something/");
            var result = uri.WithTrailingSlash().ToString();
            Assert.Equal("https://www.mysite.com/something/", result);

            uri = new Uri("https://www.mysite.com/something");
            result = uri.WithTrailingSlash().ToString();
            Assert.Equal("https://www.mysite.com/something/", result);

            uri = new Uri("https://www.mysite.com/something/1?else=true");
            result = uri.WithTrailingSlash().ToString();
            Assert.Equal("https://www.mysite.com/something/1/?else=true", result);
        }
    }
}
