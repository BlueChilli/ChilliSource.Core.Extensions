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


    }
}
