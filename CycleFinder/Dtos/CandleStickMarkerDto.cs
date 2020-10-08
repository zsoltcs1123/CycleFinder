using CycleFinder.Extensions;
using System.Drawing;

namespace CycleFinder.Dtos
{
    public class CandleStickMarkerDto
    {
        public long Time { get; }
        public string Color { get; }

        public CandleStickMarkerDto(long time, Color color)
        {
            Time = time;
            Color = color.ToHexString();
        }
    }
}
