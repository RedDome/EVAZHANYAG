using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHProject.Model
{
    public class ZHProjectEventArgs : EventArgs
    {
        private Int32 _score;
        private Boolean _isOver;
        public Int32 Score { get { return _score; } set { _score = value; } }

        public Boolean IsOver { get { return _isOver; } }

        public ZHProjectEventArgs(Int32 score, Boolean isOver)
        {
            _score = score;
            _isOver = isOver;
        }
    }
}
