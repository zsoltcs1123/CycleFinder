using System;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Services.Ephemeris
{
    public interface ILunarCalendar
    {
        public IEnumerable<DateTime> GetFibSquareMoons(DateTime start);
    }
}
