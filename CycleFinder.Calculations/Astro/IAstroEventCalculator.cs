using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Astro
{
    public interface IAstroEventCalculator
    {
        public Task<IEnumerable<AstroEvent>> GetAstroEvents(
            DateTime from, 
            DateTime to, 
            IEnumerable<Planet> planets, 
            IEnumerable<ExtremeType> extremes,
            IEnumerable<AspectType> aspects);

        public Task<IEnumerable<AstroEvent>> GetAspectsBetweenPlanets(
            DateTime from,
            DateTime to,
            Planet smallerPlanet,
            Planet largerPlanet,
            IEnumerable<AspectType> aspects);
    }
}
