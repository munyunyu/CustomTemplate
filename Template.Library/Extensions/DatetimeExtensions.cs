using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Extensions
{
    public static class DatetimeExtensions
    {
        public static (DateTime firstDateOfMonth, DateTime lastDateOfMonth) GetFirstAndLastDateOfMonth(this DateTime date)
        {
            // First date of the month at the beginning of the day
            DateTime firstDateOfMonth = new DateTime(date.Year, date.Month, 1, 0, 0, 0);

            // Last date of the month at the end of the day
            int lastDay = DateTime.DaysInMonth(date.Year, date.Month);
            DateTime lastDateOfMonth = new DateTime(date.Year, date.Month, lastDay, 23, 59, 59, 999);

            return (firstDateOfMonth, lastDateOfMonth);
        }
    }
}
