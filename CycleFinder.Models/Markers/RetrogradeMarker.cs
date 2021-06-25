using CycleFinder.Models.Extensions;
using System;

namespace CycleFinder.Models.Markers
{
    public class RetrogradeMarker : EventMarker
    {
        public override MarkerShape Shape => MarkerShape.Square;

        public RetrogradeMarker(DateTime time, RetrogradeStatus? status, double longitude, double speed) : base(time)
        {
            Text = $"{longitude} {speed} {(status.HasValue ? status.Value.GetDescription() : string.Empty)}";
        }
    }
}
