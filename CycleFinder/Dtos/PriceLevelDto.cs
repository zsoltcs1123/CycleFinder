using CycleFinder.Extensions;
using CycleFinder.Models.Ephemeris;

namespace CycleFinder.Controllers
{
    public class PriceLevelDto
    {
        public string LineColor { get; }
        public double Price { get; }
        public double LineWidth { get; }

        public PriceLevelDto(W24PriceLevel priceLevel)
        {
            Price = priceLevel.Value;
            LineColor = (priceLevel.LineType == Models.W24LineType._24Line ? System.Drawing.Color.Red : System.Drawing.Color.Orange).ToHexString();
            LineWidth = 2;
        }

    }
}