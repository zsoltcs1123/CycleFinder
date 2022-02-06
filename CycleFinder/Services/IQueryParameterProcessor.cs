using CycleFinder.Models;
using System.Collections.Generic;

namespace CycleFinder.Services
{
    public interface IQueryParameterProcessor
    {
        public IEnumerable<Planet> PlanetsFromString(string planet);
        public AspectType AscpectTypesFromString(string aspectType);
        public TimeFrame? TimeFrameFromString(string timeFrame);
    }
}
