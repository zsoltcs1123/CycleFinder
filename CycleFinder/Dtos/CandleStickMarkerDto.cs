using CycleFinder.Extensions;
using CycleFinder.Models;
using System;
using System.Drawing;

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

        public CandleStickMarkerDto(CandleStick candle, Color color, string text, MarkerPosition position, MarkerShape shape)
        {
            _time = candle.Time;
            Time = candle.TimeInSeconds;
            Color = color.ToHexString();
            Text = text;
            Position = position.GetDescription();
            Shape = shape.GetDescription();
        }
    }
}
