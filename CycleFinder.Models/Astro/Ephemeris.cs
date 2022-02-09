using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Models.Astro
{
    public class Ephemeris
    {
        public List<EphemerisEntry> Entries { get; }

        public Ephemeris(IEnumerable<EphemerisEntry> entries)
        {
            Entries = new List<EphemerisEntry>(entries);
        }

        public IEnumerable<Coordinates> GetEntriesByPlanet(Planet planet)
        {
            return Entries.Select(e => e.GetCoordinatesByPlanet(planet));
        }
    }
}
