using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Astro.Retrograde
{
    public interface IRetrogradeCalculcator
    {
        public Task<IEnumerable<RetrogradeCycle>> GetRetrogradeCycles(Planet planet, DateTime from);
    }
}
