using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Ephemeris.Retrograde
{
    public interface IRetrogradeCalculcator
    {
        public Task<RetrogradeCycles> GetRetrogradeCycles(Planet planet, DateTime from);
    }
}
