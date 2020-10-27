using CycleFinder.Calculations.Math;
using System;
using System.Linq;

namespace CyceFinder.CalculationsTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var calc = new W24Calculator(8345.21, 100);
            var p1 = calc.ConvertLongitudesToPrices(new double[] { 359.7, 359.8, 359.9, 0.02, 0, 359.7 });
        }
    }
}
