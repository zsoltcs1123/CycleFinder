using CycleFinder.Models;

namespace CycleFinder.Services
{
    public interface IQueryParameterProcessor
    {
        public Planet PlanetsFromString(string planet);
        public Planet? PlanetFromString(string planet);
    }
}
