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
	
	
        #region Date manipulation
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
		/// Returns the last day of the month for the specified date.
		/// </summary>
		/// <param name="dt">The specified date.</param>
		/// <returns>The last day of the month for the specified date.</returns>
		public static DateTime EndOfMonth(this DateTime dt)
		{
			return dt.ThisMonth().AddMonths(1).AddMilliseconds(-1);
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
            int diff = nextDay - dt.DayOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(diff).Date;
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

        #endregion

        #region Date queries
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
        /// Checks whether the specified date is tomorrow's date.
        /// </summary>
        /// <param name="compare">The specified date.</param>
        /// <returns>True when the specified date is tomorrow's date, otherwise false.</returns>
        public static bool IsTomorrow(this DateTime compare)
        {
            DateTime tomorrow = compare.AddDays(-1);

            return tomorrow.IsToday();
        }

        /// <summary>
        /// Checks whether the month in the specified date is current month.
        /// </summary>
        /// <param name="compare">The specified date.</param>
        /// <returns>True when the month in the specified date is current month, otherwise false.</returns>
        public static bool IsThisMonth(this DateTime compare)
		{
			return (compare.Month == DateTime.UtcNow.Month && compare.Year == DateTime.UtcNow.Year);
		}

        /// <summary>
        /// Returns the minimum date of SQL server.
        /// </summary>
        /// <returns>The minimum date of SQL server (1 January 1753).</returns>
        public static DateTime MinDateForSqlServer() { return new DateTime(1753, 1, 1); }

        #endregion

        #region Standards date formatting
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
				return dt.ToString($"yyyy-MM-dd{(isISO8601 ? "T" : " ")}HH:mm:ss");
			}
			else
			{
				return dt.ToString($"yyyy-MM-dd{(isISO8601 ? "T" : " ")}HH:mm");
			}
		}

		/// <summary>
		/// Converts the value of the specified date to its equivalent JavaScript Date object string representation.
		/// </summary>
		/// <param name="dt">The specified date.</param>
		/// <returns>A string that represents the value of this instance in equivalent JavaScript Date object representation.</returns>
		public static string ToJavaScript(this DateTime dt)
		{
			return $"new Date({dt.Year},{dt.Month},{dt.Day},{dt.Minute},{dt.Second},{dt.Millisecond})";
		}

		/// <summary>
		/// Convert a datatime to a unix timestamp
		/// </summary>
		public static TimeSpan ToUnixTime(this DateTime date)
		{
			return new TimeSpan(date.Ticks - 621355968000000000);
		}
        #endregion
    }
}
