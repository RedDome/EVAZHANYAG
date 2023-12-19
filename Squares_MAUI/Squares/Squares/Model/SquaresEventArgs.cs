using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squares.Model
{
    public class SquaresEventArgs : EventArgs
    {
        private Int32 _currentPlayer;
        private Int32 _p1score;
        private Int32 _p2score;
        private Boolean _IsOver;
        private Int32 _winner;

        public Int32 CurrentPlayer { get { return _currentPlayer; } set { _currentPlayer = value; } }
        public Int32 P1Score { get {  return _p1score; } set { _p1score = value; } }
        public Int32 P2Score { get { return _p2score; } set { _p2score = value; } }

        public Boolean IsOver { get { return _IsOver; } }

        public Int32 Winner { get { return _winner; } set { _winner = value; } }

        public SquaresEventArgs(Int32 currentPlayer, Int32 p1score, Int32 p2score, Boolean isOver, int winner)
        {
            _currentPlayer = currentPlayer;
            _p1score = p1score;
            _p2score = p2score;
            _IsOver = isOver;
            _winner = winner;
        }
    }
}
