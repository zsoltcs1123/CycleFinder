using CycleFinder.Models;
using System;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Services
{
    public class LongitudeComparer : ILongitudeComparer
    {
        private readonly Dictionary<Planets, double> _tolerances = new Dictionary<Planets, double>()
        {
            {Planets.Moon, 0},
            {Planets.Sun, 0},
            {Planets.Mercury, 0.5},
            {Planets.Venus, 0},
            {Planets.Mars, 0},
            {Planets.Jupiter, 0},
            {Planets.Saturn, 0},
            {Planets.Uranus, 0},
            {Planets.Neptune, 0},
            {Planets.Pluto, 0},
        };

        public bool AreEqual(Planets planet, double longitude1, double longitude2) => Math.Abs(longitude1 - longitude2) <= _tolerances[planet];
    }
}
