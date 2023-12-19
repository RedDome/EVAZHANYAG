using System;
using System.Windows.Forms;
using ZHProject.Model;
using ZHProject.Persistence;

namespace ZHProject.WinForms
{
    public partial class ZHProjectView : Form
    {
        private IZHProjectDataAccess _dataAccess = null!;
        private ZHProjectModel _model = null!;
        private Button[,] _Fields = null!;
        private Button[,] _Shapes = null!;
        private Int32 _usedShape = 0;

        public ZHProjectView()
        {
            InitializeComponent();

            _dataAccess = new ZHProjectFileDataAccess();

            _model = new ZHProjectModel(_dataAccess);
            _model.PointChanged += new EventHandler<ZHProjectEventArgs>(Game_PointChanged);
            _model.GameOver += new EventHandler<ZHProjectEventArgs>(Game_GameOver);

            GenerateTable();
            SetupMenus();

            _model.NewGame();
            SetupShape();
        }

        private void ButtonGrid_MouseClick(Object? sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                Int32 x = (button.TabIndex - 100) / _model.Table.Size;
                Int32 y = (button.TabIndex - 100) % _model.Table.Size;

                bool helper = false;
                if (CheckIfFit(x, y))
                    helper = true;

                if (helper)
                {
                    CheckIfRowFull();
                    RefreshTable();
                    SetupShape();
                    _model.AddPoint();
                }
            }
        }



        private void _menuNewGame_Click(object sender, EventArgs e)
        {
            _menuSaveGame.Enabled = true;

            _model.NewGame();

            SetupText();
            GenerateTable();
            SetupMenus();
            SetupShape();
        }

