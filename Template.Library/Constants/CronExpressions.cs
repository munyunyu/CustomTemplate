using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Constants
{
    public static class CronExpressions
    {
        //Run every minute
        public const string EveryMinute = "* * * * *";

        //Run every 5 minutes
        public const string Every5Minutes = "0 */5 * * * *";

        //Run everyday at 23:30PM
        public const string DailyAt2330 = "0 30 23 * * *";

        //Run everyday at 00:30AM
        public const string DailyAt0030 = "0 30 0 * * *";

        //Run after 30 minutes daily (every 30 minutes)
        public const string Every30Minutes = "0 */30 * * * *";

        //Run on Saturday early first hour (Saturday at 00:00-00:59)
        public const string SaturdayFirstHour = "0 0 0 ? * SAT";

        //Alternative for Saturday first hour (more explicit)
        public const string SaturdayFirstHourAlternative = "0 0 0 * * SAT";

        //Additional common cron expressions

        public const string EveryHour = "0 0 * * * *";

        public const string EveryDayAtMidnight = "0 0 0 * * *";

        public const string WeekdaysAt9AM = "0 0 9 * * MON-FRI";

        public const string WeekendsAtNoon = "0 0 12 * * SAT,SUN";
    }
}
