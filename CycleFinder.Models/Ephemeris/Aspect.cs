using System;

namespace CycleFinder.Models.Ephemeris
{
    public class Aspect
    {
        public DateTime Time { get; }
        public Planet Planet1 { get; }
        public Planet Planet2 { get; }
        public AspectType AspectType { get; }

        public string Text => $"{Planet1.GetDescription()} {AspectType.GetDescription()} {Planet2.GetDescription()}";

        public Aspect(DateTime time, Planet planet1, Planet planet2, AspectType aspectType)
        {
            Time = time;
            Planet1 = planet1;
            Planet2 = planet2;
            AspectType = aspectType;
        }
    }
}
