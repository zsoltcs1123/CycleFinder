using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Data
{
    public interface IEphemerisEntryRepository
    {
        Task<Coordinates> GetCoordinatesByTime(DateTime time, Planet planet);
        Task<IDictionary<DateTime, double>> GetDatesByLongitude(double longitude, Planet planet);
        Task<IDictionary<Planet, Coordinates>> GetCoordinatesByTime(DateTime time);
    }
}
