using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;

namespace CycleFinder.Models.Candles
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

        public Dictionary<Planet, Coordinates> PlanetaryCoordinates { get; private set; }

        public CandleStick(double time, double open, double high, double low, double close, double volume)
        {
            Time = DateTime.UnixEpoch.AddSeconds(time);
            TimeInSeconds = (long)time;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public CandleStick(double timeInSeconds)
        {
            Time = DateTime.UnixEpoch.AddSeconds(timeInSeconds);
            TimeInSeconds = (long)timeInSeconds;
        }

        public override string ToString()
        {
            return $"Date:{Time}|Open:{Open}|High:{High}|Low:{Low}|Close:{Close}|Volume:{Volume}";
        }

        public void SetEmphemerisEntry(EphemerisEntry entry)
        {
            PlanetaryCoordinates = new Dictionary<Planet, Coordinates>
            {
                {Planet.Moon, entry.Moon },
                {Planet.Sun, entry.Sun },
                {Planet.Mercury, entry.Mercury },
                {Planet.Venus, entry.Venus },
                {Planet.Mars, entry.Mars },
                {Planet.Jupiter, entry.Jupiter },
                {Planet.Saturn, entry.Saturn },
                {Planet.Uranus, entry.Uranus },
                {Planet.Neptune, entry.Neptune },
                {Planet.Pluto, entry.Pluto },
            };
        }
    }
}
