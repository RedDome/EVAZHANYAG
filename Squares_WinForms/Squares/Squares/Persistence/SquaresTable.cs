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

            Empty,
            Player1,
            Player2
        }

        private Int32 _tableSize;
        private Field[,] _Squares;
        private Field[,] _Horizontal;
        private Field[,] _Vertical;

        public Int32 Size
        {
            get { return _tableSize; }
            set { _tableSize = value; }
        }

        public Boolean IsFilled
        {
            get
            {
                foreach (Field value in _Squares)
                    if (value == Field.Empty)
                        return false;
                return true;
            }
        }

        public SquaresTable ()
        {
            _tableSize = 3;

            _Squares = new Field[_tableSize, _tableSize];
            _Horizontal = new Field[_tableSize, _tableSize + 1];
            _Vertical = new Field[_tableSize + 1, _tableSize];

            ResetTable();
        }

        public SquaresTable(Int32 size)
        {
            _tableSize = size;

            _Squares = new Field[_tableSize, _tableSize];
            _Horizontal = new Field[_tableSize, _tableSize + 1];
            _Vertical = new Field[_tableSize + 1, _tableSize];

            ResetTable();
        }

        public Field GetTableValue(Int32 x, Int32 y, Int32 version)
        {
            if (x < 0 || x > _tableSize + 1)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y > _tableSize + 1)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");
            if ((version == 1 && (x > _tableSize || y > _tableSize)) || (version == 2 && x > _tableSize) || (version == 3 && y > _tableSize))
                throw new ArgumentOutOfRangeException("Wrong version selected.");

            if (version == 1)
                return _Squares[x, y];
            else if (version == 2)
                return _Vertical[x, y];
            else if (version == 3)
                return _Horizontal[x, y];

            return Field.Empty;
        }

        public void SetTableValue(Int32 x, Int32 y, Field field, Int32 version)
        {
            if (x < 0 || x > _tableSize + 1)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y > _tableSize + 1)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");
            if ((version == 1 && (x > _tableSize || y > _tableSize)) || (version == 2 && x > _tableSize) || (version == 3 && y > _tableSize))
                throw new ArgumentOutOfRangeException("Wrong version selected.");

            if (version == 1)
                _Squares[x, y] = field;
            else if (version == 2)
                _Vertical[x, y] = field;
            else if (version == 3)
                _Horizontal[x, y] = field;
        }

        public Boolean IsSpaceFilled (Int32 x, Int32 y, Int32 version)
        {
            if (x < 0 || x > _tableSize + 1)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y > _tableSize + 1)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");
            if ((version == 1 && (x > _tableSize || y > _tableSize)) || (version == 2 && x > _tableSize) || (version == 3 && y > _tableSize))
                throw new ArgumentOutOfRangeException("Wrong version selected.");

            if (version == 1)
                if (_Squares[x, y] == Field.Player1 || _Squares[x, y] == Field.Player2)
                    return true;
            
            if (version == 2)
                if (_Vertical[x, y] == Field.Player1 || _Vertical[x, y] == Field.Player2)
                    return true;

            if (version == 3)
                if (_Horizontal[x, y] == Field.Player1 || _Horizontal[x, y] == Field.Player2)
                    return true;

            return false;
        }

        public void ResetTable()
        {
            for (Int32 i = 0; i < _tableSize; ++i)
            {
                for (Int32 j = 0; j < _tableSize; ++j)
                {
                    _Squares[i, j] = Field.Empty;
                }
            }

            for (Int32 i = 0; i < _tableSize; ++i)
            {
                for (Int32 j = 0; j < _tableSize + 1; ++j)
                {
                    _Horizontal[i, j] = Field.Empty;
                }
            }

            for (Int32 i = 0; i < _tableSize + 1; ++i)
            {
                for (Int32 j = 0; j < _tableSize; ++j)
                {
                    _Vertical[i, j] = Field.Empty;
                }
            }
        }
    }
}
