using CycleFinder.Calculations.Math.Generic;
using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Calculations.Astro
{
    public class Ephemeris
    {
        public IEnumerable<Coordinates> GetEntriesByPlanet(Planet planet)
        {
            return Entries.Select(e => e.GetCoordinatesByPlanet(planet));
        }

        public IEnumerable<Coordinates> GetMinimaBy(Func<Coordinates, double> selector, Planet planet)
        {
            var coords = GetEntriesByPlanet(planet).ToArray();
            var indices = GenericMath.FindLocalMinima(coords.Select(selector).ToArray(), 1);
            return indices.Select(_ => coords[_]);
        }

        public IEnumerable<Coordinates> GetMaximaBy(Func<Coordinates, double> selector, Planet planet)
        {
            var coords = GetEntriesByPlanet(planet).ToArray();
            var indices = GenericMath.FindPeaks(coords.Select(selector).ToArray());
            return indices.Select(_ => coords[_]);
        }


        public List<EphemerisEntry> Entries { get; }

        public Ephemeris(IEnumerable<EphemerisEntry> entries)
        {
            Entries = new List<EphemerisEntry>(entries);
        }
    }
}
