using CycleFinder.Calculations.Math;
using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Ephemeris
{
    public class PlanetaryLinesCalculator : IPlanetaryLinesCalculator
    {
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;

        public PlanetaryLinesCalculator(IEphemerisEntryRepository ephemerisEntryRepository)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
        }

        public async Task<IEnumerable<PlanetaryLine>> GetPlanetaryLines(Planet planet, double currentPrice, DateTime from, DateTime to, int upperOctaves = 1, int lowerOctaves = 1)
        {
            var ephem = (await _ephemerisEntryRepository.GetEntries(from)).Where(entry => entry.Time <= to).ToArray();

            var ret = new List<PlanetaryLine>();

            //TODO shorter cycles(moon, sun, mercury, venus, mars don't display enough data due to steepness
            //TODO make this work for all price ranges and increments

            foreach (var price in new PriceOctaveCalculator(currentPrice, 100, 8, 0).Octaves)
            {
                var w24calc = new W24Calculator(price, 100);

                var prices = w24calc.ConvertLongitudesToPrices(ephem.Select(entry => entry.GetCoordinatesByPlanet(planet).Longitude).ToArray());

                var values = new List<(DateTime, double)>();
                for (int i = 0; i < ephem.Length; i++)
                {
                    if (prices[i].HasValue)
                    {
                        values.Add((ephem[i].Time, prices[i].Value));
                    }
                }

                ret.Add(new PlanetaryLine(planet, values));
            }
            return ret;
        }
    }
}
