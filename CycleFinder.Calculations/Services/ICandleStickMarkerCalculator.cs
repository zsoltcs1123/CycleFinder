using CycleFinder.Models;
using CycleFinder.Models.Candles;
using CycleFinder.Models.Markers;
using CycleFinder.Models.Specifications;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Services
{
    public interface ICandleStickMarkerCalculator
    {
        public IEnumerable<ICandleStickMarker> GetMarkers(CandleMarkerSpecification spec, IEnumerable<CandleStick> candles, int order, int? limit);
    }
}
