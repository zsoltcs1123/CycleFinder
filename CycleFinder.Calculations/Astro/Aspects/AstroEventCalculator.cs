using CycleFinder.Calculations.Astro;
using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CycleFinder.Calculations.Services.Astro.Aspects
{
    public class AstroEventCalculator : IAstroEventCalculator
    {
        //TODO orb should come from API. Also, maybe orb per planet?
        private static readonly double _orb = 2.00;
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;

        private readonly Planet[] Planets = ((Planet[])Enum.GetValues(typeof(Planet))).Where(_ => _ != Planet.None && _ != Planet.All && _ != Planet.AllExceptMoon).ToArray();

        public AstroEventCalculator(IEphemerisEntryRepository ephemerisEntryRepository)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
        }

        public async Task<IEnumerable<Aspect>> GetAspects(DateTime from, DateTime to, Planet planet, AspectType aspectType)
        {
            var ephem = new Ephemeris(await _ephemerisEntryRepository.GetEntries(from, to));

            IEnumerable<Aspect> ret = new List<Aspect>();

            if (planet == Planet.None || planet == Planet.All)
            {
                return ret;
            }

            if (planet == Planet.AllExceptMoon)
            {
                foreach (var spl in Planets.Where(_ => _ != Planet.Moon))
                {
                    //Get Aspects
                    foreach (var lpl in Planets.Where(_ => _ > spl))
                    {
                        var aspects = GetAspectsForPlanetPairs(ephem.Entries, spl, lpl, aspectType);
                        ret = ret.Concat(aspects);
                    }
                }
            }

            else
            {
                foreach (var lpl in Planets.Where(_ => _ > planet))
                {
                    var aspects = GetAspectsForPlanetPairs(ephem.Entries, planet, lpl, aspectType);
                    ret = ret.Concat(aspects);
                }
            }
            var hits = ret.GroupBy(a => a.Description).Select(g => new { Name = g.Key, FirstDate = g.OrderBy(a => a.Time).First() }).ToList();
            return ret.OrderBy(_ => _.Time);
            //return hits.Select(g => g.FirstDate).OrderBy(_ => _.Time);
        }

        public async Task<IEnumerable<AstroEvent>> GetAstroEvents(DateTime from, DateTime to, Planet planet)
        {
            var ephem = new Ephemeris(await _ephemerisEntryRepository.GetEntries(from, to));
            List<AstroEvent> ret = new();

            if (planet == Planet.None || planet == Planet.All)
            {
                return ret;
            }

            if (planet == Planet.AllExceptMoon)
            {
                foreach (var spl in Planets.Where(_ => _ != Planet.Moon))
                {
                    //Get Extremes
                    ret.AddRange(GetExtremes(ephem, spl, ExtremeType.DeclinationMin));
                    ret.AddRange(GetExtremes(ephem, spl, ExtremeType.DeclinationMax));
                    //ret.AddRange(GetExtremes(ephem, spl, ExtremeType.LatitudeMin));
                    //ret.AddRange(GetExtremes(ephem, spl, ExtremeType.LatitudeMax));
                    //ret.AddRange(GetExtremes(ephem, spl, ExtremeType.SpeedMin));
                    //ret.AddRange(GetExtremes(ephem, spl, ExtremeType.SpeedMax));


                    //Get Aspects
/*                    foreach (var lpl in Planets.Where(_ => _ > spl))
                    {
                        var aspects = GetAspectsForPlanetPairs(ephem.Entries, spl, lpl, AspectType.MainAspects);
                        ret.AddRange(aspects);
                    }*/
                }
            }
            else
            {
                //ret.AddRange(GetExtremes(ephem, planet, ExtremeType.DeclinationMin));
                ret.AddRange(GetExtremes(ephem, planet, ExtremeType.DeclinationMax));
            }
            return ret;
         }

        private static IEnumerable<AstroEvent> GetExtremes(Ephemeris ephem, Planet planet, ExtremeType extremeType)
        {
            Func<Coordinates, double> selector = extremeType switch
            {
                ExtremeType.DeclinationMax or ExtremeType.DeclinationMin => c => c.Declination,
                ExtremeType.LatitudeMax or ExtremeType.LatitudeMin => c => c.Latitude,
                ExtremeType.SpeedMax or ExtremeType.SpeedMin => c => c.Speed,
                _ => throw new NotImplementedException()
            };

            Func<Func<Coordinates, double>, Planet, IEnumerable<Coordinates>> extremeFunc = extremeType switch
            {
                ExtremeType.LatitudeMax or ExtremeType.SpeedMax or ExtremeType.DeclinationMax => ephem.GetMaximaBy,
                ExtremeType.LatitudeMin or ExtremeType.SpeedMin or ExtremeType.DeclinationMin => ephem.GetMinimaBy,
                _ => throw new NotImplementedException()
            };

            //Skip the first & last ones because they will always be an extreme due to being the first/last... deal with this later
            return extremeFunc(selector, planet).Select(c => new Extreme(c.EphemerisEntryTime, planet, selector(c), extremeType));
        }

        private static IEnumerable<Aspect> GetAspectsForPlanetPairs(
            IEnumerable<EphemerisEntry> ephem,
            Planet smallerPlanet,
            Planet largerPlanet,
            AspectType aspectType)
        {
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


        private static double GetCircularDifference(double l1, double l2) => System.Math.Abs(360 - l1 - (360 - l2));

        private static AspectType? GetAspectType(double diff, double orb)
        {
            return diff switch
            {
                double d when 0 - orb <= d && 0 + orb >= d => AspectType.Conjunction,
                double d when 30 - orb <= d && 30 + orb >= d => AspectType.SemiSextile,
                double d when 330 - orb <= d && 330 + orb >= d => AspectType.SemiSextile,
                double d when 60 - orb <= d && 60 + orb >= d => AspectType.Sextile,
                double d when 300 - orb <= d && 300 + orb >= d => AspectType.Sextile,
                double d when 120 - orb <= d && 120 + orb >= d => AspectType.Trine,
                double d when 240 - orb <= d && 240 + orb >= d => AspectType.Trine,
                double d when 90 - orb <= d && 90 + orb >= d => AspectType.Square,
                double d when 270 - orb <= d && 270 + orb >= d => AspectType.Square,
                double d when 150 - orb <= d && 150 + orb >= d => AspectType.Inconjunct,
                double d when 210 - orb <= d && 210 + orb >= d => AspectType.Inconjunct,
                double d when 180 - orb <= d && 180 + orb >= d => AspectType.Opposition,
                _ => null,
            };
        }
    }
}
