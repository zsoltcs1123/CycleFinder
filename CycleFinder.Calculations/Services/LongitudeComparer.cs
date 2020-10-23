using CycleFinder.Models;
using System;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Services
{
    public class LongitudeComparer : ILongitudeComparer
    {
        private readonly Dictionary<Planet, double> _tolerances = new Dictionary<Planet, double>()
        {
            {Planet.Moon, 0},
            {Planet.Sun, 0},
            {Planet.Mercury, 0.5},
            {Planet.Venus, 0},
            {Planet.Mars, 0},
            {Planet.Jupiter, 0},
            {Planet.Saturn, 0},
            {Planet.Uranus, 0},
            {Planet.Neptune, 0},
            {Planet.Pluto, 0},
        };

        public bool AreEqual(Planet planet, double longitude1, double longitude2) => System.Math.Abs(longitude1 - longitude2) <= _tolerances[planet];

        public bool AreEqual(double longitude1, double longitude2, double orb) => System.Math.Abs(longitude1 - longitude2) <= orb;
    }
}
