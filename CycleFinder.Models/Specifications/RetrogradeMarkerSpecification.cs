using CycleFinder.Models.Extensions;
using System;

namespace CycleFinder.Models.Specifications
{
    public class RetrogradeMarkerSpecification : MarkerSpecification
    {
        public Planet Planet { get; }
        public DateTime From { get; }

        public Predicate<RetrogradeStatus> Filter { get => FilterResult
                ? (RetrogradeStatus status) => status == RetrogradeStatus.StationaryDirect || status == RetrogradeStatus.StationaryRetrograde
                : (RetrogradeStatus status) => status == RetrogradeStatus.StationaryDirect || status == RetrogradeStatus.StationaryRetrograde
                || status == RetrogradeStatus.MaxDirect || status == RetrogradeStatus.MaxRetrograde;
        }

        public bool FilterResult {get;}

        public override bool IsValid => !Planet.HasMultipleValues() && Planet != Planet.None;

        public RetrogradeMarkerSpecification(DateTime from, Planet planet, bool filterResult)
        {
            Planet = planet;
            From = from;
            FilterResult = filterResult;
        }
    }

}
