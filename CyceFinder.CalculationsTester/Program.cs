using System;

namespace CyceFinder.CalculationsTester
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (int i in CycleFinder.Calculations.GenericMath.FindLocalMinima(new double[] { 1, 2, 1, 0, 2, 5, 10 }, 3))
            {
                Console.WriteLine(i);
            }
        }
    }
}
