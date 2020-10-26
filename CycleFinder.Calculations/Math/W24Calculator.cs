using System.Collections.Generic;

namespace CycleFinder.Calculations.Math
{
    public class W24Calculator 
    {
        private double _keyNumber;

        public int InitialPriceOctave { get; }

        public W24Calculator(double initialPrice, double increment)
        {
            _keyNumber = increment * 24;
            InitialPriceOctave = (int)(initialPrice / _keyNumber);
        }

        public double ConvertLongitudeToPrice(double longitude, int? octave = null)
        {
            double ratio = longitude / 24;
            double timeRatio = System.Math.Round(TruncateIntegerPart(ratio), 3);
            double basePrice = timeRatio * _keyNumber;

            return basePrice + (octave ?? InitialPriceOctave * _keyNumber);
        }

        public IEnumerable<double> ConvertLongitudesToPrices(double[] longitudes)
        {
            double previousTimeRatio, currentTimeRatio, truncatedCurrentTimeRatio;
            int currentOctave = InitialPriceOctave;

            var ret = new List<double>();

            for (int i = 0; i < longitudes.Length; i++)
            {
                currentTimeRatio = GetTimeRatio(longitudes[i]);
                truncatedCurrentTimeRatio = TruncateIntegerPart(currentTimeRatio);

                if (i > 0)
                {
                    previousTimeRatio = GetTimeRatio(longitudes[i - 1]);
                    var octaveShift = (TruncateDecimals(previousTimeRatio) - TruncateDecimals(currentTimeRatio));
                    if  (octaveShift < 0)
                    {
                        currentOctave++;
                    }
                    else if (octaveShift > 0)
                    {
                        currentOctave--;
                    }
                }

                double basePrice = truncatedCurrentTimeRatio * _keyNumber;

                ret.Add(basePrice + (currentOctave * _keyNumber));
            }
            return ret;
        }

        private static double TruncateDecimals(double num) => System.Math.Truncate(num);
        private static double TruncateIntegerPart(double num) => System.Math.Round(num - System.Math.Truncate(num),3);
        private static double GetTimeRatio(double longitude) => longitude / 24;

        private static int[,] CreateTimeTable()
        {
            int[,] table = new int[24, 15];

            table[0,0] = 0;

            for (int row = 0; row < 24; row++)
            {
                if (row > 0)
                {
                    table[row, 0] = table[row - 1, 0] + 1;
                }

                for (int col = 1; col < 15; col++)
                {
                    table[row, col] = table[row, col - 1] + 1 * 24;
                }
            }
            return table;
        }

    }
}
