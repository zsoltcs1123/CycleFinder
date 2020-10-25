using System.Collections.Generic;

namespace CycleFinder.Calculations.Math
{
    public class W24Table 
    {
        private int[,] _table = new int[24, 15];

        public W24Table(int startValue, int increment)
        {
            _table[0, 0] = startValue;

            for (int i = 0; i < 24; i++)
            {
                if (i > 0)
                {
                    _table[i, 0] = _table[i - 1, 0] + increment;
                }

                for (int j = 1; j < 15; j++)
                {
                    _table[i, j] = _table[i, j - 1] + increment*24;
                }
            }
        }

        public int? FindRow(int value)
        {
            for (int i = 0; i < _table.GetLength(0); i += 1)
            {
                for (int j = 0; j < _table.GetLength(1); j += 1)
                {
                    if (_table[i, j] == value) return i;
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
    }
}
