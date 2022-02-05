using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CycleFinder.Models.Astro
{
    [Owned]
    public class Coordinates
    {
        public DateTime EphemerisEntryTime { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Declination { get; set; }
        public double Speed { get; set; }

        [NotMapped]
        public bool IsRetrograde => Speed < 0;

        public override string ToString() => $"{Longitude}|{Latitude}|{Declination}|{Speed}";
    }
}
