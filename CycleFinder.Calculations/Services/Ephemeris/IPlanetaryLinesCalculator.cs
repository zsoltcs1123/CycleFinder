using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Ephemeris
{
    public interface IPlanetaryLinesCalculator
    {
        public Task<IEnumerable<PlanetaryLine>> GetPlanetaryLines(Planet planet, double currentPrice, DateTime from, DateTime to, int upperOctaves = 1, int lowerOctaves = 1);
        public Task<IEnumerable<W24Crossing>> GetW24Crossings(Planet planet, DateTime from);
    }
}
