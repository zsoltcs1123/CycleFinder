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

        public Coordinates GetCoordinatesByPlanet(Planet planet)
        {
            return planet switch
            {
                Planet.Moon => Moon,
                Planet.Sun => Sun,
                Planet.Mercury => Mercury,
                Planet.Venus => Venus,
                Planet.Mars => Mars,
                Planet.Jupiter => Jupiter,
                Planet.Saturn => Saturn,
                Planet.Uranus => Uranus,
                Planet.Neptune => Neptune,
                Planet.Pluto => Pluto,
                _ => null,
            };
        }
    }
}
