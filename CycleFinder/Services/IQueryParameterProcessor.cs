using CycleFinder.Models;

namespace CycleFinder.Services
{
    public interface IQueryParameterProcessor
    {
        public Planet PlanetsFromString(string planet);
        public AspectType AscpectTypesFromString(string aspectType);
        public TimeFrame? TimeFrameFromString(string timeFrame);
    }
}
