namespace CycleFinder.Models.Ephemeris
{
    public class PriceLevel
    {
        public double Value { get; }
        public W24LineType LineType { get; }

        public PriceLevel(double value, W24LineType lineType)
        {
            Value = value;
            LineType = lineType;
        }
    }
}
