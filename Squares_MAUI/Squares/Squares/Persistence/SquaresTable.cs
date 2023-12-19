using Squares.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Squares.Persistence
{
    public class SquaresTable
    {
        public enum Field
        {
            NotUsed,
            Empty,
            Player1,
            Player2
        }

        private Int32 _tableSize;
        private Int32 _wpfSize;
        private Field[,] _Squares;

        public Int32 Size
        {
            get { return _tableSize; }
            set { _tableSize = value; }
        }

        public Int32 WPFSize
        {
            get { return _wpfSize; }
            set { _wpfSize = value; }
        }

        public Boolean IsFilled
        {
            get
            {
                Int32 num = 0;
                for (int x = 0; x < WPFSize; x++)
                {
                    for (int y = 0; y < WPFSize; y++)
                    {
                        if (x % 2 == 1 && y % 2 == 1)
                        {
                            if (IsSpaceFilled(x, y))
                                num++;
                        }
                    }
                }

                if (num == Size * Size)
                    return true;

                return false;
            }
        }

        public SquaresTable ()
        {
            _tableSize = 3;
            _wpfSize = 7;

            _Squares = new Field[_wpfSize, _wpfSize];

            ResetTable();
        }

        public SquaresTable(Int32 size)
        {
            _tableSize = size;

            if (size == 3)
            {
                _wpfSize = 7;
            }
            else if (size == 5)
            {
                _wpfSize = 11;
            }
            else if (size == 9)
            {
                _wpfSize = 19;
            }


            _Squares = new Field[_wpfSize, _wpfSize];

            ResetTable();
        }

        public Field GetTableValue(Int32 x, Int32 y)
        {
            if (x < 0 || x > _wpfSize + 1)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y > _wpfSize + 1)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _Squares[x, y];
        }

        public void SetTableValue(Int32 x, Int32 y, Field field)
        {
            if (x < 0 || x > _wpfSize + 1)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y > _wpfSize + 1)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            _Squares[x, y] = field;
        }

        public Boolean IsSpaceFilled (Int32 x, Int32 y)
        {
            if (x < 0 || x > _wpfSize + 1)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y > _wpfSize + 1)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return (_Squares[x, y] == Field.Player1 || _Squares[x, y] == Field.Player2);
        }

        public Boolean IsSpaceNotUsed(Int32 x, Int32 y)
        {
            if (x < 0 || x > _wpfSize + 1)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y > _wpfSize + 1)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _Squares[x, y] == Field.NotUsed;
        }

        public void ResetTable()
        {
            for (Int32 i = 0; i < _wpfSize; ++i)
            {
                for (Int32 j = 0; j < _wpfSize; ++j)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            _Squares[i, j] = Field.NotUsed;
                        }
                        else
                        {
                            _Squares[i, j] = Field.Empty;
                        }
                    }
                    else
                    {
                        _Squares[i, j] = Field.Empty;
                    }
                }
            }
        }
    }
}
