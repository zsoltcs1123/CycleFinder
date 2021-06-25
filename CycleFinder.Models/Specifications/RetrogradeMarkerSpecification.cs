using CycleFinder.Models.Extensions;
using System;

namespace CycleFinder.Models.Specifications
{
    public class RetrogradeMarkerSpecification : MarkerSpecification
    {
        public Planet Planet { get; }
        public DateTime From { get; }

        public override bool IsValid => !Planet.HasMultipleValues() && Planet != Planet.None;

        public RetrogradeMarkerSpecification(DateTime from, Planet planet)
        {
            Planet = planet;
            From = from;
        }
    }

}
