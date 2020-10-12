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
        [Description("1h")]
        Hourly,
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

    public enum Planet
    {
        Moon,
        Sun,
        Mercury,
        Venus,
        Mars,
        Jupiter,
        Saturn,
        Uranus,
        Neptune,
        Pluto
    }
}
