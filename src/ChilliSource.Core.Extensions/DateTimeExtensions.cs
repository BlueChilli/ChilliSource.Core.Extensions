#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;

namespace ChilliSource.Core.Extensions
{
	/// <summary>
	/// Date time extensions.
	/// </summary>
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Returns a new DateTime instance that has the date of <paramref name="dateTime"/>
		/// and the time as specified by <paramref name="hours"/>, <paramref name="minutes"/>, and <paramref name="seconds"/>
		/// </summary>
		/// <returns>The combined DateTime</returns>
		/// <param name="dateTime">Original date</param>
		/// <param name="hours">New hours value</param>
		/// <param name="minutes">New minutes value</param>
		/// <param name="seconds">New seconds value</param>
		public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds = 0)
		{
			return new DateTime(
			dateTime.Year,
			dateTime.Month,
			dateTime.Day,
			hours,
			minutes,
			seconds,
			0,
			dateTime.Kind);
		}

		/// <summary>
		/// Returns a DateTime instance based on <paramref name="dateTime"/> with its DateTimeKind set to Unspecified
		/// </summary>
		/// <returns>Timezone independent DateTime instance</returns>
		/// <param name="dateTime">Original DateTime instance</param>
		public static DateTime RemoveTimeZone(this DateTime dateTime)
		{
			return DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
		}

		/// <summary>
		/// Returns date which set the day to the first of the month for the specified date.
		/// </summary>
		/// <param name="dt">The specified date and time.</param>
		/// <returns>Date which set the day to the first of the month for the specified date.</returns>
		public static DateTime ThisMonth(this DateTime dt)
		{
			return new DateTime(dt.Year, dt.Month, 1);
		}

		/// <summary>
		/// Returns date which set to first day of next month for the specified date.
		/// </summary>
		/// <param name="dt">The specified date and time.</param>
		/// <returns>Date which set to first day of next month for the specified date.</returns>
		public static DateTime NextMonth(this DateTime dt)
		{
			return new DateTime(dt.Year, dt.Month, 1).AddMonths(1);
		}

		/// <summary>
		/// Returns the number of days in month for the specified date.
		/// </summary>
		/// <param name="dt">The specified date.</param>
		/// <returns>The number of days in month for the specified date.</returns>
		public static int DaysInMonth(this DateTime dt)
		{
			return DateTime.DaysInMonth(dt.Year, dt.Month);
		}

		/// <summary>
		/// Returns the last day of the month for the specified date.
		/// </summary>
		/// <param name="dt">The specified date.</param>
		/// <returns>The last day of the month for the specified date.</returns>
		public static DateTime EndOfMonth(this DateTime dt)
		{
			return dt.ThisMonth().AddMonths(1).AddMilliseconds(-1);
		}

		/// <summary>
		/// Checks whether the specified date is today's date.
		/// </summary>
		/// <param name="compare">The specified date.</param>
		/// <returns>True when the specified date is today's date, otherwise false.</returns>
		public static bool IsToday(this DateTime compare)
		{
			if (DateTime.UtcNow.DayOfYear == compare.DayOfYear && DateTime.UtcNow.Year == compare.Year)
				return true;
			return false;
		}

		/// <summary>
		/// Checks whether the specified date is yesterday's date.
		/// </summary>
		/// <param name="compare">The specified date.</param>
		/// <returns>True when the specified date is yesterday's date, otherwise false.</returns>
		public static bool IsYesterday(this DateTime compare)
		{
			DateTime yesterday = compare.AddDays(1);

			return yesterday.IsToday();
		}

		/// <summary>
		/// Checks whether the month in the specified date is current month.
		/// </summary>
		/// <param name="compare">The specified date.</param>
		/// <returns>True when the month in the specified date is current month, otherwise false.</returns>
		public static bool IsCurrentMonth(this DateTime compare)
		{
			return (compare.Month == DateTime.UtcNow.Month && compare.Year == DateTime.UtcNow.Year);
		}

		/// <summary>
		/// Converts the value of the specified date to its equivalent string representation using the ISO format with date only (yyyy-MM-dd).
		/// </summary>
		/// <param name="dt">The specified date.</param>
		/// <returns>A value of string in ISO format with date only.</returns>
		public static string ToIsoDate(this DateTime dt)
		{
			return dt.ToString("yyyy-MM-dd");
		}

		/// <summary>
		/// Converts the value of the specified date to its equivalent string representation using the ISO format with date and time (yyyy-MM-dd HH:mm).
		/// </summary>
		/// <param name="dt">The specified date.</param>
		/// <param name="outputSeconds">Output seconds after minutes</param>
		/// <param name="isISO8601">Output T as separator between date and time</param>
		/// <returns>A value of string in ISO format with date and time.</returns>
		public static string ToIsoDateTime(this DateTime dt, bool outputSeconds = false, bool isISO8601 = false)
		{
			if (outputSeconds)
			{
				return dt.ToString("yyyy-MM-dd{0}HH:mm:ss".FormatWith(isISO8601 ? "T" : " "));
			}
			else
			{
				return dt.ToString("yyyy-MM-dd{0}HH:mm".FormatWith(isISO8601 ? "T" : " "));
			}
		}

		/// <summary>
		/// Returns the minimum date of SQL server.
		/// </summary>
		/// <param name="notUsed">The specified date (not used).</param>
		/// <returns>The minimum date of SQL server (1 January 1753).</returns>
		public static DateTime MinDateForSqlServer(this DateTime notUsed) { return MinDateForSqlServer(); }

		/// <summary>
		/// Returns the minimum date of SQL server.
		/// </summary>
		/// <returns>The minimum date of SQL server (1 January 1753).</returns>
		public static DateTime MinDateForSqlServer() { return new DateTime(1753, 1, 1); }

		/// <summary>
		/// Converts the value of the specified date to its equivalent JavaScript Date object string representation.
		/// </summary>
		/// <param name="dt">The specified date.</param>
		/// <returns>A string that represents the value of this instance in equivalent JavaScript Date object representation.</returns>
		public static string ToJavaScript(this DateTime dt)
		{
			return "new Date({0},{1},{2},{3},{4},{5}".FormatWith(dt.Year, dt.Month, dt.Day, dt.Minute, dt.Second, dt.Millisecond);
		}

		/// <summary>
		/// Convert a datatime to a unix timestamp
		/// </summary>
		public static TimeSpan ToUnixTime(this DateTime date)
		{
			return new TimeSpan(date.Ticks - 621355968000000000);
		}


		/// <summary>
		/// Return the closest future date in which the day of week matches the input.
		/// If the current datetime day of week matches the input, the same date is returned.
		/// </summary>
		/// <param name="dt">The specified date.</param>
		/// <param name="nextDay">The day of week to set</param>
		/// <returns>A System.DateTime object.</returns>
		public static DateTime NextDayOfWeek(this DateTime dt, DayOfWeek nextDay = DayOfWeek.Sunday)
		{
			int diff = dt.DayOfWeek - nextDay;
			if (diff < 0)
			{
				diff += 7;
			}
			return dt.AddDays(-1 * diff).Date;
		}
	}
}
