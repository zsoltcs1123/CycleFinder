using System.Drawing;

namespace CycleFinder.Extensions
{
    public static class ColorConverterExtensions
    {
        public static string ToHexString(this Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

    }
}
