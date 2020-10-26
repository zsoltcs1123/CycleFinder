using CycleFinder.Extensions;
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
            Color = (pLine.Planet switch
            {
                Planet.Moon => System.Drawing.Color.Gray,
                Planet.Sun => System.Drawing.Color.Gold,
                Planet.Mercury => System.Drawing.Color.Gray,
                Planet.Venus => System.Drawing.Color.Pink,
                Planet.Mars => System.Drawing.Color.Red,
                Planet.Jupiter => System.Drawing.Color.Orange,
                Planet.Saturn => System.Drawing.Color.Brown,
                Planet.Uranus => System.Drawing.Color.Green,
                Planet.Neptune => System.Drawing.Color.Purple,
                Planet.Pluto => System.Drawing.Color.Blue,
                _ => System.Drawing.Color.Black,
            }).ToHexString();

            LineValues = pLine.Values.Select(_ => new PlanetaryLineValueDto((long)_.Time.ToUnixTimestamp(), _.Value));
        }
    }
}
