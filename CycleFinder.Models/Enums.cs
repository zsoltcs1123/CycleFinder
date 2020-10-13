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
        [Description("Moon")]
        Moon,
        [Description("Sun")]
        Sun,
        [Description("Mercury")]
        Mercury,
        [Description("Venus")]
        Venus,
        [Description("Mars")]
        Mars,
        [Description("Jupiter")]
        Jupiter,
        [Description("Saturn")]
        Saturn,
        [Description("Uranus")]
        Uranus,
        [Description("Neptune")]
        Neptune,
        [Description("Pluto")]
        Pluto
    }
}
