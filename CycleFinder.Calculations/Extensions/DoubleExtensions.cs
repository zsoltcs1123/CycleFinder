namespace CycleFinder.Calculations.Extensions
{
    public static class DoubleExtensions
    {
        public static int TruncateDecimals(this double num) => (int)System.Math.Truncate(num);
        public static double TruncateIntegerPart(this double num) => System.Math.Round(num - System.Math.Truncate(num), 3);
    }
}
