using System;

namespace CyceFinder.CalculationsTester
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (int i in CycleFinder.Calculations.Math.FindLocalMinima(new int[] { 1, 2, 1, 4, 2, 5 }, 1))
            {
                Console.WriteLine(i);
            }
        }
    }
}
