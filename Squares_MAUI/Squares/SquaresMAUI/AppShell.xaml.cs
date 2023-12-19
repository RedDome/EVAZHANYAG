using Squares.Model;
using Squares.Persistence;
using SquaresMAUI.View;
using SquaresMAUI.ViewModel;

namespace SquaresMAUI
{
    public partial class AppShell : Shell
    {
        private ISquaresDataAccess _squaresDataAccess;
        private readonly SquaresModel _squaresGameModel;
        private readonly SquaresViewModel _squaresViewModel;

        private readonly IStore _store;
        private readonly StoredGameBrowserModel _storedGameBrowserModel;
        private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;

        public AppShell(IStore squaresStore,
        ISquaresDataAccess squaresDataAccess,
        SquaresModel squaresGameModel,
        SquaresViewModel squaresViewModel)
        {
            InitializeComponent();

            // játék összeállítása
            _store = squaresStore;
            _squaresDataAccess = squaresDataAccess;
            _squaresGameModel = squaresGameModel;
            _squaresViewModel = squaresViewModel;

            _squaresGameModel.GameOver += SquaresGameModel_GameOver;

            _squaresViewModel.NewGame += SquaresViewModel_NewGame;
            _squaresViewModel.LoadGame += SquaresViewModel_LoadGame;
            _squaresViewModel.SaveGame += SquaresViewModel_SaveGame;
            _squaresViewModel.ExitGame += SquaresViewModel_ExitGame;

            // a játékmentések kezelésének összeállítása
            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
            _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
        }

        private async void SquaresGameModel_GameOver(object? sender, SquaresEventArgs e)
        {
            if (e.Winner == 1)
            {
                await DisplayAlert("Squares",
                                    "First Player Won!",
                                    "OK");
            }
            else if (e.Winner == 2)
            {
                await DisplayAlert("Squares",
                                    "Second Player Won!",
                                    "OK");
            }
            else if (e.P1Score == e.P2Score)
            {
                await DisplayAlert("Squares",
                                    "Draw!",
                                    "OK");
            }
        }

        private void SquaresViewModel_NewGame(object? sender, EventArgs e)
        {
            _squaresViewModel.DeleteTable();
            _squaresViewModel.GenerateTable();
            _squaresGameModel.NewGame();
            _squaresViewModel.UpdateView();

            _squaresViewModel.RefreshMenu();
        }

        private async void SquaresViewModel_LoadGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();
            await Navigation.PushAsync(new LoadGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }

        private async void SquaresViewModel_SaveGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();
            await Navigation.PushAsync(new SaveGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }

        private async void SquaresViewModel_ExitGame(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = _squaresViewModel
            });
        }

        private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync();

            try
            {
                await _squaresGameModel.LoadGameAsync(e.Name);

                await Navigation.PopAsync();
                await DisplayAlert("Squares játék", "Sikeres betöltés.", "OK");
            }
            catch
            {
                await DisplayAlert("Squares játék", "Sikertelen betöltés.", "OK");
            }
        }

        private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync();

            try
            {
                await _squaresGameModel.SaveGameAsync(e.Name);
                await DisplayAlert("Squares játék", "Sikeres mentés.", "OK");
            }
            catch
            {
                await DisplayAlert("Squares játék", "Sikertelen mentés.", "OK");
            }
        }
    }
}