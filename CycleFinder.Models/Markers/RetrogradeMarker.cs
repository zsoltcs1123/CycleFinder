using CycleFinder.Models.Extensions;
using System;
using System.Drawing;

namespace CycleFinder.Models.Markers
{
    public class RetrogradeMarker : EventMarker
    {
        public override MarkerShape Shape => MarkerShape.Square;

        public RetrogradeMarker(DateTime time, Color color, RetrogradeStatus? status, double longitude, double speed) : base(time)
        {
            Text = $"{longitude} {speed} {(status.HasValue ? status.Value.GetDescription() : string.Empty)}";
            Color = color;
        }
    }
}
