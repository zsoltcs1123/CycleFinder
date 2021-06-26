using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using CycleFinder.Models.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Dtos
{
    public class PlanetaryLinesDto
    {
        public string Color { get; }

        public IEnumerable<PlanetaryLineValueDto> LineValues { get; }

        public PlanetaryLinesDto(PlanetaryLine pLine)
        {
            Color = pLine.Planet.ToColor().ToHexString();

            LineValues = pLine.Values.Select(_ => new PlanetaryLineValueDto((long)_.Time.ToUnixTimestamp(), _.Value));
        }
    }
}
