using System.Collections.Generic;

namespace CycleFinder.Calculations.Math
{
    public class W24Table 
    {
        //row, col
        private readonly int[,] _table;

        public int StartValue { get; private set; }

        public static W24Table TimeTable = new W24Table(0, 1, 15);

        public W24Table(int startValue, int increment, int columns)
        {
            _table = CreateTable(startValue, increment, columns);
        }

        public W24Table(double currentPrice, int increment, int upperOctaves = 1, int lowerOctaves = 1)
        {
            double div = 24 * increment;
            int intPart = (int)(currentPrice / div);
            double closestW24Value = (intPart * div);

            double startValue = closestW24Value - (lowerOctaves * div);

            _table = CreateTable((int)startValue, increment, 1 + upperOctaves + lowerOctaves);
        }

        private int[,] CreateTable(int startValue, int increment, int columns)
        {
            int[,] table = new int[24, columns];

            StartValue = startValue;

            for (int row = 0; row < 24; row++)
            {
                if (row > 0)
                {
                    table[row, 0] = table[row - 1, 0] + increment;
                }
                else
                {
                    table[row, 0] = StartValue + increment;
                }

                for (int col = 1; col < columns; col++)
                {
                    table[row, col] = table[row, col - 1] + increment * 24;
                }
            }

            return table;
        }

        public int? FindRow(double value)
        {
            for (int row = 0; row < _table.GetLength(0); row += 1)
            {
                for (int col = 0; col < _table.GetLength(1); col += 1)
                {
                    if (_table[row, col] == value) return row;
                }
            }
            return null;
        }

        public int? FindColumn(double value)
        {
            for (int row = 0; row < _table.GetLength(0); row += 1)
            {
                for (int col = 0; col < _table.GetLength(1)-1; col += 1)
                {
                    if (_table[row, col] < value && _table[row, col+1] > value) return col;
                }
            }
            return null;
        }

        public IEnumerable<int> GetRow(int row)
        {
            for (int i = 0; i < _table.GetLength(1); i += 1)
            {
                yield return _table[row, i];
            }
        }

        public IEnumerable<int> GetColumn(int column)
        {
            for (int i = 0; i < _table.GetLength(0); i += 1)
            {
                yield return _table[i, column];
            }
        }
    }
}
