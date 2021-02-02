using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Ephemeris
{
    public interface IPlanetaryLinesService
    {
        public Task<IEnumerable<PlanetaryLine>> GetPlanetaryLines(
            Planet planet, 
            double currentPrice, 
            DateTime from, 
            DateTime to, 
            TimeFrame timeFrame,
            double increment,
            int upperOctaves = 1, 
            int lowerOctaves = 1);

        public Task<IEnumerable<HarmonicCrossing>> GetW24Crossings(Planet planet, DateTime from);
        public Task<IEnumerable<HarmonicCrossing>> GetSQ9Crossings(Planet planet, DateTime from);
    }
}
