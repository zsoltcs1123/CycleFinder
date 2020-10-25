using CycleFinder.Models;
using System;

namespace CycleFinder.Services
{
    public class QueryParameterProcessor : IQueryParameterProcessor
    {
        public Planet PlanetsFromString(string planet)
        {
            if (String.IsNullOrEmpty(planet)) return Planet.All;

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

        public Planet? PlanetFromString(string planet) => planet switch
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
    }
}
