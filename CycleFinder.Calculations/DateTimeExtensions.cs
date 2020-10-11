using System;

namespace CycleFinder.Calculations
{
    public static class DateTimeExtensions
    {
        public static double ToUnixTimestamp(this DateTime date)
        {
            DateTime origin = DateTime.UnixEpoch;
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
