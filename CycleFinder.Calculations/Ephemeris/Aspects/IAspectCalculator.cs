using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Ephemeris.Aspects
{
    public interface IAspectCalculator
    {
        public Task<IEnumerable<Aspect>> GetAspects(DateTime from, DateTime to, Planet smallerPlanet, Planet largerPlanet, AspectType aspectType);
        public Task<IEnumerable<Aspect>> GetAspects(DateTime from, DateTime to, Planet planet, AspectType aspectType);
    }
}
