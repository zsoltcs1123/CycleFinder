using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;

namespace CycleFinder.Services
{
    public class QueryParameterProcessor : IQueryParameterProcessor
    {
        public Planet PlanetsFromString(string planet)
        {
            if (String.IsNullOrEmpty(planet)) return Planet.AllExceptMoon;

            Planet ret = Planet.None;

            foreach (string s in planet.Split(","))
            {
                var planetEnum = PlanetFromString(s);
                if (planetEnum.HasValue)
                {
                    ret |= planetEnum.Value;
                }
            }
            return ret;
        }

        public AspectType AscpectTypesFromString(string aspect)
        {
            if (String.IsNullOrEmpty(aspect)) return AspectType.MainAspects;

            AspectType ret = AspectType.None;

            foreach (string s in aspect.Split(","))
            {
                var aspectEnum = AspectTypeFromString(s);
                if (aspectEnum.HasValue)
                {
                    ret |= aspectEnum.Value;
                }
            }
            return ret;
        }

        private static Planet? PlanetFromString(string planet) => planet switch
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
    }
}
