using CycleFinder.Extensions;
using CycleFinder.Models.Ephemeris;

namespace CycleFinder.Controllers
{
    public class PriceLevelDto
    {
        public string Color { get; }
        public double Price { get; }
        public double LineWidth { get; }

        public PriceLevelDto(PriceLevel priceLevel)
        {
            Price = priceLevel.Value;
            Color = (priceLevel.LineType == Models.W24LineType._24Line ? System.Drawing.Color.Red : System.Drawing.Color.Orange).ToHexString();
            LineWidth = 2;
        }

    }
}