using System.Collections;

namespace PathFindingDemo
{
    /// <summary>
    /// BitArray-based matrix for storing boolean values.
    /// </summary>
    internal class BitMatrix
    {
        private readonly BitArray[] _bitArrays;

        public BitMatrix(int rows, int cols)
        {
            _bitArrays = new BitArray[rows];
            for (int i = 0; i < rows; i++)
            {
                _bitArrays[i] = new BitArray(cols);
            }
        }

        public int RowsCount => _bitArrays.Length;

        public int ColumnsCount => _bitArrays[0].Length;

        public void Set(int row, int col, bool value) => _bitArrays[row][col] = value;

        public bool Get(int row, int col) => _bitArrays[row][col];

        public bool this[int row, int col]
        {
            get => Get(row, col);
            set => Set(row, col, value);
        }
    }
}
