﻿#region License

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
        /// Returns date with day set to the first of the month for the specified date.
        /// </summary>
        /// <param name="date">The original date.</param>
        /// <returns>Date with day set to the first of the month.</returns>
        public static DateTime StartOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}

        /// <summary>
        /// Returns date with day set to the first day of the following month for the specified date.
        /// </summary>
        /// <param name="date">The original date.</param>
        /// <returns>date with day set to the first day of the following month.</returns>
        public static DateTime NextMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1).AddMonths(1);
		}

        /// <summary>
        /// Returns date representing the last day of the month for the specified date.
        /// </summary>
        /// <param name="date">The original date.</param>
        /// <returns>date representing the last day of the month.</returns>
        public static DateTime EndOfMonth(this DateTime date)
		{
			return date.StartOfMonth().AddMonths(1).AddMilliseconds(-1);
		}

        /// <summary>
        /// Return the closest date matching the <paramref name="nextWeekDay"/>.
        /// If the day of week of the specified <paramref name="date"/> matches the <paramref name="nextWeekDay"/>, the same date is returned.
        /// </summary>
        /// <param name="date">The specified date.</param>
        /// <param name="nextWeekDay">The day of week to select</param>
        /// <returns>A System.DateTime object.</returns>
        public static DateTime NextDayOfWeek(this DateTime date, DayOfWeek nextWeekDay = DayOfWeek.Sunday)
        {
            int diff = nextWeekDay - date.DayOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return date.AddDays(diff).Date;
        }

        /// <summary>
        /// Return the closest previous date matching the <paramref name="previousWeekDay"/>.
        /// If the day of week of the specified <paramref name="date"/> matches the <paramref name="previousWeekDay"/>, the same date is returned.
        /// </summary>
        /// <param name="date">The specified date.</param>
        /// <param name="previousWeekDay">The day of week to select</param>
        /// <returns>A System.DateTime object.</returns>
        public static DateTime PreviousDayOfWeek(this DateTime date, DayOfWeek previousWeekDay = DayOfWeek.Sunday)
        {
            int diff = date.DayOfWeek - previousWeekDay;
            if (diff < 0)
            {
                diff += 7;
            }
            return date.AddDays(-diff).Date;
        }

        /// <summary>
        /// Returns a DateTime instance based on <paramref name="dateTime"/> with its DateTimeKind set to "Unspecified", thereby removing the time zone component
        /// </summary>
        /// <returns>Timezone independent DateTime instance</returns>
        /// <param name="dateTime">Original DateTime instance</param>
        public static DateTime RemoveTimeZone(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
        }

        /// <summary>
        /// Gets a System.DateTime object by adding number of working days to the specified date. (Working days exclude Saturday, Sunday, but include all holidays).
        /// </summary>
        /// <param name="date">The specified date.</param>
        /// <param name="workingDays">Number of working days.</param>
        /// <returns>A System.DateTime object.</returns>
        public static DateTime AddWorkingDays(this DateTime date, int workingDays)
        {
            int direction = workingDays < 0 ? -1 : 1;
            DateTime newDate = date;
            while (workingDays != 0 || !IsWorkingDay(newDate))
            {
                newDate = newDate.AddDays(direction);
                while (!IsWorkingDay(newDate))
                {
                    newDate = newDate.AddDays(direction);
                };
                if (workingDays != 0)
                {
                    workingDays -= direction;
                }
            }
            return newDate;
        }
        #endregion

        #region Date queries
        /// <summary>
        /// Returns the number of days in the month of the specified <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The original date.</param>
        /// <returns>The number of days in the month of the specified date.</returns>
        public static int DaysInMonth(this DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }

        /// <summary>
        /// Checks whether the specified <paramref name="comparisonDate"/> is today's date.
        /// </summary>
        /// <param name="comparisonDate">The date to compare.</param>
        /// <returns>True when the specified date is today's date, otherwise false.</returns>
        public static bool IsTodayUtc(this DateTime comparisonDate)
		{
            var now = DateTime.UtcNow;
            if (now.DayOfYear == comparisonDate.DayOfYear && now.Year == comparisonDate.Year)
            {
                return true;
            }
			return false;
		}

		/// <summary>
		/// Checks whether the specified <paramref name="comparisonDate"/> is yesterday's date.
		/// </summary>
		/// <param name="comparisonDate">The date to compare.</param>
		/// <returns>True when the specified date is yesterday's date, otherwise false.</returns>
		public static bool IsYesterdayUtc(this DateTime comparisonDate)
		{
			DateTime yesterday = comparisonDate.AddDays(1);

			return yesterday.IsTodayUtc();
		}

        /// <summary>
        /// Checks whether the specified <paramref name="comparisonDate"/> is tomorrow's date.
        /// </summary>
        /// <param name="comparisonDate">The date to compare.</param>
        /// <returns>True when the specified date is tomorrow's date, otherwise false.</returns>
        public static bool IsTomorrowUtc(this DateTime comparisonDate)
        {
            DateTime tomorrow = comparisonDate.AddDays(-1);

            return tomorrow.IsTodayUtc();
        }

        /// <summary>
        /// Checks whether the month of the <paramref name="comparisonDate"/> is the current month.
        /// </summary>
        /// <param name="comparisonDate">The date to compare.</param>
        /// <returns>True when the month of the specified date is the current month, otherwise false.</returns>
        public static bool IsThisMonthUtc(this DateTime comparisonDate)
		{
            var now = DateTime.UtcNow;
            return (comparisonDate.Month == now.Month && comparisonDate.Year == now.Year);
		}

        /// <summary>
        /// Returns true if date is a working day (Working days exclude Saturday, Sunday but include all holidays).
        /// </summary>
        /// <param name="date">The specified date.</param>
        /// <returns></returns>
        public static bool IsWorkingDay(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;

        }

        /// <summary>
        /// Returns the minimum date that can be stored in SQL Server.
        /// </summary>
        /// <returns>The minimum date that can be stored in SQL server (1 January 1753).</returns>
        public static DateTime MinDateForSqlServer() { return new DateTime(1753, 1, 1); }

        #endregion

        #region Standards date formatting
        /// <summary>
        /// Converts the value of the specified date to its equivalent string representation using the ISO format with date only (yyyy-MM-dd).
        /// </summary>
        /// <param name="date">The specified date.</param>
        /// <returns>A value of string in ISO format with date only.</returns>
        public static string ToIsoDate(this DateTime date)
		{
			return date.ToString("yyyy-MM-dd");
		}

		/// <summary>
		/// Converts the value of the specified date to its equivalent string representation using the ISO format with date and time (yyyy-MM-dd HH:mm).
		/// </summary>
		/// <param name="date">The specified date.</param>
		/// <param name="outputSeconds">Output seconds after minutes</param>
		/// <param name="isISO8601">Output T as separator between date and time</param>
		/// <returns>A value of string in ISO format with date and time.</returns>
		public static string ToIsoDateTime(this DateTime date, bool outputSeconds = false, bool isISO8601 = false)
		{
			if (outputSeconds)
			{
				return date.ToString($"yyyy-MM-dd{(isISO8601 ? "T" : " ")}HH:mm:ss");
			}
			else
			{
				return date.ToString($"yyyy-MM-dd{(isISO8601 ? "T" : " ")}HH:mm");
			}
		}

		/// <summary>
		/// Converts the value of the specified date to its equivalent JavaScript Date object string representation.
		/// </summary>
		/// <param name="date">The specified date.</param>
		/// <returns>A string that represents the value of this instance in equivalent JavaScript Date object representation.</returns>
		public static string ToJavaScript(this DateTime date)
		{
			return $"new Date({date.Year},{date.Month},{date.Day},{date.Minute},{date.Second},{date.Millisecond})";
		}

		/// <summary>
		/// Convert the specified date to a Unix timestamp
		/// </summary>
		public static TimeSpan ToUnixTime(this DateTime date)
		{
			return new TimeSpan(date.Ticks - 621355968000000000);
		}
        #endregion
    }
}
