using System;

namespace CycleFinder.Models.Ephemeris
{
    public class HarmonicCrossing
    {
        public DateTime Time { get; }
        public Planet Planet { get; }
        public double Position { get; }

        public HarmonicCrossing(DateTime time, Planet planet, double position)
        {
            Time = time;
            Planet = planet;
            Position = position;
        }
    }
}
