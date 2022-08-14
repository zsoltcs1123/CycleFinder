using CycleFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Services
{
    public class QueryParameterProcessor : IQueryParameterProcessor
    {
        public IEnumerable<Planet> PlanetsFromString(string planet)
        {
            var ret = new List<Planet>();

            if (String.IsNullOrEmpty(planet)) return ret;

            foreach (string s in planet.Split(","))
            {
                var planetEnum = PlanetFromString(s);
                if (planetEnum.HasValue)
                {
                    ret.Add(planetEnum.Value);
                }
            }
            return ret;
        }

        private static Planet? GetPlanetFromString(string planet) => planet switch
        {
            "mo" => Planet.Moon,
            "su" => Planet.Sun,
            "me" => Planet.Mercury,
            "ve" => Planet.Venus,
            "ma" => Planet.Mars,
            "ju" => Planet.Jupiter,
            "sa" => Planet.Saturn,
            "ur" => Planet.Uranus,
            "ne" => Planet.Neptune,
            "pl" => Planet.Pluto,
            _ => null,
        };

        public IEnumerable<ExtremeType> ExtremeTypesFromString(string extremes)
        {
            var ret = new List<ExtremeType>();

            if (String.IsNullOrEmpty(extremes)) return ret;

            foreach (string s in extremes.Split(","))
            {
                var exts = ExtremeTypeFromString(s);
                if (exts.Any())
                {
                    ret.AddRange(exts);
                }
            }
            return ret;
        }

        private static ExtremeType[] ExtremeTypeFromString(string ext) => ext switch
        {
            "dec" => new ExtremeType[] { ExtremeType.DeclinationMin, ExtremeType.DeclinationMax },
            "lat" => new ExtremeType[] { ExtremeType.LatitudeMin, ExtremeType.LatitudeMax },
            "spe" => new ExtremeType[] { ExtremeType.SpeedMax, ExtremeType.SpeedMin },
            _ => Array.Empty<ExtremeType>()
        };

        public IEnumerable<AspectType> AscpectTypesFromString(string aspects)
        {
            var ret = new List<AspectType>();

            if (String.IsNullOrEmpty(aspects)) return ret;


            foreach (string s in aspects.Split(","))
            {
                var asp = AspectTypeFromString(s);
                if (asp.HasValue)
                {
                    ret.Add(asp.Value);
                }
            }
            return ret;
        }

        private static AspectType? AspectTypeFromString(string aspectType) => aspectType switch
        {
            "cj" => AspectType.Conjunction,
            "op" => AspectType.Opposition,
            "sq" => AspectType.Square,
            "tri" => AspectType.Trine,
            "sex" => AspectType.Sextile,
            "ssex" => AspectType.SemiSextile,
            "icj" => AspectType.Inconjunct,
            _ => null,
        };

        public TimeFrame? TimeFrameFromString(string timeFrame) => timeFrame switch
        {
            "1M" => TimeFrame.Monthly,
            "1W" => TimeFrame.Weekly,
            "1d" => TimeFrame.Daily,
            "4h" => TimeFrame.H4,
            "1h" => TimeFrame.H1,
            _ => null,
        };

        public Planet? PlanetFromString(string planet)
        {
            return QueryParameterProcessor.GetPlanetFromString(planet);
        }

    }
}
