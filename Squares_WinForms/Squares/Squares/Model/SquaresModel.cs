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

        public void CheckIfOver ()
        {
            if (_gameTable.IsFilled)
            {
                if (P1Score > P2Score)
                {
                    OnGameOver(1);
                } 
                else if (P1Score < P2Score)
                {
                    OnGameOver(2);
                }
                OnGameOver(0);
            }
        }

        public void CheckIfComplete(Int32 x, Int32 y, Int32 version)
        {
            Boolean _didChange = false;
            Int32 _currPlayer = CheckCurrentPlayer();

            if (version == 3)
            {
                if (y == 0)
                {
                    if (Table.IsSpaceFilled(x, y + 1, 3) && Table.IsSpaceFilled(x, y, 2) && Table.IsSpaceFilled(x + 1, y, 2))
                    {
                        if (!Table.IsSpaceFilled(x, y, 1))
                        {
                            if (_currPlayer == 1)
                            {
                                Table.SetTableValue(x, y, SquaresTable.Field.Player1, 1);
                                _didChange = true;
                                AddPoint();
                            }
                            else if (_currPlayer == 2)
                            {
                                Table.SetTableValue(x, y, SquaresTable.Field.Player2, 1);
                                _didChange = true;
                                AddPoint();
                            }
                        }
                    }
                }
                else if (y == GameSize)
                {
                    if (Table.IsSpaceFilled(x, y - 1, 3) && Table.IsSpaceFilled(x, y - 1, 2) && Table.IsSpaceFilled(x + 1, y - 1, 2))
                    {
                        if (!Table.IsSpaceFilled(x, y - 1, 1))
                        {
                            if (_currPlayer == 1)
                            {
                                Table.SetTableValue(x, y - 1, SquaresTable.Field.Player1, 1);
                                _didChange = true;
                                AddPoint();
                            }
                            else if (_currPlayer == 2)
                            {
                                Table.SetTableValue(x, y - 1, SquaresTable.Field.Player2, 1);
                                _didChange = true;
                                AddPoint();
                            }
                        }
                    }
                }
                else
                {
                    if (Table.IsSpaceFilled(x, y - 1, 3) && Table.IsSpaceFilled(x, y - 1, 2) && Table.IsSpaceFilled(x + 1, y - 1, 2))
                    {
                        if (!Table.IsSpaceFilled(x, y - 1, 1))
                        {
                            if (_currPlayer == 1)
                            {
                                Table.SetTableValue(x, y - 1, SquaresTable.Field.Player1, 1);
                                _didChange = true;
                                AddPoint();
                            }
                            else if (_currPlayer == 2)
                            {
                                Table.SetTableValue(x, y - 1, SquaresTable.Field.Player2, 1);
                                _didChange = true;
                                AddPoint();
                            }
                        }
                    }

                    if (Table.IsSpaceFilled(x, y + 1, 3) && Table.IsSpaceFilled(x, y, 2) && Table.IsSpaceFilled(x + 1, y, 2))
                    {
                        if (!Table.IsSpaceFilled(x, y, 1))
                        {
                            if (_currPlayer == 1)
                            {
                                Table.SetTableValue(x, y, SquaresTable.Field.Player1, 1);
                                _didChange = true;
                                AddPoint();
                            }
                            else if (_currPlayer == 2)
                            {
                                Table.SetTableValue(x, y, SquaresTable.Field.Player2, 1);
                                _didChange = true;
                                AddPoint();
                            }
                        }
                    }
                }
            }
            else if (version == 2)
            {
                if (x == 0)
                {
                    if (Table.IsSpaceFilled(x + 1, y, 2) && Table.IsSpaceFilled(x, y, 3) && Table.IsSpaceFilled(x, y + 1, 3))
                    {
                        if (!Table.IsSpaceFilled(x, y, 1))
                        {
                            if (_currPlayer == 1)
                            {
                                Table.SetTableValue(x, y, SquaresTable.Field.Player1, 1);
                                _didChange = true;
                                AddPoint();
                            }
                            else if (_currPlayer == 2)
                            {
                                Table.SetTableValue(x, y, SquaresTable.Field.Player2, 1);
                                _didChange = true;
                                AddPoint();
                            }
                        }
                    }
                }
                else if (x == GameSize)
                {
                    if (Table.IsSpaceFilled(x - 1, y, 2) && Table.IsSpaceFilled(x - 1, y, 3) && Table.IsSpaceFilled(x - 1, y + 1, 3))
                    {
                        if (!Table.IsSpaceFilled(x - 1, y, 1))
                        {
                            if (_currPlayer == 1)
                            {
                                Table.SetTableValue(x - 1, y, SquaresTable.Field.Player1, 1);
                                _didChange = true;
                                AddPoint();
                            }
                            else if (_currPlayer == 2)
                            {
                                Table.SetTableValue(x - 1, y, SquaresTable.Field.Player2, 1);
                                _didChange = true;
                                AddPoint();
                            }
                        }
                    }
                }
                else
                {
                    if (Table.IsSpaceFilled(x - 1, y, 2) && Table.IsSpaceFilled(x - 1, y, 3) && Table.IsSpaceFilled(x - 1, y + 1, 3))
                    {
                        if (!Table.IsSpaceFilled(x - 1, y, 1))
                        {
                            if (_currPlayer == 1)
                            {
                                Table.SetTableValue(x - 1, y, SquaresTable.Field.Player1, 1);
                                _didChange = true;
                                AddPoint();
                            }
                            else if (_currPlayer == 2)
                            {
                                Table.SetTableValue(x - 1, y, SquaresTable.Field.Player2, 1);
                                _didChange = true;
                                AddPoint();
                            }
                        }
                    }

                    if (Table.IsSpaceFilled(x + 1, y, 2) && Table.IsSpaceFilled(x, y, 3) && Table.IsSpaceFilled(x, y + 1, 3))
                    {
                        if (!Table.IsSpaceFilled(x, y, 1))
                        {
                            if (_currPlayer == 1)
                            {
                                Table.SetTableValue(x, y, SquaresTable.Field.Player1, 1);
                                _didChange = true;
                                AddPoint();
                            }
                            else if (_currPlayer == 2)
                            {
                                Table.SetTableValue(x, y, SquaresTable.Field.Player2, 1);
                                _didChange = true;
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
