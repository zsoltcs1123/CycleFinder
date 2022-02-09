using CycleFinder.Calculations.Astro;
using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CycleFinder.Calculations.Math.Extremes;
using CycleFinder.Calculations.Astro.Aspects;
using CycleFinder.Calculations.Astro.Extremes;

namespace CycleFinder.Calculations.Services.Astro
{
    public class AstroEventCalculator : IAstroEventCalculator
    {
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;
        private readonly IAspectCalculator _aspectCalculator;
        private readonly IAstroExtremeCalculator _extremeCalculator;

        public AstroEventCalculator(
            IEphemerisEntryRepository ephemerisEntryRepository, 
            IAspectCalculator aspectCalculator,
            IAstroExtremeCalculator extremeCalculator)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
            _aspectCalculator = aspectCalculator;
            _extremeCalculator = extremeCalculator;
        }

        public async Task<IEnumerable<AstroEvent>> GetAstroEvents(DateTime from, DateTime to, IEnumerable<Planet> planets)
        {
            List<AstroEvent> ret = new();

            ret.AddRange(await _extremeCalculator.GetExtremes(from, to, planets, new ExtremeType[] { ExtremeType.DeclinationMax, ExtremeType.DeclinationMin }));

            return ret;
         }

    }
}
