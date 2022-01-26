﻿using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Ephemeris
{
    public interface IEphemerisEntryRepository
    {
        Task<IEnumerable<EphemerisEntry>> GetEntries(DateTime startTime);
    }
}