using CycleFinder.Models;
using System.Collections.Generic;

namespace CycleFinder.Services
{
    public interface IQueryParameterProcessor
    {
        public IEnumerable<Planet> PlanetsFromString(string planet);
        public Planet? PlanetFromString(string planet);
        public IEnumerable<AspectType> AscpectTypesFromString(string aspectType);
        public TimeFrame? TimeFrameFromString(string timeFrame);
        public IEnumerable<ExtremeType> ExtremeTypesFromString(string planet);
    }
}
