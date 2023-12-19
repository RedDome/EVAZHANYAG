using Squares.Model;
using static Squares.Persistence.SquaresTable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquaresMAUI.ViewModel
{
    public class SquaresViewModel : ViewModelBase
    {
        private SquaresModel _model;
        private Int32 _tableSize;
        private TableSizeViewModel _size = null!;

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }

        public ObservableCollection<SquaresField> Squares { get; set; }

        public ObservableCollection<TableSizeViewModel> TableSizes { get; set; }

        public Int32 CurrentPlayer { get { return _model.CurrentPlayer; } }

        public Int32 P1Score { get { return _model.P1Score; } }

        public Int32 P2Score { get { return _model.P2Score; } }

        public Int32 TableSizeView { get { return _model.WPFGameSize; } }

        public Int32 SquareCount { get { return _model.GameSize; } }

        public TableSizeViewModel Size
        {
            get => _size;
            set
            {
                _size = value;
                _model.TableSize = value.Size;
                OnPropertyChanged();
            }
        }

        public int TableSizeMAUI
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

        public RowDefinitionCollection GameTableRows
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), TableSizeMAUI).ToArray());
        }

        public ColumnDefinitionCollection GameTableColumns
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), TableSizeMAUI).ToArray());
        }

        public event EventHandler? NewGame;

        public event EventHandler? LoadGame;

        public event EventHandler? SaveGame;

        public event EventHandler? ExitGame;

        public SquaresViewModel(SquaresModel model)
        {
            _model = model;
            _model.PlayerChanged += new EventHandler<SquaresEventArgs>(Model_PlayerChanged);
            _model.PointChanged += new EventHandler<SquaresEventArgs>(Model_PointChanged);

            TableSizes = new ObservableCollection<TableSizeViewModel>
            {
                new TableSizeViewModel { Size = TableSize.Small },
                new TableSizeViewModel { Size = TableSize.Medium },
                new TableSizeViewModel { Size = TableSize.Large }
            };
            Size = TableSizes[1];

            TableSizeMAUI = _model.Table.WPFSize;

            // UpdateView();

            Squares = new ObservableCollection<SquaresField>();
            GenerateTable();
            RefreshTable();

            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
        }

        public void GenerateTable()
        {
            for (Int32 i = 0; i < TableSizeView; i++)
            {
                for (Int32 j = 0; j < TableSizeView; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            Squares.Add(new SquaresField
                            {
                                Usage = -1,
                                X = i,
                                Y = j
                            });
                        }
                        else
                        {
                            Squares.Add(new SquaresField
                            {
                                Usage = 0,
                                X = i,
                                Y = j,
                                ClickCommand = new DelegateCommand(param =>
                                {
                                    if (param is Tuple<Int32, Int32> position)
                                        SquareClick(position.Item1, position.Item2);
                                })
                            });
                        }
                    }
                    else
                    {
                        if (j % 2 == 1)
                        {
                            Squares.Add(new SquaresField
                            {
                                Usage = 0,
                                X = i,
                                Y = j,
                                // TODO szedd ki
                                /*
                                ClickCommand = new DelegateCommand(param =>
                                {
                                    if (param is Tuple<Int32, Int32> position)
                                        SquareClick(position.Item1, position.Item2);
                                })
                                */
                            });
                        }
                        else
                        {
                            Squares.Add(new SquaresField
                            {
                                Usage = 0,
                                X = i,
                                Y = j,
                                ClickCommand = new DelegateCommand(param =>
                                {
                                    if (param is Tuple<Int32, Int32> position)
                                        SquareClick(position.Item1, position.Item2);
                                })
                            });
                        }
                    }
                }
            }
        }

        public void DeleteTable()
        {
            Squares.Clear();
        }

        public void UpdateView()
        {
            TableSizeMAUI = _model.Table.WPFSize;
            // OnPropertyChanged(nameof(GameTableRows));
            // OnPropertyChanged(nameof(GameTableColumns));
        }

        private void Model_PlayerChanged(Object? sender, SquaresEventArgs e)
        {
            OnPropertyChanged(nameof(CurrentPlayer));
        }

        private void Model_PointChanged(Object? sender, SquaresEventArgs e)
        {
            OnPropertyChanged(nameof(P1Score));
            OnPropertyChanged(nameof(P2Score));
        }

        public void RefreshMenu()
        {
            OnPropertyChanged(nameof(CurrentPlayer));
            OnPropertyChanged(nameof(P1Score));
            OnPropertyChanged(nameof(P2Score));
        }

        public void RefreshTable()
        {
            SynchronizeTable();

            CheckIfOver();
        }

        private void SquareClick(Int32 x, Int32 y)
        {
            _model.SquareClick(x, y);
            _model.CheckIfComplete();
            RefreshTable();
        }

        private void SynchronizeTable()
        {
            foreach (SquaresField field in Squares)
            {
                if (_model.Table.GetTableValue(field.X, field.Y) == Field.Player1)
                    field.Usage = 1;

                if (_model.Table.GetTableValue(field.X, field.Y) == Field.Player2)
                    field.Usage = 2;
            }
        }

        private void CheckIfOver()
        {
            if (_model.Table.IsFilled)
                _model.IsOver();
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
