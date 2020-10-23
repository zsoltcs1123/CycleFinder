using CycleFinder.Models;

namespace CycleFinder.Calculations.Services
{
    public interface ILongitudeComparer
    {
        public bool AreEqual(Planet planet, double longitude1, double longitude2);
    }
}
