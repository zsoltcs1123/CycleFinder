using Microsoft.EntityFrameworkCore;

namespace CycleFinder.Models.Ephemeris
{
    [Owned]
    public class Coordinates
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Declination { get; set; }
        public double Speed { get; set; }
    }
}
