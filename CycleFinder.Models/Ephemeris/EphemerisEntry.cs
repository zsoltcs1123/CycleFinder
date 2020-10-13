using System;
using System.ComponentModel.DataAnnotations;

namespace CycleFinder.Models.Ephemeris
{
    public class EphemerisEntry
    {
        [Key]
        public DateTime Time { get; set; }
        public Coordinates Moon { get; set; }
        public Coordinates Sun { get; set; }
        public Coordinates Mercury { get; set; }
        public Coordinates Venus { get; set; }
        public Coordinates Mars { get; set; }
        public Coordinates Jupiter { get; set; }
        public Coordinates Saturn { get; set; }
        public Coordinates Uranus { get; set; }
        public Coordinates Neptune { get; set; }
        public Coordinates Pluto { get; set; }

    }
}
