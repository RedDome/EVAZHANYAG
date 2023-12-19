using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using ZHProject.Model;
using ZHProject.Persistence;

namespace ZHProject.WPF.ViewModel
{
    public class ZHProjectViewModel : ViewModelBase
    {
        private ZHProjectModel _model;

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }

        public ObservableCollection<ZHProjectField> Fields { get; set; }

        public Int32 Score { get { return _model.Score; } }

        public Int32 GameSize { get { return _model.GameSize; } }

        public Boolean IsGameEasy
        {
            get { return _model.Difficulty == Difficulty.Easy; }
            set
            {
                if (_model.Difficulty == Difficulty.Easy)
                    return;

                _model.Difficulty = Difficulty.Easy;

                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }

        public Boolean IsGameMedium
        {
            get { return _model.Difficulty == Difficulty.Medium; }
            set
            {
                if (_model.Difficulty == Difficulty.Medium)
                    return;

                _model.Difficulty = Difficulty.Medium;

                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }

        public Boolean IsGameHard
        {
            get { return _model.Difficulty == Difficulty.Hard; }
            set
            {
                if (_model.Difficulty == Difficulty.Hard)
                    return;

                _model.Difficulty = Difficulty.Hard;

                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }

        public event EventHandler? NewGame;

        public event EventHandler? LoadGame;

        public event EventHandler? SaveGame;

        public event EventHandler? ExitGame;

        public ZHProjectViewModel(ZHProjectModel model)
        {
            _model = model;
            _model.PointChanged += new EventHandler<ZHProjectEventArgs>(Model_PointChanged);

            Fields = new ObservableCollection<ZHProjectField>();
            for (Int32 i = 0; i < _model.Table.Size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    Fields.Add(new ZHProjectField
                    {
                        X = i,
                        Y = j,
                        ClickCommand = new DelegateCommand(param =>
                        {
                            if (param is Tuple<Int32, Int32> position)
                                SquareClick(position.Item1, position.Item2);
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }

            RefreshMenu();

            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
        }

        private void Model_PointChanged(Object? sender, ZHProjectEventArgs e)
        {
            OnPropertyChanged(nameof(Score));
        }

        public void RefreshMenu()
        {
            OnPropertyChanged(nameof(Score));
        }

        private void SquareClick(Int32 x, Int32 y)
        {
            // TODO
        }

        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }
    }
}
