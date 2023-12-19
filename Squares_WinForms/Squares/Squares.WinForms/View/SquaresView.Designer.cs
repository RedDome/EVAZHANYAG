namespace Squares.WinForms
{
    partial class SquaresView
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
            _menuStrip = new MenuStrip();
            _menuGameButton = new ToolStripMenuItem();
            _menuNewGameButton = new ToolStripMenuItem();
            _menuLoadGameButton = new ToolStripMenuItem();
            _menuSaveGameButton = new ToolStripMenuItem();
            _menuExitGameButton = new ToolStripMenuItem();
            _menuSettingsButton = new ToolStripMenuItem();
            _menuEasyButton = new ToolStripMenuItem();
            _menuMediumButton = new ToolStripMenuItem();
            _menuHardButton = new ToolStripMenuItem();
            _openFileDialog = new OpenFileDialog();
            _saveFileDialog = new SaveFileDialog();
            statusStrip1 = new StatusStrip();
            _currPlayerLabel = new ToolStripStatusLabel();
            _currPlayerText = new ToolStripStatusLabel();
            _p1pointLabel = new ToolStripStatusLabel();
            _p1Points = new ToolStripStatusLabel();
            _p2PointLabel = new ToolStripStatusLabel();
            _p2Points = new ToolStripStatusLabel();
            _menuStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // _menuStrip
            // 
            _menuStrip.Items.AddRange(new ToolStripItem[] { _menuGameButton, _menuSettingsButton });
            _menuStrip.Location = new Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Size = new Size(612, 24);
            _menuStrip.TabIndex = 0;
            _menuStrip.Text = "menuStrip1";
            // 
            // _menuGameButton
            // 
            _menuGameButton.DropDownItems.AddRange(new ToolStripItem[] { _menuNewGameButton, _menuLoadGameButton, _menuSaveGameButton, _menuExitGameButton });
            _menuGameButton.Name = "_menuGameButton";
            _menuGameButton.Size = new Size(50, 20);
            _menuGameButton.Text = "Game";
            // 
            // _menuNewGameButton
            // 
            _menuNewGameButton.Name = "_menuNewGameButton";
            _menuNewGameButton.Size = new Size(134, 22);
            _menuNewGameButton.Text = "New Game";
            _menuNewGameButton.Click += _menuNewGameButton_Click;
            // 
            // _menuLoadGameButton
            // 
            _menuLoadGameButton.Name = "_menuLoadGameButton";
            _menuLoadGameButton.Size = new Size(134, 22);
            _menuLoadGameButton.Text = "Load Game";
            _menuLoadGameButton.Click += _menuLoadGameButton_Click;
            // 
            // _menuSaveGameButton
            // 
            _menuSaveGameButton.Name = "_menuSaveGameButton";
            _menuSaveGameButton.Size = new Size(134, 22);
            _menuSaveGameButton.Text = "Save Game";
            _menuSaveGameButton.Click += _menuSaveGameButton_Click;
            // 
            // _menuExitGameButton
            // 
            _menuExitGameButton.Name = "_menuExitGameButton";
            _menuExitGameButton.Size = new Size(134, 22);
            _menuExitGameButton.Text = "Exit Game";
            _menuExitGameButton.Click += _menuExitGameButton_Click;
            // 
            // _menuSettingsButton
            // 
            _menuSettingsButton.DropDownItems.AddRange(new ToolStripItem[] { _menuEasyButton, _menuMediumButton, _menuHardButton });
            _menuSettingsButton.Name = "_menuSettingsButton";
            _menuSettingsButton.Size = new Size(61, 20);
            _menuSettingsButton.Text = "Settings";
            // 
            // _menuEasyButton
            // 
            _menuEasyButton.Name = "_menuEasyButton";
            _menuEasyButton.Size = new Size(153, 22);
            _menuEasyButton.Text = "Easy Game";
            _menuEasyButton.Click += _menuEasyButton_Click;
            // 
            // _menuMediumButton
            // 
            _menuMediumButton.Name = "_menuMediumButton";
            _menuMediumButton.Size = new Size(153, 22);
            _menuMediumButton.Text = "Medium Game";
            _menuMediumButton.Click += _menuMediumButton_Click;
            // 
            // _menuHardButton
            // 
            _menuHardButton.Name = "_menuHardButton";
            _menuHardButton.Size = new Size(153, 22);
            _menuHardButton.Text = "Hard Game";
            _menuHardButton.Click += _menuHardButton_Click;
            // 
            // _openFileDialog
            // 
            _openFileDialog.Filter = "Squares game (*.sqr)|*.sqr";
            _openFileDialog.Title = "Squares Game Loading";
            // 
            // _saveFileDialog
            // 
            _saveFileDialog.Filter = "Squares game (*.sqr)|*.sqr";
            _saveFileDialog.Title = "Squares Game Save";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { _currPlayerLabel, _currPlayerText, _p1pointLabel, _p1Points, _p2PointLabel, _p2Points });
            statusStrip1.Location = new Point(0, 629);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(612, 24);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // _currPlayerLabel
            // 
            _currPlayerLabel.Name = "_currPlayerLabel";
            _currPlayerLabel.Size = new Size(85, 19);
            _currPlayerLabel.Text = "Current Player:";
            // 
            // _currPlayerText
            // 
            _currPlayerText.BorderSides = ToolStripStatusLabelBorderSides.Right;
            _currPlayerText.BorderStyle = Border3DStyle.Etched;
            _currPlayerText.Name = "_currPlayerText";
            _currPlayerText.Size = new Size(52, 19);
            _currPlayerText.Text = "Player 1";
            // 
            // _p1pointLabel
            // 
            _p1pointLabel.Name = "_p1pointLabel";
            _p1pointLabel.Size = new Size(101, 19);
            _p1pointLabel.Text = "Points of Player 1:";
            // 
            // _p1Points
            // 
            _p1Points.BorderSides = ToolStripStatusLabelBorderSides.Right;
            _p1Points.BorderStyle = Border3DStyle.Etched;
            _p1Points.Name = "_p1Points";
            _p1Points.Size = new Size(17, 19);
            _p1Points.Text = "0";
            // 
            // _p2PointLabel
            // 
            _p2PointLabel.Name = "_p2PointLabel";
            _p2PointLabel.Size = new Size(101, 19);
            _p2PointLabel.Text = "Points of Player 2:";
            // 
            // _p2Points
            // 
            _p2Points.BorderSides = ToolStripStatusLabelBorderSides.Right;
            _p2Points.BorderStyle = Border3DStyle.Etched;
            _p2Points.Name = "_p2Points";
            _p2Points.Size = new Size(17, 19);
            _p2Points.Text = "0";
            // 
            // SquaresView
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            ClientSize = new Size(612, 653);
            Controls.Add(statusStrip1);
            Controls.Add(_menuStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = _menuStrip;
            MaximizeBox = false;
            Name = "SquaresView";
            Text = "Squares";
            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip _menuStrip;
        private ToolStripMenuItem _menuGameButton;
        private ToolStripMenuItem _menuSettingsButton;
        private ToolStripMenuItem _menuNewGameButton;
        private ToolStripMenuItem _menuLoadGameButton;
        private ToolStripMenuItem _menuSaveGameButton;
        private ToolStripMenuItem _menuExitGameButton;
        private ToolStripMenuItem _menuEasyButton;
        private ToolStripMenuItem _menuMediumButton;
        private ToolStripMenuItem _menuHardButton;
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel _currPlayerLabel;
        private ToolStripStatusLabel _currPlayerText;
        private ToolStripStatusLabel _p1pointLabel;
        private ToolStripStatusLabel _p1Points;
        private ToolStripStatusLabel _p2PointLabel;
        private ToolStripStatusLabel _p2Points;
    }
}