using CycleFinder.Models;
using CycleFinder.Models.Candles;
using CycleFinder.Models.Markers;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Services
{
    public interface ICandleStickMarkerCalculator
    {
        public IEnumerable<ICandleStickMarker> GetMarkers(CandleStickMarkerSpecification spec, IEnumerable<CandleStick> candles, int order, int? limit);
    }
}
