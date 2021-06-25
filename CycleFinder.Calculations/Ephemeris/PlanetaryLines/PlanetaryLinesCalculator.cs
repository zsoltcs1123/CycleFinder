using CycleFinder.Calculations.Math;
using CycleFinder.Calculations.Math.Sq9;
using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Ephemeris.PlanetaryLines
{
    public class PlanetaryLinesCalculator : IPlanetaryLinesCalculator
    {
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;
        private readonly IPriceTimeCalculator _w24Calculator;
        private readonly ISq9Calculator _sq9Calculator;

        public PlanetaryLinesCalculator(IEphemerisEntryRepository ephemerisEntryRepository, IPriceTimeCalculator w24Calculator, ISq9Calculator sq9Calculator)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
            _w24Calculator = w24Calculator;
            _sq9Calculator = sq9Calculator;
        }

        //TODO this is for slow planets
        public async Task<IEnumerable<PlanetaryLine>> GetPlanetaryLines(
            Planet planet,
            double currentPrice,
            DateTime from,
            DateTime to,
            TimeFrame timeFrame,
            double increment,
            int upperOctaves = 1,
            int lowerOctaves = 1)
        {
            var ephem = (await _ephemerisEntryRepository.GetEntries(from)).Where(entry => entry.Time <= to).ToArray();

            var ret = new List<PlanetaryLine>();

            //TODO shorter cycles(moon, sun, mercury, venus, mars don't display enough data due to steepness
            //TODO make this work for all price ranges and increments
            double maxPrice = currentPrice * 3;
            foreach (var price in new PriceOctaveCalculator(currentPrice, increment, 10, 2).Octaves)
            {
                var prices = _w24Calculator.ConvertLongitudesToPrices(ephem.Select(entry => entry.GetCoordinatesByPlanet(planet).Longitude).ToArray(), price, increment);

                var values = new List<(DateTime, double)>();
                for (int i = 0; i < ephem.Length; i++)
                {
                    if (prices[i].HasValue && prices[i] < maxPrice)
                    {
                        values.Add((ephem[i].Time, prices[i].Value));

                        if (timeFrame == TimeFrame.H4)
                        {
                            values.Add((ephem[i].Time.AddHours(4), prices[i].Value));
                            values.Add((ephem[i].Time.AddHours(8), prices[i].Value));
                            values.Add((ephem[i].Time.AddHours(12), prices[i].Value));
                            values.Add((ephem[i].Time.AddHours(16), prices[i].Value));

                            values.Add((ephem[i].Time.AddHours(20), prices[i].Value));
                        }
                    }
                }

                ret.Add(new PlanetaryLine(planet, values));
            }
            return ret;
        }

        //TODO this is for fast planets
        public async Task<IEnumerable<HarmonicCrossing>> GetW24Crossings(Planet planet, DateTime from)
        {
            var ephem = await _ephemerisEntryRepository.GetEntries(from);

            var ret = new List<HarmonicCrossing>();
            foreach (var entry in ephem)
            {
                var longitude = entry.GetCoordinatesByPlanet(planet).Longitude;
                if (_w24Calculator.AtHarmonicCrossing(longitude))
                {
                    ret.Add(new HarmonicCrossing(entry.Time, planet, longitude));
                }
            }

            return ret;
        }

        public async Task<IEnumerable<HarmonicCrossing>> GetSQ9Crossings(Planet planet, DateTime from)
        {
            var ephem = await _ephemerisEntryRepository.GetEntries(from);

            var ret = new List<HarmonicCrossing>();
            foreach (var entry in ephem)
            {
                var longitude = entry.GetCoordinatesByPlanet(planet).Longitude;
                if (_sq9Calculator.AtCardinalCrossing(longitude) || _sq9Calculator.AtFixedCrossing(longitude))
                {
                    ret.Add(new HarmonicCrossing(entry.Time, planet, longitude));
                }
            }

            return ret;
        }
    }
}
