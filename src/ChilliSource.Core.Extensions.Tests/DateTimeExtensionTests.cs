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
	public class DateTimeExtensionTests
	{
	
		[Fact]
		public void RemoveTimeZone_ShouldReturnUTCDate()
		{
			var result = DateTime.Now.RemoveTimeZone();
			Assert.Equal(DateTimeKind.Unspecified, result.Kind);
		}

        [Fact]
        public void ThisMonth_ShouldReturnDayOneOfMonth()
        {
            var result = new DateTime(2001, 2, 28, 12, 33, 44).StartOfMonth();
            Assert.Equal(new DateTime(2001, 2, 1), result);
        }

        [Fact]
        public void NextMonth_ShouldReturnDayOneOfNextMonth()
        {
            var result = new DateTime(2001, 2, 28, 12, 33, 44).NextMonth();
            Assert.Equal(new DateTime(2001, 3, 1), result);
        }

        [Fact]
        public void EndOfMonth_ShouldReturnLastMillisecondOfMonth()
        {
            var result = new DateTime(2001, 2, 28, 12, 33, 44).EndOfMonth();
            Assert.Equal(new DateTime(2001, 2, 28, 23, 59, 59, 999), result);
        }

        [Fact]
        public void NextDayOfWeek_ShouldReturnTheNextDayOfTheWeek()
        {
            //Wednesday
            var result = new DateTime(2001, 2, 28, 12, 33, 44).NextDayOfWeek(DayOfWeek.Wednesday);
            Assert.Equal(new DateTime(2001, 2, 28), result);

            result = new DateTime(2001, 2, 28, 12, 33, 44).NextDayOfWeek(DayOfWeek.Friday);
            Assert.Equal(new DateTime(2001, 3, 2), result);

            result = new DateTime(2001, 2, 28, 12, 33, 44).NextDayOfWeek(DayOfWeek.Sunday);
            Assert.Equal(new DateTime(2001, 3, 4), result);

            result = new DateTime(2001, 2, 28, 12, 33, 44).NextDayOfWeek(DayOfWeek.Tuesday);
            Assert.Equal(new DateTime(2001, 3, 6), result);
        }

        [Fact]
        public void PreviousDayOfWeek_ShouldReturnThePreviousDayOfTheWeek()
        {
            //Wednesday
            var result = new DateTime(2001, 2, 28, 12, 33, 44).PreviousDayOfWeek(DayOfWeek.Wednesday);
            Assert.Equal(new DateTime(2001, 2, 28), result);

            result = new DateTime(2001, 2, 28, 12, 33, 44).PreviousDayOfWeek(DayOfWeek.Friday);
            Assert.Equal(new DateTime(2001, 2, 23), result);

            result = new DateTime(2001, 2, 28, 12, 33, 44).PreviousDayOfWeek(DayOfWeek.Sunday);
            Assert.Equal(new DateTime(2001, 2, 25), result);

            result = new DateTime(2001, 2, 28, 12, 33, 44).PreviousDayOfWeek(DayOfWeek.Tuesday);
            Assert.Equal(new DateTime(2001, 2, 27), result);
        }

        [Fact]
        public void DaysInMonth_Feburary_ShouldReturn28()
        {
            var result = new DateTime(1900, 2, 1).DaysInMonth();
            Assert.Equal(28, result);

            result = new DateTime(1999, 2, 1).DaysInMonth();
            Assert.Equal(28, result);

            result = new DateTime(2000, 2, 1).DaysInMonth();
            Assert.Equal(29, result);

            result = new DateTime(2004, 2, 1).DaysInMonth();
            Assert.Equal(29, result);
        }

        [Fact]
        public void IsToday_ShouldReturnTrueWhenToday()
        {
            var result = DateTime.UtcNow.IsTodayUtc();
            Assert.Equal(true, result);

            result = DateTime.UtcNow.AddDays(-1).IsTodayUtc();
            Assert.Equal(false, result);

            result = DateTime.UtcNow.AddDays(1).IsTodayUtc();
            Assert.Equal(false, result);
        }

        [Fact]
        public void IsYesterday_ShouldReturnTrueWhenYesterday()
        {
            var result = DateTime.UtcNow.IsYesterdayUtc();
            Assert.Equal(false, result);

            result = DateTime.UtcNow.AddDays(-1).IsYesterdayUtc();
            Assert.Equal(true, result);

            result = DateTime.UtcNow.AddDays(1).IsYesterdayUtc();
            Assert.Equal(false, result);
        }

        [Fact]
        public void IsTomorrow_ShouldReturnTrueWhenTomorrow()
        {
            var result = DateTime.UtcNow.IsTomorrowUtc();
            Assert.Equal(false, result);

            result = DateTime.UtcNow.AddDays(-1).IsTomorrowUtc();
            Assert.Equal(false, result);

            result = DateTime.UtcNow.AddDays(1).IsTomorrowUtc();
            Assert.Equal(true, result);
        }

        [Fact]
        public void IsNextMonth_ShouldReturnTrueWhenIsNextMonth()
        {
            var result = DateTime.UtcNow.IsThisMonthUtc();
            Assert.Equal(true, result);

            result = DateTime.UtcNow.AddMonths(-1).IsThisMonthUtc();
            Assert.Equal(false, result);

            result = DateTime.UtcNow.AddMonths(1).IsThisMonthUtc();
            Assert.Equal(false, result);
        }

        [Fact]
        public void ToIsoDate_ShouldReturnDateStringInIsoFormat()
        {
            var result = new DateTime(2001, 2, 28, 12, 33, 44).ToIsoDate();
            Assert.Equal("2001-02-28", result);

            result = new DateTime(2001, 2, 4, 12, 33, 44).ToIsoDate();
            Assert.Equal("2001-02-04", result);
        }

        [Fact]
        public void ToIsoDate_ShouldReturnDateTimeStringInIsoFormat()
        {
            var result = new DateTime(2001, 2, 28, 12, 33, 44).ToIsoDateTime();
            Assert.Equal("2001-02-28 12:33", result);

            result = new DateTime(2001, 2, 4, 15, 33, 44).ToIsoDateTime(isISO8601: true);
            Assert.Equal("2001-02-04T15:33", result);

            result = new DateTime(2001, 2, 4, 18, 33, 44).ToIsoDateTime(outputSeconds: true, isISO8601: true);
            Assert.Equal("2001-02-04T18:33:44", result);
        }

        [Fact]
        public void ToJavaScript_ShouldReturnDateStringInJavaScriptObjectFormat()
        {
            var result = new DateTime(2001, 2, 28, 12, 33, 44).ToJavaScript();
            Assert.Equal("new Date(2001,2,28,33,44,0)", result);
        }

        [Fact]
        public void ToUnixTime_ShouldReturnDateInUnixTime()
        {
            var result = new DateTime(2001, 2, 28, 12, 33, 44).ToUnixTime();
            Assert.Equal(983363624, result.TotalSeconds);
        }

    }
}
