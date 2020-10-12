using System;
using System.ComponentModel.DataAnnotations;

namespace CycleFinder.Models.Planets
{
    public class Ephemeris
    {
        [Key]
        public DateTime Date { get; set; }
        public Coordinate Moon { get; set; }
        public Coordinate Sun { get; set; }
        public Coordinate Mercury { get; set; }
        public Coordinate Venus { get; set; }
        public Coordinate Mars { get; set; }
        public Coordinate Jupiter { get; set; }
        public Coordinate Saturn { get; set; }
        public Coordinate Uranus { get; set; }
        public Coordinate Neptune { get; set; }
        public Coordinate Pluto { get; set; }

    }
}
