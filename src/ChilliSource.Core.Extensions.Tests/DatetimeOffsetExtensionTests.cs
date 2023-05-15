using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChilliSource.Core.Extensions.Tests
{
    public class DatetimeOffsetExtensionTests
    {
        [Fact]
        public void TestAddWorkingDays()
        {
            var testDate1 = new DateTimeOffset(2016, 8, 1, 0, 0, 0, new TimeSpan(10, 0, 0)); //Sydney

            var date1 = testDate1.AddWorkingDays(3);
            Assert.True(date1.DateTime == new DateTime(2016, 8, 4));

            var date2 = testDate1.AddWorkingDays(0);
            Assert.True(date2.DateTime == new DateTime(2016, 8, 1));

            var date3 = testDate1.AddWorkingDays(-3);
            Assert.True(date3.DateTime == new DateTime(2016, 7, 27));


            var testDate2 = new DateTimeOffset(2016, 8, 13, 0, 0, 0, new TimeSpan(12, 0, 0)); //NZ

            date1 = testDate2.AddWorkingDays(5);
            Assert.True(date1.DateTime == new DateTime(2016, 8, 19));

            date2 = testDate2.AddWorkingDays(0);
            Assert.True(date2.DateTime == new DateTime(2016, 8, 15));

            date3 = testDate2.AddWorkingDays(-5);
            Assert.True(date3.DateTime == new DateTime(2016, 8, 8));

            var testDate3 = new DateTimeOffset(2016, 8, 15, 0, 0, 0, new TimeSpan(8, 0, 0)); //Perth

            date1 = testDate3.AddWorkingDays(5);
            Assert.True(date1.DateTime == new DateTime(2016, 8, 22));

            date1 = new DateTime(2016, 8, 19).AddWorkingDays(5);
            Assert.True(date1.DateTime == new DateTime(2016, 8, 26));
        }
    }
}
