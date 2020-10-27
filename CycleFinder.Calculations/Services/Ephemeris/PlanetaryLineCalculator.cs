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

            var ret = new List<PlanetaryLine>();

            foreach (var price in new PriceOctaveCalculator(currentPrice, 100, 8, 0).Octaves)
            {
                var w24calc = new W24Calculator(price, 100);

                var prices = w24calc.ConvertLongitudesToPrices(ephem.Select(entry => entry.GetCoordinatesByPlanet(planet).Longitude).ToArray());

                var values = new List<(DateTime, double)>();
                for (int i = 0; i < ephem.Length; i++)
                {
                    values.Add((ephem[i].Time, prices[i]));
                }

                ret.Add(new PlanetaryLine(planet, values));
            }
            return ret;
        }
    }
}
