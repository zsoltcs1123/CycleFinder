using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Astro
{
    public interface IAstroEventCalculator
    {
        public Task<IEnumerable<AstroEvent>> GetAstroEvents(DateTime from, DateTime to, IEnumerable<Planet> planets);
    }
}
