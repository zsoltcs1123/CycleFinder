using System;

namespace CycleFinder.Models
{
    public class CandleStick
    {
        public DateTime Time { get; }
        public long TimeInSeconds { get; }
        public double Open { get; }
        public double High { get; }
        public double Low { get;}
        public double Close { get; }
        public double Volume { get; }

        public CandleStick(double time, double open, double high, double low, double close, double volume)
        {
            Time = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(time);
            TimeInSeconds = (long)time / 1000;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public override string ToString()
        {
            return $"Date:{Time}|Open:{Open}|High:{High}|Low:{Low}|Close:{Close}|Volume:{Volume}";
        }
    }
}
