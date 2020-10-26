using CycleFinder.Calculations.Math;
using System;
using System.Linq;

namespace CyceFinder.CalculationsTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var prices = new W24Calculator(8345.21, 100).ConvertLongitudesToPrices(new double[] { 23.99, 24.02, 24.1, 23.98, 23.97, 23.99, 24, 24.03 });
        
        }
    }
}
