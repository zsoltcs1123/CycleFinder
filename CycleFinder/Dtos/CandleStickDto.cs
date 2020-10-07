namespace CycleFinder.Dtos
{
    public class CandleStickDto
    {
        public long Time { get; }
        public double Open { get; }
        public double High { get; }
        public double Low { get; }
        public double Close { get; }
        public double Volume { get; }


        public CandleStickDto(long time, double open, double high, double low, double close, double volume)
        {
            Time = time;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }
    }
}
