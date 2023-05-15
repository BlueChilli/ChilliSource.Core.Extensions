using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliSource.Core.Extensions
{

    /// <summary>
    /// DatetimeOffset extensions.
    /// </summary>
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// Gets a System.DateTimeOffset object by adding number of working days to the specified date. (Working days exclude Saturday, Sunday, but include all holidays).
        /// </summary>
        /// <param name="date">The specified date.</param>
        /// <param name="workingDays">Number of working days.</param>
        /// <returns>A System.DateTime object.</returns>
        public static DateTimeOffset AddWorkingDays(this DateTimeOffset date, int workingDays)
        {
            int direction = workingDays < 0 ? -1 : 1;
            var newDate = date;
            while (workingDays != 0 || !newDate.DateTime.IsWorkingDay())
            {
                newDate = newDate.AddDays(direction);
                while (!newDate.DateTime.IsWorkingDay())
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
    }
}
