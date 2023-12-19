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
            SetupTable();
        }

        private void ButtonGrid_MouseClick(Object? sender, MouseEventArgs e)
        {
            // TODO
        }



        private void _menuNewGame_Click(object sender, EventArgs e)
        {
            _menuSaveGame.Enabled = true;

            _model.NewGame();

            SetupText();
            GenerateTable();
            SetupTable();
            SetupMenus();
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
                SetupTable();
            }
        }

        private void Game_PointChanged(Object? sender, ZHProjectEventArgs e)
        {
            _labelPoints.Text = _model.Score.ToString();
        }

        private void Game_GameOver(Object? sender, ZHProjectEventArgs e)
        {
            _menuSaveGame.Enabled = false;

            MessageBox.Show("Game Is Over!",
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
                    _Fields[i, j].Enabled = false;
                    _Fields[i, j].TabIndex = 100 + i * _model.Table.Size + j;
                    _Fields[i, j].FlatStyle = FlatStyle.Flat;
                    _Fields[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);

                    Controls.Add(_Fields[i, j]);
                }
        }

        private void SetupTable()
        {
            // SZINVALTOZTATAS TODO
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