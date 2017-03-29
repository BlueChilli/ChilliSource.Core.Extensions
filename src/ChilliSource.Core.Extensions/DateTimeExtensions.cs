#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;

namespace ChilliSource.Core
{
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
	}
}
