using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Astro.Aspects
{
    public interface IAstroEventCalculator
    {
        public Task<IEnumerable<Aspect>> GetAspects(DateTime from, DateTime to, Planet planet, AspectType aspectType);
        public Task<IEnumerable<AstroEvent>> GetAstroEvents(DateTime from, DateTime to, Planet planet);
    }
}
