using CycleFinder.Models.Ephemeris;
using System.Drawing;

namespace CycleFinder.Models.Markers
{
    public class AspectMarker : EventMarker
    {
        public AspectMarker(Aspect aspect) : base(aspect.Time)
        {
            Color = aspect.AspectType switch
            {
                AspectType.Conjunction => Color.Blue,
                AspectType.Opposition => Color.Purple,
                AspectType.Sextile => Color.Cyan,
                AspectType.Square => Color.Red,
                AspectType.Trine => Color.Green,
                _ => Color.Black,
            };

            Text = $"{aspect.Planet1.Planet.GetDescription()}" +
            $" {(aspect.Planet1.IsRetrograde ? " (R)" : "")}" +
            $" {aspect.AspectType.GetDescription()} " +
            $"{aspect.Planet2.Planet.GetDescription()} " +
            $"{(aspect.Planet2.IsRetrograde ? " (R)" : "")}";
        }
    }
}
