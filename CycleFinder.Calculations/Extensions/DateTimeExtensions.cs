using System;

namespace CycleFinder.Calculations.Extensions
{
    public static class DateTimeExtensions
    {
        public static double ToUnixTimestamp(this DateTime date)
        {
            DateTime origin = DateTime.UnixEpoch;
            TimeSpan diff = date.ToUniversalTime() - origin;
            return System.Math.Floor(diff.TotalSeconds);
        }

        public static DateTime FromUnixTimeStamp(long timeStamp)
        {
            return DateTime.UnixEpoch.AddSeconds(timeStamp);
        }
    }
}
