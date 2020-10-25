using CycleFinder.Calculations.Math;
using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Ephemeris
{
    public class PlanetaryLineCalculator : IPlanetaryLineCalculator
    {
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;

        public PlanetaryLineCalculator(IEphemerisEntryRepository ephemerisEntryRepository)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
        }

        public async Task<IEnumerable<PlanetaryLine>> GetPlanetaryLines(Planet planet, double currentPrice, DateTime from, int octaves = 3)
        {
            var ephem = await _ephemerisEntryRepository.GetEntries(from);
            var priceTable = new W24Table(100, 100);
            var colIndex = priceTable.FindColumn(currentPrice); //current octave
            var column = priceTable.GetColumn(colIndex.Value).ToArray();

            var ret = ephem.Select(entry => (entry.Time, CalculatePriceFromLongitude(entry.GetCoordinatesByPlanet(planet).Longitude, column)));

            return new List<PlanetaryLine>() { new PlanetaryLine(planet, ret) };
        }

        private int CalculatePriceFromLongitude(double longitude, int[] column)
        {
            if (longitude < 0.5)
            {
                longitude = 360 + longitude;
            }

            var rowIndex = W24Table.TimeTable.FindRow((int)System.Math.Round(longitude, 0));

            if (!rowIndex.HasValue)
            {
                return 0;
            }

            return column[rowIndex.Value];
        }
    }
}
