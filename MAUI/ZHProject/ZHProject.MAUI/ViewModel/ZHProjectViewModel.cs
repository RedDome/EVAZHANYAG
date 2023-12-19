using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHProject.Model;

namespace ZHProject.MAUI.ViewModel
{
    public class ZHProjectViewModel : ViewModelBase
    {
        private ZHProjectModel _model;
        private Int32 _tableSize;
        private GameDifficultyViewModel _difficulty = null!;

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }

        public ObservableCollection<ZHProjectField> Squares { get; set; }

        public ObservableCollection<GameDifficultyViewModel> DifficultyLevels { get; set; }

        public Int32 Score { get { return _model.Score; } }

        public Int32 ModelSize { get { return _model.GameSize; } }

        public GameDifficultyViewModel GameDifficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                _model.Difficulty = value.GameDifficulty;
                OnPropertyChanged();
            }
        }

        public Int32 Size
        {
            get => _tableSize;
            set
            {
                _tableSize = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GameTableRows));
                OnPropertyChanged(nameof(GameTableColumns));
            }
        }

        // Resizable Rows, Columns -> MainPage.xaml 19,20. sor {Binding GameTableRows} {Binding GameTableColumns}
         
        public RowDefinitionCollection GameTableRows
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), Size).ToArray());
        }

        public ColumnDefinitionCollection GameTableColumns
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), Size).ToArray());
        }

        public event EventHandler? NewGame;

        public event EventHandler? LoadGame;

        public event EventHandler? SaveGame;

        public event EventHandler? ExitGame;

        public ZHProjectViewModel(ZHProjectModel model)
        {
            _model = model;
            _model.PointChanged += new EventHandler<ZHProjectEventArgs>(Model_PointChanged);

            DifficultyLevels = new ObservableCollection<GameDifficultyViewModel>
            {
                new GameDifficultyViewModel { GameDifficulty = Difficulty.Easy },
                new GameDifficultyViewModel { GameDifficulty = Difficulty.Medium },
                new GameDifficultyViewModel { GameDifficulty = Difficulty.Hard }
            };
            GameDifficulty = DifficultyLevels[1]; // medium

            Size = _model.Table.Size;

            Squares = new ObservableCollection<ZHProjectField>();
            for (Int32 i = 0; i < _model.Table.Size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    Squares.Add(new ZHProjectField
                    {
                        X = i,
                        Y = j,
                        ClickCommand = new DelegateCommand(param =>
                        {
                            if (param is Tuple<Int32, Int32> position)
                                SquaresClick(position.Item1, position.Item2);
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }
            RefreshTable();

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

        public void RefreshTable()
        {

        }

        private void SquaresClick(Int32 x, Int32 y)
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
