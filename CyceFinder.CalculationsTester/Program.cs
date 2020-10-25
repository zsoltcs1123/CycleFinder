using CycleFinder.Calculations.Math;
using System;
using System.Linq;

namespace CyceFinder.CalculationsTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var asd = W24Table.TimeTable.FindRow(295);

            var priceTable = new W24Table(8432.5, 100,11,3);
            double price = 8432.12;

            var column = priceTable.FindColumn(price);

            var prices = priceTable.GetRow(asd.Value).ToArray()[column.Value];
        }
    }
}
