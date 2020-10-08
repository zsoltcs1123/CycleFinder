using CycleFinder.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Dtos
{
    public class LowCandleStickDto
    {
        public long Time { get; }
        public string Color { get; }

        public LowCandleStickDto(long time, Color color)
        {
            Time = time;
            Color = color.ToHexString();
        }
    }
}
