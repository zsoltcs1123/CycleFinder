namespace CycleFinder.Dtos
{
    public class PlanetaryLineValueDto
    {
        public long Time { get; }
        public double Value { get; }

        public PlanetaryLineValueDto(long time, double value)
        {
            Time = time;
            Value = value;
        }
    }
}
