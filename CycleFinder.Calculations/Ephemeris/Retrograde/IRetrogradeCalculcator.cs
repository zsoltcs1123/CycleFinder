using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Ephemeris.Retrograde
{
    public interface IRetrogradeCalculcator
    {
        public Task<IEnumerable<RetrogradeCycle>> GetRetrogradeCycles(Planet planet, DateTime from);
    }
}
