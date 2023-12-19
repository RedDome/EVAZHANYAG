using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZHProject.Model;
using ZHProject.Persistence;
using ZHProject.WPF.View;
using ZHProject.WPF.ViewModel;

namespace ZHProject.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ZHProjectModel _model = null!;
        private ZHProjectViewModel _viewModel = null!;
        private MainWindow _view = null!;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            _model = new ZHProjectModel(new ZHProjectFileDataAccess());
            _model.GameOver += new EventHandler<ZHProjectEventArgs>(Model_GameOver);
            _model.NewGame();

            _viewModel = new ZHProjectViewModel(_model);
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
            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "ZHProject", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _model.NewGame();

            _viewModel.RefreshMenu();
        }

        private async void ViewModel_LoadGame(object? sender, System.EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "ZHProject tábla betöltése";
                openFileDialog.Filter = "ZHProject tábla|*.zhp";
                if (openFileDialog.ShowDialog() == true)
                {
                    await _model.LoadGameAsync(openFileDialog.FileName);
                }
            }
            catch (ZHProjectDataException)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "ZHProject", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "ZHProject tábla mentése";
                saveFileDialog.Filter = "ZHProject tábla|*.zhp";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (ZHProjectDataException)
                    {
                        MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("A fájl mentése sikertelen!", "ZHProject", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close();
        }

        private void Model_GameOver(object? sender, ZHProjectEventArgs e)
        {
            MessageBox.Show("Game Is Over!",
                                "ZHProject",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
        }
    }
}
