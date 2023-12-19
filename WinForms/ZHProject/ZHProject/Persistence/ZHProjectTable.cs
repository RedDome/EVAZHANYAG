using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHProject.Persistence
{
    public class ZHProjectTable
    {
        private Int32 _tableSize;
        private Int32[,] _fields;

        public Int32 Size
        {
            get { return _tableSize; }
            set { _tableSize = value; }
        }

        /*
        public Boolean IsFilled
        {
           // TODO
        }
        */

        public ZHProjectTable()
        {
            _tableSize = 5;
            _fields = new Int32[_tableSize, _tableSize];
        }

        public Boolean IsEmpty(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fields.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fields.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _fields[x, y] == 0;
        }

        public Int32 GetValue(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fields.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fields.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _fields[x, y];
        }

        public void SetValue(Int32 x, Int32 y, Int32 value, Boolean lockField)
        {
            if (x < 0 || x >= _fields.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fields.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            // CHECKSTEP?
            _fields[x, y] = value;
        }


    }
}
