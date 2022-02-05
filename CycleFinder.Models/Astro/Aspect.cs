using CycleFinder.Models.Extensions;
using System;

namespace CycleFinder.Models.Astro
{
    public record Aspect : AstroEvent
    {
        public (Planet Planet, Coordinates cords) SmallerPlanet { get; }
        public (Planet Planet, Coordinates cords) LargerPlanet { get; }
        public AspectType AspectType { get; }

        public override string Description
            => $"{SmallerPlanet.Planet.GetDescription()} {(SmallerPlanet.cords.IsRetrograde ? "rx" : "")} {AspectType.GetDescription()}" +
            $" {LargerPlanet.Planet.GetDescription()} {(LargerPlanet.cords.IsRetrograde ? "rx" : "")}"; 

        public Aspect(DateTime time, (Planet Planet, Coordinates cords) smallerPlanet, (Planet Planet, Coordinates cords) largerPlanet, AspectType aspectType) : base(time)
        {
            SmallerPlanet = smallerPlanet;
            LargerPlanet = largerPlanet;
            AspectType = aspectType;
        }
    }
}
