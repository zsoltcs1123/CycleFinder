using CycleFinder.Calculations.Math;
using System;

namespace CyceFinder.CalculationsTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var table = new W24TimeTable();
            var asd = table.FindRow(295);

            var priceTable = new W24Table(100, 100);

            var price = priceTable.GetRow(asd.Value);
        }
    }
}
