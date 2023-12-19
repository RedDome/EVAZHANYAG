using System;
using System.Collections.ObjectModel;
using ELTE.Sudoku.Model;

namespace ELTE.Sudoku.ViewModel
{
    /// <summary>
    /// Sudoku nézetmodell típusa.
    /// </summary>
    public class SudokuViewModel : ViewModelBase
    {
        #region Fields
        
        private SudokuGameModel _model; // modell
        private Int32 _tableSize;
        private GameDifficultyViewModel _difficulty = null!; // konstruktor propertyn keresztül inicializálja

        #endregion

        #region Properties

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<SudokuField> Fields { get; set; }

        /// <summary>
        /// Nehézségi szintek.
        /// </summary>
        public ObservableCollection<GameDifficultyViewModel> DifficultyLevels { get; set; }

        /// <summary>
        /// Lépések számának lekérdezése.
        /// </summary>
        public Int32 GameStepCount { get { return _model.GameStepCount; } }

        /// <summary>
        /// Fennmaradt játékidő lekérdezése.
        /// </summary>
        public String GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }

        /// <summary>
        /// Nehézségi szint beállítás.
        /// </summary>
        public GameDifficultyViewModel Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                _model.GameDifficulty = value.Difficulty;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A tábla méretezéséhez használt property
        /// </summary>
        public int TableSize
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

        /// <summary>
        /// Segédproperty a tábla méretezéséhez
        /// </summary>
        public RowDefinitionCollection GameTableRows
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), TableSize).ToArray());
        }


        /// <summary>
        /// Segédproperty a tábla méretezéséhez
        /// </summary>
        public ColumnDefinitionCollection GameTableColumns
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), TableSize).ToArray());
        }

        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler? NewGame;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler? LoadGame;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler? SaveGame;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler? ExitGame;

        #endregion

        #region Constructors

        /// <summary>
        /// Sudoku nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public SudokuViewModel(SudokuGameModel model)
        {
            // játék csatlakoztatása
            _model = model;
            _model.FieldChanged += new EventHandler<SudokuFieldEventArgs>(Model_FieldChanged);
            _model.GameAdvanced += new EventHandler<SudokuEventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<SudokuEventArgs>(Model_GameOver);
			_model.GameCreated += new EventHandler<SudokuEventArgs>(Model_GameCreated);

            // parancsok kezelése
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());

            // nehézségi szintek
            DifficultyLevels = new ObservableCollection<GameDifficultyViewModel>
            {
                new GameDifficultyViewModel { Difficulty = GameDifficulty.Easy },
                new GameDifficultyViewModel { Difficulty = GameDifficulty.Medium },
                new GameDifficultyViewModel { Difficulty = GameDifficulty.Hard }
            };
            Difficulty = DifficultyLevels[1]; // medium

            // tábla méret
            TableSize = _model.Table.Size;

            // játéktábla létrehozása
            Fields = new ObservableCollection<SudokuField>();
            for (Int32 i = 0; i < _model.Table.Size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    Fields.Add(new SudokuField
                    {
                        IsLocked = true,
                        Text = String.Empty,
                        X = i,
                        Y = j,
                        StepCommand = new DelegateCommand(param =>
                        {
                            if (param is Tuple<Int32, Int32> position)
                                StepGame(position.Item1, position.Item2);
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }

            RefreshTable();
        }

		#endregion

		#region Private methods

		/// <summary>
		/// Tábla frissítése.
		/// </summary>
		private void RefreshTable()
        {
            foreach (SudokuField field in Fields) // inicializálni kell a mezőket is
            {
                field.Text = !_model.Table.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty;
                field.IsLocked = _model.Table.IsLocked(field.X, field.Y);
            }

            OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(GameStepCount));
        }

        /// <summary>
        /// Játék léptetése eseménykiváltása.
        /// </summary>
        /// <param name="x">A lépett mező X koordinátája.</param>
        /// <param name="y">A lépett mező Y koordinátája.</param>
        private void StepGame(Int32 x, Int32 y)
        {
            _model.Step(x, y);
        }

        #endregion

        #region Game event handlers

        /// <summary>
        /// Játékmodell mező megváltozásának eseménykezelője.
        /// </summary>
        private void Model_FieldChanged(object? sender, SudokuFieldEventArgs e)
        {
            // mező frissítése
            SudokuField field = Fields.Single(f => f.X == e.X && f.Y == e.Y);

            field.Text = !_model.Table.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty; // visszaírjuk a szöveget
            OnPropertyChanged(nameof(GameStepCount)); // jelezzük a lépésszám változást
        }

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object? sender, SudokuEventArgs e)
        {
            foreach (SudokuField field in Fields)
            {
                field.IsLocked = true; // minden mezőt lezárunk
            }
        }

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        private void Model_GameAdvanced(object? sender, SudokuEventArgs e)
        {
            OnPropertyChanged(nameof(GameTime));
        }

	    /// <summary>
	    /// Játék létrehozásának eseménykezelője.
	    /// </summary>
		private void Model_GameCreated(object? sender, SudokuEventArgs e)
	    {
		    RefreshTable();
	    }

		#endregion

		#region Event methods

		/// <summary>
		/// Új játék indításának eseménykiváltása.
		/// </summary>
		private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        

        /// <summary>
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játékból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
