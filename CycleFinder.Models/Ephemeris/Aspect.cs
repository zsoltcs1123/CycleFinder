using System;

namespace CycleFinder.Models.Ephemeris
{
    public class Aspect
    {
        public DateTime Time { get; }
        public (Planet Planet, bool IsRetrograde) Planet1 { get; }
        public (Planet Planet, bool IsRetrograde) Planet2 { get; }
        public AspectType AspectType { get; }

        public Aspect(DateTime time, (Planet Planet, bool IsRetrograde) planet1, (Planet Planet, bool IsRetrograde) planet2, AspectType aspectType)
        {
            Time = time;
            Planet1 = planet1;
            Planet2 = planet2;
            AspectType = aspectType;
        }
    }
}
