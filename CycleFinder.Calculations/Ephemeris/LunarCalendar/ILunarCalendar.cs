using System;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Ephemeris.LunarCalendar
{
    public interface ILunarCalendar
    {
        public IEnumerable<DateTime> GetFibSquareMoons(DateTime start);
    }
}
