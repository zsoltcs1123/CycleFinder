using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CycleFinder.Models.Ephemeris
{
    [Owned]
    public class Coordinates
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Declination { get; set; }
        public double Speed { get; set; }

        [NotMapped]
        public bool IsRetrograde => Speed < 0;
    }
}
