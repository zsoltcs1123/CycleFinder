namespace CycleFinder.Models.Ephemeris
{
    public class W24PriceLevel
    {
        public double Value { get; }
        public double KeyNumber { get; }
        public W24LineType LineType { get; }

        public W24PriceLevel(double value, double keyNumber, W24LineType lineType)
        {
            Value = value;
            KeyNumber = keyNumber;
            LineType = lineType;
        }
    }
}
