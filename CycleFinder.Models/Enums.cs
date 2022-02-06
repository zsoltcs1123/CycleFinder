using System;
using System.ComponentModel;

namespace CycleFinder.Models
{
    public enum TimeFrame
    {
        [Description("1M")]
        Monthly,
        [Description("1W")]
        Weekly,
        [Description("1d")]
        Daily,
        [Description("4h")]
        H4,
        [Description("1h")]
        H1,
    }

    public enum Endpoint
    {
        Connectivity,
        MarketData,
        ExchangeInfo,
    }

    public enum QuoteAsset
    {
        BTC,
        ETH,
        USDT
    }

    [Flags]
    public enum Planet
    {
        [Description("Moon")]
        Moon = 1 << 0,
        [Description("Mercury")]
        Mercury = 1 << 2,
        [Description("Venus")]
        Venus = 1 << 3,
        [Description("Mars")]
        Mars = 1 << 4,
        [Description("Sun")]
        Sun = 1 << 1,
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
    }

    public enum ExtremeType
    {
        [Description("MIN DECLINATION")]
        DeclinationMin,
        [Description("MAX DECLINATION")]
        DeclinationMax,
        [Description("MIN SPEED")]
        SpeedMin,
        [Description("MAX SPEED")]
        SpeedMax,
        [Description("MIN LATITUDE")]
        LatitudeMin,
        [Description("MAX LATITUDE")]
        LatitudeMax
    }

    public enum RetrogradeStatus
    {
        [Description("D")]
        Direct,
        [Description("MAX D")]
        MaxDirect,
        [Description("SD")]
        StationaryDirect,
        [Description("R")]
        Retrograde,
        [Description("MAX R")]
        MaxRetrograde,
        [Description("SR")]
        StationaryRetrograde,
        [Description("Unknown")]
        Unknown,
    }

    public enum AspectType
    {
        None = 0,

        [Description("cj")]
        Conjunction = 1 << 0,
        [Description("op")]
        Opposition = 1 << 1,
        [Description("sq")]
        Square = 1 << 2,
        [Description("tri")]
        Trine = 1 << 3,
        [Description("sex")]
        Sextile = 1 << 4,
        [Description("ssex")]
        SemiSextile = 1 << 5,
        [Description("icj")]
        Inconjunct = 1 << 6,

        Any = ~None,
        MainAspects = Conjunction | Opposition | Square | Trine | Sextile
    }

    public enum MarkerPosition
    {
        [Description("aboveBar")]
        AboveBar,
        [Description("belowBar")]
        BelowBar
    }

    public enum MarkerShape
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

    public enum W24LineType
    {
        _24Line,
        IntermediateLine,
    }
}
