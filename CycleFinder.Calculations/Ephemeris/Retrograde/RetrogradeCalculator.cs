using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Ephemeris.Retrograde
{
    public class RetrogradeCalculator : IRetrogradeCalculcator
    {
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;

        public RetrogradeCalculator(IEphemerisEntryRepository ephemerisEntryRepository)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
        }

        public async Task<RetrogradeCycles> GetRetrogradeCycles(Planet planet, DateTime from)
        {
            var entries = await _ephemerisEntryRepository.GetEntries(from);
            var tolerance = StationarySpeedTolerance(planet);
            var ret = new Dictionary<DateTime, (Coordinates Coordinates, RetrogradeStatus? RetrogradeStatus)>();

            RetrogradeStatus? previousStatus = null;

            foreach (var entry in entries)
            {
                var coords = entry.GetCoordinatesByPlanet(planet);
                var status = RetrogradeStatusFromSpeed(coords.Speed, tolerance, previousStatus);
                ret.Add(entry.Time, (coords, status));

                previousStatus = status;
            }

            return new RetrogradeCycles(planet, ret);
        }

        private RetrogradeStatus? RetrogradeStatusFromSpeed(double speed, double tolerance, RetrogradeStatus? previousStatus)
        {
            if (speed > 0 && speed > tolerance)
            {
                return RetrogradeStatus.Direct;
            }
            else if (speed >= 0 && speed <= tolerance)
            {
                if (previousStatus == RetrogradeStatus.Direct || previousStatus == RetrogradeStatus.StationaryRetrograde)
                    return RetrogradeStatus.StationaryRetrograde;
                else if (previousStatus == RetrogradeStatus.Retrograde || previousStatus == RetrogradeStatus.StationaryDirect)
                    return RetrogradeStatus.StationaryDirect;
                else return null;
            }
            else if (speed < 0 && speed < tolerance * -1)
            {
                return RetrogradeStatus.Retrograde;
            }
            else if (speed <= 0 && speed >= tolerance * -1 && (previousStatus == RetrogradeStatus.Retrograde || previousStatus == RetrogradeStatus.StationaryDirect))
            {
                return RetrogradeStatus.StationaryDirect;

            }
            else return null;
        }

        private static double StationarySpeedTolerance(Planet planet)
        {
            return planet == Planet.Mercury ? 0.04 : planet == Planet.Venus ? 0.03 : planet == Planet.Mars ? 0.01 : 0;
        }
    }
}
