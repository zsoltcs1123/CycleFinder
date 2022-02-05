using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Astro
{
    public interface IEphemerisEntryRepository
    {
        Task<IEnumerable<EphemerisEntry>> GetEntries(DateTime from, DateTime to);
    }
}
