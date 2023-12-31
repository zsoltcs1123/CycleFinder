﻿using CycleFinder.Models.Ephemeris;
using CycleFinder.Models.Extensions;
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

        public CandleStick(DateTime time, double open, double high, double low, double close, double volume) : this(time.ToUnixTimestamp(), open, high, low, close, volume) { }


        public CandleStick(double timeInSeconds)
        {
            Time = DateTime.UnixEpoch.AddSeconds(timeInSeconds);
            TimeInSeconds = (long)timeInSeconds;
        }

        public override string ToString()
        {
            return $"Date:{Time}|Open:{Open}|High:{High}|Low:{Low}|Close:{Close}|Volume:{Volume}";
        }
    }
}
