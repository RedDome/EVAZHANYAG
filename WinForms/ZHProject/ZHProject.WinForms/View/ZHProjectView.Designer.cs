namespace ZHProject.WinForms
{
    partial class ZHProjectView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            _menuFile = new ToolStripMenuItem();
            _menuNewGame = new ToolStripMenuItem();
            _menuLoadGame = new ToolStripMenuItem();
            _menuSaveGame = new ToolStripMenuItem();
            _menuExitGame = new ToolStripMenuItem();
            _menuSettings = new ToolStripMenuItem();
            _menuEasyGame = new ToolStripMenuItem();
            _menuMediumGame = new ToolStripMenuItem();
            _menuHardGame = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            _labelPointText = new ToolStripStatusLabel();
            _labelPoints = new ToolStripStatusLabel();
            _openFileDialog = new OpenFileDialog();
            _saveFileDialog = new SaveFileDialog();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { _menuFile, _menuSettings });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(514, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // _menuFile
            // 
            _menuFile.DropDownItems.AddRange(new ToolStripItem[] { _menuNewGame, _menuLoadGame, _menuSaveGame, _menuExitGame });
            _menuFile.Name = "_menuFile";
            _menuFile.Size = new Size(37, 20);
            _menuFile.Text = "File";
            // 
            // _menuNewGame
            // 
            _menuNewGame.Name = "_menuNewGame";
            _menuNewGame.Size = new Size(134, 22);
            _menuNewGame.Text = "New Game";
            _menuNewGame.Click += _menuNewGame_Click;
            // 
            // _menuLoadGame
            // 
            _menuLoadGame.Name = "_menuLoadGame";
            _menuLoadGame.Size = new Size(134, 22);
            _menuLoadGame.Text = "Load Game";
            _menuLoadGame.Click += _menuLoadGame_Click;
            // 
            // _menuSaveGame
            // 
            _menuSaveGame.Name = "_menuSaveGame";
            _menuSaveGame.Size = new Size(134, 22);
            _menuSaveGame.Text = "Save Game";
            _menuSaveGame.Click += _menuSaveGame_Click;
            // 
            // _menuExitGame
            // 
            _menuExitGame.Name = "_menuExitGame";
            _menuExitGame.Size = new Size(134, 22);
            _menuExitGame.Text = "Exit Game";
            _menuExitGame.Click += _menuExitGame_Click;
            // 
            // _menuSettings
            // 
            _menuSettings.DropDownItems.AddRange(new ToolStripItem[] { _menuEasyGame, _menuMediumGame, _menuHardGame });
            _menuSettings.Name = "_menuSettings";
            _menuSettings.Size = new Size(61, 20);
            _menuSettings.Text = "Settings";
            // 
            // _menuEasyGame
            // 
            _menuEasyGame.Name = "_menuEasyGame";
            _menuEasyGame.Size = new Size(153, 22);
            _menuEasyGame.Text = "Easy Game";
            _menuEasyGame.Click += _menuEasyGame_Click;
            // 
            // _menuMediumGame
            // 
            _menuMediumGame.Name = "_menuMediumGame";
            _menuMediumGame.Size = new Size(153, 22);
            _menuMediumGame.Text = "Medium Game";
            _menuMediumGame.Click += _menuMediumGame_Click;
            // 
            // _menuHardGame
            // 
            _menuHardGame.Name = "_menuHardGame";
            _menuHardGame.Size = new Size(153, 22);
            _menuHardGame.Text = "Hard Game";
            _menuHardGame.Click += _menuHardGame_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { _labelPointText, _labelPoints });
            statusStrip1.Location = new Point(0, 539);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(514, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // _labelPointText
            // 
            _labelPointText.Name = "_labelPointText";
            _labelPointText.Size = new Size(40, 17);
            _labelPointText.Text = "Points";
            // 
            // _labelPoints
            // 
            _labelPoints.Name = "_labelPoints";
            _labelPoints.Size = new Size(13, 17);
            _labelPoints.Text = "0";
            // 
            // _openFileDialog
            // 
            _openFileDialog.Filter = "ZHProject tábla (*.zhp)|*.zhp";
            _openFileDialog.Title = "ZHProject Game Load";
            // 
            // _saveFileDialog
            // 
            _saveFileDialog.Filter = "ZHProject tábla (*.zhp)|*.zhp";
            _saveFileDialog.Title = "ZHProject Game Save";
            // 
            // ZHProjectView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(514, 561);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "ZHProjectView";
            Text = "ZHProject";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem _menuFile;
        private ToolStripMenuItem _menuNewGame;
        private ToolStripMenuItem _menuLoadGame;
        private ToolStripMenuItem _menuSaveGame;
        private ToolStripMenuItem _menuExitGame;
        private ToolStripMenuItem _menuSettings;
        private ToolStripMenuItem _menuEasyGame;
        private ToolStripMenuItem _menuMediumGame;
        private ToolStripMenuItem _menuHardGame;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel _labelPointText;
        private ToolStripStatusLabel _labelPoints;
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;
    }
}