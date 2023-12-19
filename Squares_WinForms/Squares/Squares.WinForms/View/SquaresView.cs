using Squares.Persistence;
using Squares.Model;
using System.Windows.Forms;
using System.Numerics;
using System.DirectoryServices.ActiveDirectory;

namespace Squares.WinForms
{
    public partial class SquaresView : Form
    {
        private ISquaresDataAccess _dataAccess = null!;
        private SquaresModel _model = null!;
        private Button[,] _SquareBoxes = null!;
        private Button[,] _VerBoxes = null!;
        private Button[,] _HorBoxes = null!;

        public SquaresView()
        {
            InitializeComponent();

            _dataAccess = new SquaresFileDataAccess();

            _model = new SquaresModel(_dataAccess);
            _model.PlayerChanged += new EventHandler<SquaresEventArgs>(Game_PlayerChanged);
            _model.PointChanged += new EventHandler<SquaresEventArgs>(Game_PointChanged);
            _model.GameOver += new EventHandler<SquaresEventArgs>(Game_GameOver);

            GenerateTable();
            SetupMenus();

            _model.NewGame();
            SetupTable();
        }

        private void ButtonGrid_MouseClick(Object? sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                Int32 _currPlayer = _model.CheckCurrentPlayer();
                if (button.TabIndex > 199)
                {
                    Int32 x = (button.TabIndex - 200) / (_model.Table.Size + 1);
                    Int32 y = (button.TabIndex - 200) % (_model.Table.Size + 1);

                    if (_HorBoxes[x, y].BackColor != Color.Red && _HorBoxes[x, y].BackColor != Color.Blue)
                    {
                        if (_currPlayer == 1)
                        {
                            _model.Table.SetTableValue(x, y, SquaresTable.Field.Player1, 3);
                        }
                        else if (_currPlayer == 2)
                        {
                            _model.Table.SetTableValue(x, y, SquaresTable.Field.Player2, 3);
                        }
                        _model.CheckIfComplete(x, y, 3);
                    }
                }
                else
                {
                    Int32 x = (button.TabIndex - 100) / _model.Table.Size;
                    Int32 y = (button.TabIndex - 100) % _model.Table.Size;

                    if (_VerBoxes[x, y].BackColor != Color.Red && _VerBoxes[x, y].BackColor != Color.Blue)
                    {
                        if (_currPlayer == 1)
                        {
                            _model.Table.SetTableValue(x, y, SquaresTable.Field.Player1, 2);
                        }
                        else if (_currPlayer == 2)
                        {
                            _model.Table.SetTableValue(x, y, SquaresTable.Field.Player2, 2);
                        }
                        _model.CheckIfComplete(x, y, 2);
                    }
                }
            }

            SetupTable();

            _model.CheckIfOver();
        }

        

        private void _menuNewGameButton_Click(object sender, EventArgs e)
        {
            _menuSaveGameButton.Enabled = true;

            DestroyTable();
            _model.NewGame();

            SetupText();
            GenerateTable();
            SetupTable();
            SetupMenus();
        }

