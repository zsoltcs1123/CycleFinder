using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Ephemeris.Aspects
{
    public interface IAspectCalculator
    {
        public Task<IEnumerable<Aspect>> GetAspects(DateTime startTime, Planet planet1, Planet planet2, AspectType aspectType);
    }
}
