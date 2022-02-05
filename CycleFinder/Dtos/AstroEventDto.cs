using CycleFinder.Models.Astro;
using CycleFinder.Models.Extensions;

namespace CycleFinder.Dtos
{
    public record AstroEventDto
    {
        public long Time { get; }
        public string Description { get; }

        public AstroEventDto(AstroEvent astroEvent)
        {
            Time = (long)astroEvent.Time.ToUnixTimestamp();
            Description = astroEvent.Description;
        }
    }
}
