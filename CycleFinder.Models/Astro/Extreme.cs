using CycleFinder.Models.Extensions;
using System;

namespace CycleFinder.Models.Astro
{
    public record Extreme : AstroEvent
    {
        public Planet Planet { get; }
        public ExtremeType ExtremeType { get; }
        public double ExtremeValue { get; }
        public override string Description { get => $"{Planet.GetDescription()} {ExtremeType.GetDescription()} ({ExtremeValue})"; }

        public Extreme(DateTime time, Planet planet, double extremeValue, ExtremeType extreme) : base(time)
        {
            Planet = planet;
            ExtremeValue = extremeValue;
            ExtremeType = extreme;
        }
    }
}
