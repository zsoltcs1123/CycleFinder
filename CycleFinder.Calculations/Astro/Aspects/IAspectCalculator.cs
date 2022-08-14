using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Astro.Aspects
{
    public interface IAspectCalculator
    {
        public Task<IEnumerable<Aspect>> GetAspects(DateTime from, DateTime to, IEnumerable<Planet> planets, IEnumerable<AspectType> aspectTypes);
        public Task<IEnumerable<Aspect>> GetAspectsForPlanetPairs(DateTime from, DateTime to, Planet smallerPlanet, Planet largerPlanet, IEnumerable<AspectType> aspectTypes);

    }
}
