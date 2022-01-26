using CycleFinder.Calculations.Ephemeris;
using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CycleFinder.Calculations.Services.Ephemeris.Aspects
{
    public class AspectCalculator : IAspectCalculator
    {
        //TODO orb should come from API
        private static readonly double _orb = 1.00;
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;

        public AspectCalculator(IEphemerisEntryRepository ephemerisEntryRepository)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
        }

        public async Task<IEnumerable<Aspect>> GetAspects(DateTime from, DateTime to, Planet smallerPlanet, Planet largerPlanet, AspectType aspectType)
        {
            var ephem = await _ephemerisEntryRepository.GetEntries(from, to);
            var ret = new List<Aspect>();

            foreach (var entry in ephem)
            {
                var coord1 = entry.GetCoordinatesByPlanet(smallerPlanet);
                var coord2 = entry.GetCoordinatesByPlanet(largerPlanet);

                var aspect = GetAspectType(GetCircularDifference(coord1.Longitude, coord2.Longitude), _orb);
                if (aspect != null && aspectType.HasFlag(aspect))
                {
                    ret.Add(new Aspect(entry.Time, (smallerPlanet, coord1), (largerPlanet, coord2), aspect.Value));
                }
            }

            return ret;
        }

        public async Task<IEnumerable<Aspect>> GetAspects(DateTime from, DateTime to, Planet planet, AspectType aspectType)
        {
            var ephem = await _ephemerisEntryRepository.GetEntries(from, to);

            IEnumerable<Aspect> ret = new List<Aspect>();

            foreach (var pl in ((Planet[])Enum.GetValues(typeof(Planet))).Where(_ => _ > planet)){
                var asp = await GetAspects(from, to, planet, pl, aspectType);
                ret = ret.Concat(asp);
            }

            return ret;
        }

        private static double GetCircularDifference(double l1, double l2) => System.Math.Abs(360 - l1 - (360 - l2));

        private static AspectType? GetAspectType(double diff, double orb)
        {
            return diff switch
            {
                double d when 0 - orb < d && 0 + orb > d => AspectType.Conjunction,
                double d when 30 - orb < d && 30 + orb > d => AspectType.SemiSextile,
                double d when 330 - orb < d && 330 + orb > d => AspectType.SemiSextile,
                double d when 60 - orb < d && 60 + orb > d => AspectType.Sextile,
                double d when 300 - orb < d && 300 + orb > d => AspectType.Sextile,
                double d when 120 - orb < d && 120 + orb > d => AspectType.Trine,
                double d when 240 - orb < d && 240 + orb > d => AspectType.Trine,
                double d when 90 - orb < d && 90 + orb > d => AspectType.Square,
                double d when 270 - orb < d && 270 + orb > d => AspectType.Square,
                double d when 150 - orb < d && 150 + orb > d => AspectType.Inconjunct,
                double d when 210 - orb < d && 210 + orb > d => AspectType.Inconjunct,
                double d when 180 - orb < d && 180 + orb > d => AspectType.Opposition,
                _ => null,
            };
        }

    }
}
