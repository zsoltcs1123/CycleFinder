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
        private readonly IW24Calculator _w24Calculator;

        public PlanetaryLinesCalculator(IEphemerisEntryRepository ephemerisEntryRepository, IW24Calculator w24Calculator)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
            _w24Calculator = w24Calculator;
        }

        //TODO this is for slow planets
        public async Task<IEnumerable<PlanetaryLine>> GetPlanetaryLines(Planet planet, double currentPrice, DateTime from, DateTime to, int upperOctaves = 1, int lowerOctaves = 1)
        {
            var ephem = (await _ephemerisEntryRepository.GetEntries(from)).Where(entry => entry.Time <= to).ToArray();

            var ret = new List<PlanetaryLine>();

            //TODO shorter cycles(moon, sun, mercury, venus, mars don't display enough data due to steepness
            //TODO make this work for all price ranges and increments
            double maxPrice = currentPrice * 3;
            foreach (var price in new PriceOctaveCalculator(currentPrice, 100, 8, 0).Octaves)
            {
                var prices = _w24Calculator.ConvertLongitudesToPrices(ephem.Select(entry => entry.GetCoordinatesByPlanet(planet).Longitude).ToArray(), price, 100);

                var values = new List<(DateTime, double)>();
                for (int i = 0; i < ephem.Length; i++)
                {
                    if (prices[i].HasValue && prices[i] < maxPrice)
                    {
                        values.Add((ephem[i].Time, prices[i].Value));
                    }
                }

                ret.Add(new PlanetaryLine(planet, values));
            }
            return ret;
        }

        //TODO this is for fast planets
        public async Task<IEnumerable<W24Crossing>> GetW24Crossings(Planet planet, DateTime from)
        {
            var ephem = await _ephemerisEntryRepository.GetEntries(from);

            var ret = new List<W24Crossing>();
            foreach (var entry in ephem)
            {
                var longitude = entry.GetCoordinatesByPlanet(planet).Longitude;
                if (_w24Calculator.AtW24Crossing(longitude))
                {
                    ret.Add(new W24Crossing(entry.Time, planet, longitude));
                }
            }

            return ret;
        }
    }
}