        private void _menuExitGame_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "ZHProject", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Close();
        }

        private void _menuEasyGame_Click(object sender, EventArgs e)
        {
            _model.Difficulty = Difficulty.Easy;
        }

        private void _menuMediumGame_Click(object sender, EventArgs e)
        {
            _model.Difficulty = Difficulty.Medium;
        }

        private void _menuHardGame_Click(object sender, EventArgs e)
        {
            _model.Difficulty = Difficulty.Hard;
        }

        private async void _menuSaveGame_Click(object sender, EventArgs e)
        {
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.SaveGameAsync(_saveFileDialog.FileName);
                }
                catch (ZHProjectDataException)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void _menuLoadGame_Click(Object sender, EventArgs e)
        {
            // DestroyTable();
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.LoadGameAsync(_openFileDialog.FileName);
                    _menuSaveGame.Enabled = true;
                }
                catch (ZHProjectDataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _model.NewGame();
                    _menuSaveGame.Enabled = true;
                }

                GenerateTable();
                SetupText();
                SetupShape();
            }
        }

        private void Game_PointChanged(Object? sender, ZHProjectEventArgs e)
        {
            _labelPoints.Text = _model.Score.ToString();
        }

        private void Game_GameOver(Object? sender, ZHProjectEventArgs e)
        {
            _menuSaveGame.Enabled = false;

            MessageBox.Show("Game Is Over!" + " Your Score: " + e.Score,
                                "ZHProject",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
        }


        private void GenerateTable()
        {
            _Fields = new Button[_model.Table.Size, _model.Table.Size];
            for (Int32 i = 0; i < _model.Table.Size; i++)
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    _Fields[i, j] = new Button();
                    _Fields[i, j].Location = new Point(5 + 100 * j, 35 + 100 * i);
                    _Fields[i, j].Size = new Size(100, 100);
                    _Fields[i, j].Enabled = true;
                    _Fields[i, j].TabIndex = 100 + i * _model.Table.Size + j;
                    _Fields[i, j].FlatStyle = FlatStyle.Flat;
                    _Fields[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);

                    Controls.Add(_Fields[i, j]);
                }

            _Shapes = new Button[2, 2];
            for (Int32 i = 0; i < 2; i++)
                for (Int32 j = 0; j < 2; j++)
                {
                    _Shapes[i, j] = new Button();
                    _Shapes[i, j].Location = new Point(455 + 100 * j, 70 + 100 * i);
                    _Shapes[i, j].Size = new Size(100, 100);
                    _Shapes[i, j].Enabled = false;
                    _Shapes[i, j].TabIndex = 300 + i * _model.Table.Size + j;
                    _Shapes[i, j].FlatStyle = FlatStyle.Flat;

                    Controls.Add(_Shapes[i, j]);
                }
        }

        private void RefreshTable()
        {
            for (Int32 i = 0; i < _Fields.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _Fields.GetLength(1); j++)
                {
                    if (_model.Table.GetValue(i, j) == 0) // ha nincs zárolva a mezõ
                    {
                        _Fields[i, j].Enabled = true;
                        _Fields[i, j].BackColor = Color.White;
                    }
                    else // ha zárolva van
                    {
                        _Fields[i, j].Enabled = false;
                        _Fields[i, j].BackColor = Color.Blue;
                    }
                }
            }
        }

        private void SetupShape()
        {
            DestroyShape();

            Random rnd = new Random();
            int num = rnd.Next(1, 5);
            if (num == 1) 
            {
                _Shapes[0, 0].BackColor = Color.Blue;
                _Shapes[1, 0].BackColor = Color.Blue;
                _usedShape = 1;
            }
            else if (num == 2)
            {
                _Shapes[1, 0].BackColor = Color.Blue;
                _Shapes[1, 1].BackColor = Color.Blue;
                _usedShape = 2;
            }
            else if (num == 3)
            {
                _Shapes[0, 0].BackColor = Color.Blue;
                _Shapes[1, 0].BackColor = Color.Blue;
                _Shapes[1, 1].BackColor = Color.Blue;
                _usedShape = 3;
            }
            else if (num == 4)
            {
                _Shapes[0, 0].BackColor = Color.Blue;
                _Shapes[0, 1].BackColor = Color.Blue;
                _Shapes[1, 1].BackColor = Color.Blue;
                _usedShape = 4;
            }
            CheckIfPlayIsPossible();
        }


        private void CheckIfPlayIsPossible()
        {
            bool possible = false;
            if (_usedShape == 1)
            {
                for (Int32 x = 0; x < _model.Table.Size; x++)
                {
                    for (Int32 y = 0; y < _model.Table.Size; y++)
                    {
                        if (!_model.Table.CheckIfOutOfBounds(x, y) && !_model.Table.CheckIfOutOfBounds(x + 1, y) && _model.Table.IsNotUsed(x, y) && _model.Table.IsNotUsed(x + 1, y))
                        { 
                            possible = true;
                        }
                    }
                }

            }
            else if (_usedShape == 2)
            {
                for (Int32 x = 0; x < _model.Table.Size; x++)
                {
                    for (Int32 y = 0; y < _model.Table.Size; y++)
                    {
                        if (!_model.Table.CheckIfOutOfBounds(x, y) && !_model.Table.CheckIfOutOfBounds(x, y + 1) && _model.Table.IsNotUsed(x, y) && _model.Table.IsNotUsed(x, y + 1))
                        {
                            possible = true;
                        }
                    }
                }
            }
            else if (_usedShape == 3)
            {
                for (Int32 x = 0; x < _model.Table.Size; x++)
                {
                    for (Int32 y = 0; y < _model.Table.Size; y++)
                    {
                        if (!_model.Table.CheckIfOutOfBounds(x, y) && !_model.Table.CheckIfOutOfBounds(x + 1, y) && !_model.Table.CheckIfOutOfBounds(x + 1, y + 1) && _model.Table.IsNotUsed(x, y) && _model.Table.IsNotUsed(x + 1, y) && _model.Table.IsNotUsed(x + 1, y + 1))
                        {
                            possible = true;
                        }
                    }
                }
            }
            else if (_usedShape == 4)
            {
                for (Int32 x = 0; x < _model.Table.Size; x++)
                {
                    for (Int32 y = 0; y < _model.Table.Size; y++)
                    {
                        if (!_model.Table.CheckIfOutOfBounds(x, y) && !_model.Table.CheckIfOutOfBounds(x, y + 1) && !_model.Table.CheckIfOutOfBounds(x + 1, y + 1) && _model.Table.IsNotUsed(x, y) && _model.Table.IsNotUsed(x, y + 1) && _model.Table.IsNotUsed(x + 1, y + 1))
                        {
                            possible = true;
                        }
                    }
                }
            }

            if (!possible)
                _model.GameEnd();
        }

        private void CheckIfRowFull()
        {
            Int32[] toClear = { -1, -1, -1 };
            for (Int32 i = 0; i < _Fields.GetLength(0); i++)
            {
                bool isFull = true;
                for (Int32 j = 0; j < _Fields.GetLength(1); j++)
                {
                    if (_model.Table.GetValue(i, j) == 0)
                    {
                        isFull = false;
                    }
                }

                if (isFull)
                {
                    if (toClear[0] == -1)
                        toClear[0] = i;
                    else if (toClear[1] == -1)
                        toClear[1] = i;
                    else if (toClear[2] == -1)
                        toClear[2] = i;
                }
            }

            for (Int32 i = 0; i < _Fields.GetLength(0); i++)
            {
                bool isFull = true;
                for (Int32 j = 0; j < _Fields.GetLength(1); j++)
                {
                    if (_model.Table.GetValue(j, i) == 0)
                    {
                        isFull = false;
                    }
                }

                if (isFull)
                {
                    if (toClear[0] == -1)
                        toClear[0] = 4 + i;
                    else if (toClear[1] == -1)
                        toClear[1] = 4 + i;
                    else if (toClear[2] == -1)
                        toClear[2] = 4 + i;
                }
            }

            for (Int32 i = 0; i < 3; i++)
            {
                ClearLine(toClear[i]);
            }
        }

        private void DestroyShape()
        {
            _Shapes[0, 0].BackColor = Color.White;
            _Shapes[0, 1].BackColor = Color.White;
            _Shapes[1, 0].BackColor = Color.White;
            _Shapes[1, 1].BackColor = Color.White;
        }

        private void ClearLine(Int32 number)
        {
            if (number > -1 && number < 4)
            {
                _model.Table.SetValue(number, 0, 0);
                _model.Table.SetValue(number, 1, 0);
                _model.Table.SetValue(number, 2, 0);
                _model.Table.SetValue(number, 3, 0);
            }
            else if (number > 3 && number < 8)
            {
                _model.Table.SetValue(0, number - 4, 0);
                _model.Table.SetValue(1, number - 4, 0);
                _model.Table.SetValue(2, number - 4, 0);
                _model.Table.SetValue(3, number - 4, 0);
            }
        }

        private bool CheckIfFit(Int32 x, Int32 y)
        {
            if (_usedShape == 1)
            {
                if (_model.Table.CheckIfOutOfBounds(x, y) || _model.Table.CheckIfOutOfBounds(x + 1, y))
                    return false;

                if (_model.Table.IsNotUsed(x, y) && _model.Table.IsNotUsed(x + 1, y))
                {
                    _model.Table.SetValue(x, y, 1);
                    _model.Table.SetValue(x + 1, y, 1);
                    return true;
                }
                   
            }
            else if (_usedShape == 2)
            {
                if (_model.Table.CheckIfOutOfBounds(x, y) || _model.Table.CheckIfOutOfBounds(x, y + 1))
                    return false;

                if (_model.Table.IsNotUsed(x, y) && _model.Table.IsNotUsed(x, y + 1))
                {
                    _model.Table.SetValue(x, y, 1);
                    _model.Table.SetValue(x, y + 1, 1);
                    return true;
                }
            }
            else if (_usedShape == 3)
            {
                if (_model.Table.CheckIfOutOfBounds(x, y) || _model.Table.CheckIfOutOfBounds(x + 1, y) || _model.Table.CheckIfOutOfBounds(x + 1, y + 1))
                    return false;

                if (_model.Table.IsNotUsed(x, y) && _model.Table.IsNotUsed(x + 1, y) && _model.Table.IsNotUsed(x + 1, y + 1))
                {
                    _model.Table.SetValue(x, y, 1);
                    _model.Table.SetValue(x + 1, y, 1);
                    _model.Table.SetValue(x + 1, y + 1, 1);
                    return true;
                }
            }
            else if (_usedShape == 4)
            {
                if (_model.Table.CheckIfOutOfBounds(x, y) || _model.Table.CheckIfOutOfBounds(x, y + 1) || _model.Table.CheckIfOutOfBounds(x + 1, y + 1))
                    return false;

                if (_model.Table.IsNotUsed(x, y) && _model.Table.IsNotUsed(x, y + 1) && _model.Table.IsNotUsed(x + 1, y + 1))
                {
                    _model.Table.SetValue(x, y, 1);
                    _model.Table.SetValue(x, y + 1, 1);
                    _model.Table.SetValue(x + 1, y + 1, 1);
                    return true;
                }
            }

            return false;
        }

        private void SetupMenus()
        {
            _menuEasyGame.Checked = (_model.Difficulty == Difficulty.Easy);
            _menuMediumGame.Checked = (_model.Difficulty == Difficulty.Medium);
            _menuHardGame.Checked = (_model.Difficulty == Difficulty.Hard);
        }

        private void SetupText()
        {
            _labelPoints.Text = _model.Score.ToString();
        }
    }
}