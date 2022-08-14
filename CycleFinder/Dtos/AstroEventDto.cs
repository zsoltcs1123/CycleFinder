using CycleFinder.Models.Astro;
using CycleFinder.Models.Extensions;
using System;

namespace CycleFinder.Dtos
{
    public record AstroEventDto
    {
        public long Time { get; }
        public string Date { get; }
        public string Description { get; }

        public AstroEventDto(AstroEvent astroEvent)
        {
            Time = (long)astroEvent.Time.ToUnixTimestamp();
            Date = Time.FromUnixTimeStamp().ToShortDateString();
            Description = astroEvent.Description;
        }
    }
}
