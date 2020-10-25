using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Text;

namespace CycleFinder.Calculations.Services.Ephemeris
{
    public interface IPlanetaryLineCalculator
    {
        public IEnumerable<PlanetaryLine> GetPlanetaryLines(Planet planet, DateTime from, int octaves = 1);
    }
}
