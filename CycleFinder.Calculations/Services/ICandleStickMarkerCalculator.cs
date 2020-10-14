using CycleFinder.Models;
using CycleFinder.Models.Markers;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Services
{
    public interface ICandleStickMarkerCalculator
    {
        public IEnumerable<ICandleStickMarker> GetMarkers(CandleStickMarkerSpecification spec);
    }
}