        private void _menuExitGameButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Squares", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Close();
        }

        private void _menuEasyButton_Click(object sender, EventArgs e)
        {
            _model.TableSize = TableSize.Small;
        }

        private void _menuMediumButton_Click(object sender, EventArgs e)
        {
            _model.TableSize = TableSize.Medium;
        }

        private void _menuHardButton_Click(object sender, EventArgs e)
        {
            _model.TableSize = TableSize.Large;
        }

        private async void _menuSaveGameButton_Click(object sender, EventArgs e)
        {
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.SaveGameAsync(_saveFileDialog.FileName);
                }
                catch (SquaresDataExpection)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void _menuLoadGameButton_Click(Object sender, EventArgs e)
        {
            DestroyTable();
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.LoadGameAsync(_openFileDialog.FileName);
                    _menuSaveGameButton.Enabled = true;
                }
                catch (SquaresDataExpection)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _model.NewGame();
                    _menuSaveGameButton.Enabled = true;
                }

                GenerateTable();
                SetupText();
                SetupTable();
            }
        }

        private void Game_PlayerChanged(Object? sender, SquaresEventArgs e)
        {
            _currPlayerText.Text = ("Player ") + _model.CurrentPlayer.ToString();
        }

        private void Game_PointChanged(Object? sender, SquaresEventArgs e)
        {
            _p1Points.Text = _model.P1Score.ToString();
            _p2Points.Text = _model.P2Score.ToString();
        }

        private void Game_GameOver(Object? sender, SquaresEventArgs e)
        {
            _menuSaveGameButton.Enabled = false;

            if (e.Winner == 1)
            {
                MessageBox.Show("First Player Won!",
                                "Squares",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
            else if (e.Winner == 2)
            {
                MessageBox.Show("Second Player Won!",
                                "Squares",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
            else if (e.P1Score == e.P2Score)
            {
                MessageBox.Show("Draw!",
                                "Squares",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
        }


        private void GenerateTable()
        {
            _SquareBoxes = new Button[_model.Table.Size, _model.Table.Size];
            int size = 0;
            int size2 = 0;
            int gap = 0;

            if (_model.Table.Size == 9)
            {
                size = 45;
                size2 = 10;
                gap = 5;
            }
            else if (_model.Table.Size == 5)
            {
                size = 73;
                size2 = 20;
                gap = 10;
            }
            else if (_model.Table.Size == 3)
            {
                size = 125;
                size2 = 30;
                gap = 15;
            }

            for (Int32 i = 0; i < _model.Table.Size; i++)
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    _SquareBoxes[i, j] = new Button();
                    _SquareBoxes[i, j].Location = new Point((10 + size2 + gap) + ((size + size2 + 2 * gap) * j), (30 + size2 + gap) + ((size + size2 + 2 * gap) * i));
                    _SquareBoxes[i, j].Size = new Size(size, size);
                    _SquareBoxes[i, j].Enabled = false;
                    _SquareBoxes[i, j].TabIndex = 100 + i * _model.Table.Size + j;
                    _SquareBoxes[i, j].FlatStyle = FlatStyle.Flat;

                    Controls.Add(_SquareBoxes[i, j]);
                }

            _VerBoxes = new Button[_model.Table.Size + 1, _model.Table.Size];
            for (Int32 i = 0; i < _model.Table.Size + 1; i++)
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    _VerBoxes[i, j] = new Button();
                    _VerBoxes[i, j].Location = new Point((10 + size2 + gap) + ((size + size2 + 2 * gap) * j), 30 + ((size2 + size + 2 * gap) * i));
                    _VerBoxes[i, j].Size = new Size(size, size2);
                    _VerBoxes[i, j].Enabled = true;
                    _VerBoxes[i, j].TabIndex = 100 + i * _model.Table.Size + j;
                    _VerBoxes[i, j].FlatStyle = FlatStyle.Flat;

                    _VerBoxes[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    Controls.Add(_VerBoxes[i, j]);
                }

            _HorBoxes = new Button[_model.Table.Size, _model.Table.Size + 1];
            for (Int32 i = 0; i < _model.Table.Size; i++)
                for (Int32 j = 0; j < _model.Table.Size + 1; j++)
                {
                    _HorBoxes[i, j] = new Button();
                    _HorBoxes[i, j].Location = new Point(10 + ((size + size2 + 2 * gap) * j), (30 + size2 + gap) + ((size2 + size + 2 * gap) * i));
                    _HorBoxes[i, j].Size = new Size(size2, size);
                    _HorBoxes[i, j].Enabled = true;
                    _HorBoxes[i, j].TabIndex = 200 + i * (_model.Table.Size + 1) + j;
                    _HorBoxes[i, j].FlatStyle = FlatStyle.Flat;

                    _HorBoxes[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    Controls.Add(_HorBoxes[i, j]);
                }
        }

        private void SetupTable()
        {
            for (Int32 x = 0; x < _SquareBoxes.GetLength(0); x++)
            {
                for (Int32 y = 0; y < _SquareBoxes.GetLength(1); y++)
                {
                    if (_model.Table.GetTableValue(x, y, 1) == SquaresTable.Field.Empty)
                        _SquareBoxes[x, y].BackColor = Color.White;
                    else if (_model.Table.GetTableValue(x, y, 1) == SquaresTable.Field.Player1)
                        _SquareBoxes[x, y].BackColor = Color.Red;
                    else if (_model.Table.GetTableValue(x, y, 1) == SquaresTable.Field.Player2)
                        _SquareBoxes[x, y].BackColor = Color.Blue;
                }
            }

            for (Int32 x = 0; x < _VerBoxes.GetLength(0); x++)
            {
                for (Int32 y = 0; y < _VerBoxes.GetLength(1); y++)
                {
                    if (_model.Table.GetTableValue(x, y, 2) == SquaresTable.Field.Empty)
                        _VerBoxes[x, y].BackColor = Color.White;
                    else if (_model.Table.GetTableValue(x, y, 2) == SquaresTable.Field.Player1)
                        _VerBoxes[x, y].BackColor = Color.Red;
                    else if (_model.Table.GetTableValue(x, y, 2) == SquaresTable.Field.Player2)
                        _VerBoxes[x, y].BackColor = Color.Blue;

                }
            }

            for (Int32 x = 0; x < _HorBoxes.GetLength(0); x++)
            {
                for (Int32 y = 0; y < _HorBoxes.GetLength(1); y++)
                {
                    if (_model.Table.GetTableValue(x, y, 3) == SquaresTable.Field.Empty)
                        _HorBoxes[x, y].BackColor = Color.White;
                    else if (_model.Table.GetTableValue(x, y, 3) == SquaresTable.Field.Player1)
                        _HorBoxes[x, y].BackColor = Color.Red;
                    else if (_model.Table.GetTableValue(x, y, 3) == SquaresTable.Field.Player2)
                        _HorBoxes[x, y].BackColor = Color.Blue;
                }
            }
        }

        private void SetupMenus()
        {
            _menuEasyButton.Checked = (_model.GameSize == 3);
            _menuMediumButton.Checked = (_model.GameSize == 5);
            _menuHardButton.Checked = (_model.GameSize == 9);
        }

        private void SetupText()
        {
            _currPlayerText.Text = ("Player ") + _model.CurrentPlayer.ToString();
            _p1Points.Text = _model.P1Score.ToString();
            _p2Points.Text = _model.P2Score.ToString();
        }

        private void DestroyTable()
        {
            for (Int32 i = 0; i < _model.Table.Size; i++)
            {
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    if (_SquareBoxes[i, j] != null)
                        _SquareBoxes[i, j].Dispose();
                }
            }

            for (Int32 i = 0; i < _model.Table.Size + 1; i++)
            {
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    if (_VerBoxes[i, j] != null)
                        _VerBoxes[i, j].Dispose();
                }
            }

            for (Int32 i = 0; i < _model.Table.Size; i++)
            {
                for (Int32 j = 0; j < _model.Table.Size + 1; j++)
                {
                    if (_HorBoxes[i, j] != null)
                        _HorBoxes[i, j].Dispose();
                }
            }
        }
    }
}