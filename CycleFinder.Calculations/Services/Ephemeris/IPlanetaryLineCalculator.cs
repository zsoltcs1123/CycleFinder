using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Ephemeris
{
    public interface IPlanetaryLineCalculator
    {
        public Task<IEnumerable<PlanetaryLine>> GetPlanetaryLines(Planet planet, double currentPrice, DateTime from, int upperOctaves = 1, int lowerOctaves = 1);
    }
}
