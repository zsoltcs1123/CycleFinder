using System;

namespace CycleFinder.Models.Ephemeris
{
    public abstract record AstroEvent
    {
        public DateTime Time { get; private set; }

        public abstract string Description { get; }

        public AstroEvent(DateTime time)
        {
            Time = time;
        }
    }
}
