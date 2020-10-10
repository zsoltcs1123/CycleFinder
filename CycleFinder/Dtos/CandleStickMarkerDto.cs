using CycleFinder.Extensions;
using System.Drawing;

namespace CycleFinder.Dtos
{
    public class CandleStickMarkerDto
    {
        public long Time { get; }
        public string Color { get; }
        public string Position { get;}
        public string Text { get; }

        public string Shape { get; }

        public CandleStickMarkerDto(long time, Color color, string text, MarkerPosition position, MarkerShape shape)
        {
            Time = time;
            Color = color.ToHexString();
            Text = text;
            Position = position.GetDescription();
            Shape = shape.GetDescription();
        }
    }
}
