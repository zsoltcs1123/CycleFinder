using CycleFinder.Models.Extensions;
using CycleFinder.Models.Markers;
using System;

namespace CycleFinder.Dtos
{
    public class CandleStickMarkerDto
    {
        private readonly DateTime _time;
        public long Time { get; }
        public string Color { get; }
        public string Position { get;}
        public string Text { get; }
        public string Shape { get; }
        public bool IsInTheFuture { get => _time > DateTime.Now; }

        //DEBUG
        public string TimeStr { get => _time.ToShortDateString(); }

        public CandleStickMarkerDto(ICandleStickMarker candleMarker)
        {
            _time = candleMarker.Time;
            Time = (long)candleMarker.Time.ToUnixTimestamp();
            Color = candleMarker.Color.ToHexString();
            Text = candleMarker.Text;
            Position = candleMarker.Position.GetDescription();
            Shape = candleMarker.Shape.GetDescription();
        }
    }
}
