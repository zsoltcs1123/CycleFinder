using CycleFinder.Models.Candles;
using CycleFinder.Models.Markers;
using CycleFinder.Models.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services
{
    public interface ICandleStickMarkerCalculator
    {
        public Task<IEnumerable<ICandleStickMarker>> GetMarkers(MarkerSpecification spec);
        public Task<IEnumerable<ICandleStickMarker>> GetMarkers(MarkerSpecification spec, IEnumerable<CandleStick> candles, int order, int? limit);
    }
}
