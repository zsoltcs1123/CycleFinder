using System;
using System.ComponentModel;

namespace CycleFinder.Models
{
    public enum TimeFrames
    {
        [Description("1M")]
        Monthly,
        [Description("1W")]
        Weekly,
        [Description("1d")]
        Daily,
        [Description("1h")]
        Hourly,
    }

    public enum Endpoints
    {
        Connectivity,
        MarketData,
        ExchangeInfo,
    }

    public enum QuoteAssets
    {
        BTC,
        ETH,
        USDT
    }

    [Flags]
    public enum Planets
    {
        None = 0,

        [Description("Moon")]
        Moon = 1 << 0,
        [Description("Sun")]
        Sun = 1 << 1,
        [Description("Mercury")]
        Mercury = 1 << 2,
        [Description("Venus")]
        Venus = 1 << 3,
        [Description("Mars")]
        Mars = 1 << 4,
        [Description("Jupiter")]
        Jupiter = 1 << 5,
        [Description("Saturn")]
        Saturn = 1 << 6,
        [Description("Uranus")]
        Uranus = 1 << 7,
        [Description("Neptune")]
        Neptune = 1 << 8,
        [Description("Pluto")]
        Pluto = 1 << 9,
        All = ~None,
        FastPlanets = Sun | Mercury | Venus | Mars,
        FastPlanetsWithMoon = FastPlanets | Moon,
        SlowPlanets = Jupiter | Saturn | Uranus | Neptune,
        SlowPlanetsWithPlut = SlowPlanets | Pluto
    }

    [Flags]
    public enum Extremes
    {
        High = 1 << 0,
        Low= 1 << 1,
        Both = High & Low
    }

    public enum MarkerPositions
    {
        [Description("aboveBar")]
        AboveBar,
        [Description("belowBar")]
        BelowBar
    }

    public enum MarkerShapes
    {
        [Description("arrowUp")]
        ArrowUp,
        [Description("arrowDown")]
        ArrowDown,
        [Description("circle")]
        Circle,
        [Description("square")]
        Square
    }
}
