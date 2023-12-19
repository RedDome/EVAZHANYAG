using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using ZHProject.Persistence;

namespace ZHProject.Model
{
    public enum Difficulty { Easy, Medium, Hard }
    public class ZHProjectModel
    {
        private IZHProjectDataAccess _dataAccess = null!;
        private Difficulty _difficulty;
        private ZHProjectTable _gameTable;
        private Int32 _score;

        public Int32 GameSize
        {
            get { return _gameTable.Size; }
            set { _gameTable.Size = value; }
        }

        public Int32 Score { get { return _score; } set { _score = value; } }

        public ZHProjectTable Table { get { return _gameTable; } }

        public Difficulty Difficulty { get { return _difficulty; } set { _difficulty = value; } }

        // public Boolean IsGameOver { get { return _gameTable.IsFilled; } }

        public event EventHandler<ZHProjectEventArgs>? PointChanged;

        public event EventHandler<ZHProjectEventArgs>? GameOver;

        public ZHProjectModel(IZHProjectDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _gameTable = new ZHProjectTable();
            _difficulty = Difficulty.Medium;
        }

        public void NewGame()
        {
            _gameTable = new ZHProjectTable();

            _score = 0;

            // TODO
            /*
            switch (_difficulty)
            {
                case Difficulty.Easy:
                    // _gameTable = new SquaresTable(3);
                    break;
                case Difficulty.Medium:
                    // _gameTable = new SquaresTable(5);
                    break;
                case Difficulty.Hard:
                    // _gameTable = new SquaresTable(9);
                    break;
            }
            */
        }

        public void AddPoint()
        {
            Score++;
            OnPointChanged();
        }

        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _gameTable = await _dataAccess.LoadAsync(path);
        }

        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, _gameTable);
        }

        private void OnPointChanged()
        {
            PointChanged?.Invoke(this, new ZHProjectEventArgs(_score, false));
        }
        private void OnGameOver(Int32 player)
        {
            GameOver?.Invoke(this, new ZHProjectEventArgs(_score, false));
        }
    }
}
