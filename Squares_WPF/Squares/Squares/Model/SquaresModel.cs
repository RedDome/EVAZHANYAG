using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Squares.Persistence;

namespace Squares.Model
{
    public enum TableSize { Small, Medium, Large }
    public class SquaresModel
    {
        private ISquaresDataAccess _dataAccess = null!;
        private SquaresTable _gameTable;
        private TableSize _tableSize;
        private Int32 _currentPlayer = 1;
        private Int32 _p1score;
        private Int32 _p2score;

        public Int32 GameSize
        {
            get { return _gameTable.Size; }
            set { _gameTable.Size = value; }
        }

        public Int32 WPFGameSize
        {
            get { return _gameTable.WPFSize; }
            set { _gameTable.WPFSize = value; }
        }

        public Int32 CurrentPlayer { get { return _currentPlayer; } set { _currentPlayer = value; } }
        public Int32 P1Score { get { return _p1score; } set { _p1score = value; } }
        public Int32 P2Score { get { return _p2score; } set { _p2score = value; } }

        public TableSize TableSize { get { return _tableSize; } set { _tableSize = value; } }

        public SquaresTable Table { get { return _gameTable; } }

        public Boolean IsGameOver { get { return _gameTable.IsFilled; } }

        public event EventHandler<SquaresEventArgs>? PlayerChanged;

        public event EventHandler<SquaresEventArgs>? PointChanged;

        public event EventHandler<SquaresEventArgs>? GameOver;

        public SquaresModel() 
        {
            _gameTable = new SquaresTable();
        }

        public SquaresModel(ISquaresDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _gameTable = new SquaresTable();
        }

        public void NewGame()
        { 
            _gameTable = new SquaresTable(GameSize);

            CurrentPlayer = 1;
            P1Score = 0;
            P2Score = 0;

            switch (_tableSize)
            {
                case TableSize.Small:
                    _gameTable = new SquaresTable(3);
                    break;
                case TableSize.Medium:
                    _gameTable = new SquaresTable(5);
                    break;
                case TableSize.Large:
                    _gameTable = new SquaresTable(9);
                    break;
            }
        }

        public void SetTableSize (TableSize tableSize)
        {
            if (tableSize == TableSize.Small)
                _gameTable.WPFSize = 7;
            else if (tableSize == TableSize.Medium)
                _gameTable.WPFSize = 11;
            else if (tableSize == TableSize.Large)
                _gameTable.WPFSize = 19;
            
            _tableSize = tableSize;
        }

        public void AddPoint ()
        {
            if (CurrentPlayer == 1)
            {
                P1Score++; 
                OnPointChanged();
            }
            else if (CurrentPlayer == 2)
            { 
                P2Score++; 
                OnPointChanged();
            }
        }

        public Int32 CheckCurrentPlayer ()
        {
            if (CurrentPlayer == 1)
                return 1;
            else if (CurrentPlayer == 2)
                return 2;

            return 0;
        }

        public void ChangeCurrentPlayer ()
        {
            if (CurrentPlayer == 1)
            {
                CurrentPlayer = 2;
                OnPlayerChanged();
            }
            else if (CurrentPlayer == 2)
            {
                CurrentPlayer = 1;
                OnPlayerChanged();
            }
        }

        public void IsOver()
        {
            if (P1Score > P2Score)
            {
                OnGameOver(1);
            }
            else if (P1Score < P2Score)
            {
                OnGameOver(2);
            }
            else
            {
                OnGameOver(0);
            }
        }

        public void SquareClick(Int32 x, Int32 y)
        {
            Int32 _currPlayer = CheckCurrentPlayer();
            if (!Table.IsSpaceFilled(x, y))
            {
                if (_currPlayer == 1)
                {
                    Table.SetTableValue(x, y, SquaresTable.Field.Player1);
                }
                else if (_currPlayer == 2)
                {
                    Table.SetTableValue(x, y, SquaresTable.Field.Player2);
                }
            }
        }

        public void CheckIfComplete()
        {
            Boolean _didChange = false;
            Int32 _currPlayer = CheckCurrentPlayer();

            for (int x = 0; x < Table.WPFSize; x++)
            {
                for (int y = 0; y < Table.WPFSize; y++)
                {
                    if (x % 2 == 1 && y % 2 == 1)
                    {
                        if (!Table.IsSpaceFilled(x,y))
                        {
                            if (Table.IsSpaceFilled(x - 1, y) && Table.IsSpaceFilled(x + 1, y) && Table.IsSpaceFilled(x, y - 1) && Table.IsSpaceFilled(x, y + 1))
                            {
                                _didChange = true;
                                if (_currPlayer == 1)
                                {
                                    Table.SetTableValue(x, y, SquaresTable.Field.Player1);
                                }
                                else if (_currPlayer == 2)
                                {
                                    Table.SetTableValue(x, y, SquaresTable.Field.Player2);
                                }
                                AddPoint();
                            }
                        }
                    }
                }
            }

            if (!_didChange)
                ChangeCurrentPlayer();
        }

        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _gameTable = await _dataAccess.LoadAsync(path, this);
        }

        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, _gameTable, CurrentPlayer, P1Score, P2Score);
        }

        private void OnPlayerChanged()
        {
            PlayerChanged?.Invoke(this, new SquaresEventArgs(_currentPlayer, _p1score, _p2score, false, 0));
        }

        private void OnPointChanged()
        {
            PointChanged?.Invoke(this, new SquaresEventArgs(_currentPlayer, _p1score, _p2score, false, 0));
        }
        private void OnGameOver(Int32 player)
        {
            GameOver?.Invoke(this, new SquaresEventArgs(_currentPlayer, _p1score, _p2score, true, player));
        }

    }
}
