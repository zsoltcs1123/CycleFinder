using CycleFinder.Calculations.Math;
using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Ephemeris
{
    public class PlanetaryLineCalculator : IPlanetaryLinesCalculator
    {
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;

        public PlanetaryLineCalculator(IEphemerisEntryRepository ephemerisEntryRepository)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
        }

        public async Task<IEnumerable<PlanetaryLine>> GetPlanetaryLines(Planet planet, double currentPrice, DateTime from, int upperOctaves = 1, int lowerOctaves = 1)
        {
            var ephem = (await _ephemerisEntryRepository.GetEntries(from)).ToArray();
            var priceTable = new W24Calculator(currentPrice,100);

            for (int i=0; i<ephem.Length; i++)
            {

            }

            return new List<PlanetaryLine>() { new PlanetaryLine(planet, null) };
        }
    }
}
