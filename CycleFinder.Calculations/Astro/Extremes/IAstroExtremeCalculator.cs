using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Astro.Extremes
{
    public interface IAstroExtremeCalculator
    {
        public Task<IEnumerable<Extreme>> GetExtremes(DateTime from, DateTime to, IEnumerable<Planet> planets, IEnumerable<ExtremeType> extremeTypes);
    }
}
