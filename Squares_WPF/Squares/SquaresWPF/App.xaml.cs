using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using SquaresWPF.ViewModel;
using SquaresWPF.View;
using Squares.Model;
using Squares.Persistence;
using System.Windows.Controls;

namespace SquaresWPF
{
    public partial class App : Application
    {
        private SquaresModel _model = null!;
        private SquaresViewModel _viewModel = null!;
        private MainWindow _view = null!;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            _model = new SquaresModel(new SquaresFileDataAccess());
            _model.GameOver += new EventHandler<SquaresEventArgs>(Model_GameOver);
            _model.NewGame();

            _viewModel = new SquaresViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing);
            _view.Show();
        }

        private void View_Closing(object? sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Sudoku", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _viewModel.DeleteTable();
            _viewModel.GenerateTable();
            _model.NewGame();
            _viewModel.UpdateView();

            _viewModel.RefreshMenu();
        }

        private async void ViewModel_LoadGame(object? sender, System.EventArgs e)
        {
            _viewModel.DeleteTable();
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Squares tábla betöltése";
                openFileDialog.Filter = "Squares tábla|*.sqr";
                if (openFileDialog.ShowDialog() == true)
                {
                    await _model.LoadGameAsync(openFileDialog.FileName);
                }
                _viewModel.GenerateTable();
                _viewModel.UpdateView();  
                _viewModel.RefreshTable();
                _viewModel.RefreshMenu();
            }
            catch (SquaresDataExpection)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "Squares", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Squares tábla mentése";
                saveFileDialog.Filter = "Squares tábla|*.sqr";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (SquaresDataExpection)
                    {
                        MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("A fájl mentése sikertelen!", "Sudoku", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close();
        }

        private void Model_GameOver(object? sender, SquaresEventArgs e)
        {
            if (e.Winner == 1)
            {
                MessageBox.Show("First Player Won!",
                                "Squares",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else if (e.Winner == 2)
            {
                MessageBox.Show("Second Player Won!",
                                "Squares",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else if (e.P1Score == e.P2Score)
            {
                MessageBox.Show("Draw!",
                                "Squares",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
        }
    }
}
