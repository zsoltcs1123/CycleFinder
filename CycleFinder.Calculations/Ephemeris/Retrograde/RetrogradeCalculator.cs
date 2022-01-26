using CycleFinder.Calculations.Math.Generic;
using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<RetrogradeCycle>> GetRetrogradeCycles(Planet planet, DateTime from)
        {
            //TODO
            var entries = await _ephemerisEntryRepository.GetEntries(from, from);
            var statuses = MapRetrogradeStatuses(entries, planet);
            var cycles = CreateCycles(statuses, planet);

            return cycles;
        }

        private static List<RetrogradeCycle> CreateCycles(Dictionary<DateTime, (Coordinates Coordinates, RetrogradeStatus RetrogradeStatus)> statuses, Planet planet)
        {
            var changeDates = new List<DateTime>();
            var previousStatus = statuses.First().Value.RetrogradeStatus;

            foreach (var kvp in statuses)
            {
                if (kvp.Value.RetrogradeStatus != previousStatus)
                {
                    changeDates.Add(kvp.Key);
                }
                previousStatus = kvp.Value.RetrogradeStatus;
            }

            var cycles = new List<RetrogradeCycle>();

            var startDate = statuses.First().Key;
            foreach (var chgDate in changeDates)
            {
                var values = statuses.Where(_ => _.Key >= startDate && _.Key < chgDate);
                var cycle = new RetrogradeCycle(planet, values.ToDictionary(_ => _.Key, _ => _.Value.Coordinates), values.First().Value.RetrogradeStatus, startDate, chgDate);
                cycles.Add(cycle);
                startDate = chgDate;
            }

            return cycles;
        }

        private static Dictionary<DateTime, (Coordinates Coordinates, RetrogradeStatus RetrogradeStatus)> MapRetrogradeStatuses(IEnumerable<EphemerisEntry> entries, Planet planet)
        {
            var tolerance = StationarySpeedTolerance(planet);
            var ret = new Dictionary<DateTime, (Coordinates Coordinates, RetrogradeStatus RetrogradeStatus)>();

            RetrogradeStatus previousStatus = RetrogradeStatus.Unknown;

            foreach (var entry in entries)
            {
                var coords = entry.GetCoordinatesByPlanet(planet);
                var status = RetrogradeStatusFromSpeed(coords.Speed, tolerance, previousStatus);
                ret.Add(entry.Time, (coords, status));

                previousStatus = status;
            }

            return ret;
        }

        private static RetrogradeStatus RetrogradeStatusFromSpeed(double speed, double tolerance, RetrogradeStatus previousStatus)
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
                else return RetrogradeStatus.Unknown; 
            }
            else if (speed < 0 && speed < tolerance * -1)
            {
                return RetrogradeStatus.Retrograde;
            }
            else if (speed <= 0 && speed >= tolerance * -1)
            {
                if (previousStatus == RetrogradeStatus.Direct || previousStatus == RetrogradeStatus.StationaryRetrograde)
                    return RetrogradeStatus.StationaryRetrograde;
                else if (previousStatus == RetrogradeStatus.Retrograde || previousStatus == RetrogradeStatus.StationaryDirect)
                    return RetrogradeStatus.StationaryDirect;
                else return RetrogradeStatus.Unknown;

            }
            else return RetrogradeStatus.Unknown;
        }

        private static double StationarySpeedTolerance(Planet planet)
        {
            return planet == Planet.Mercury ? 0.07 : planet == Planet.Venus ? 0.03 : planet == Planet.Mars ? 0.01 : 0;
        }
    }
}
