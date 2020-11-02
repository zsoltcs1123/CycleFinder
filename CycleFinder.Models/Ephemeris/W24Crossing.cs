using System;

namespace CycleFinder.Models.Ephemeris
{
    public class W24Crossing
    {
        public DateTime Time { get; }
        public Planet Planet { get; }
        public double Position { get; }

        public W24Crossing(DateTime time, Planet planet, double position)
        {
            Time = time;
            Planet = planet;
            Position = position;
        }
    }
}
