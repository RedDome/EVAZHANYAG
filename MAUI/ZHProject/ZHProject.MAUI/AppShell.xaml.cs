using ZHProject.MAUI.View;
using ZHProject.MAUI.ViewModel;
using ZHProject.MAUI.Persistence;
using ZHProject.Model;
using ZHProject.Persistence;

namespace ZHProject.MAUI
{
    public partial class AppShell : Shell
    {
        private IZHProjectDataAccess _zhprojectDataAccess;
        private readonly ZHProjectModel _zhprojectGameModel;
        private readonly ZHProjectViewModel _zhprojectViewModel;

        private readonly IStore _store;
        private readonly StoredGameBrowserModel _storedGameBrowserModel;
        private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;

        public AppShell(IStore zhprojectStore,
        IZHProjectDataAccess zhprojectDataAccess,
        ZHProjectModel zhprojectGameModel,
        ZHProjectViewModel zhprojectViewModel)
        {
            InitializeComponent();

            _store = zhprojectStore;
            _zhprojectDataAccess = zhprojectDataAccess;
            _zhprojectGameModel = zhprojectGameModel;
            _zhprojectViewModel = zhprojectViewModel;

            _zhprojectGameModel.GameOver += ZHProjectGameModel_GameOver;

            _zhprojectViewModel.NewGame += ZHProjectViewModel_NewGame;
            _zhprojectViewModel.LoadGame += ZHProjectViewModel_LoadGame;
            _zhprojectViewModel.SaveGame += ZHProjectViewModel_SaveGame;
            _zhprojectViewModel.ExitGame += ZHProjectViewModel_ExitGame;

            // a játékmentések kezelésének összeállítása
            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
            _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
        }

        private async void ZHProjectGameModel_GameOver(object? sender, ZHProjectEventArgs e)
        {
            await DisplayAlert("Game Is Over!",
                                    "ZHProject!",
                                    "OK");
        }

        private void ZHProjectViewModel_NewGame(object? sender, EventArgs e)
        {
            // _squaresViewModel.DeleteTable();
            // _squaresViewModel.GenerateTable();
            // _squaresGameModel.NewGame();
            // _squaresViewModel.UpdateView();

            // _squaresViewModel.RefreshMenu();
        }

        private async void ZHProjectViewModel_LoadGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();
            await Navigation.PushAsync(new LoadGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }

        private async void ZHProjectViewModel_SaveGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();
            await Navigation.PushAsync(new SaveGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }

        private async void ZHProjectViewModel_ExitGame(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = _zhprojectViewModel
            });
        }

        private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync();

            try
            {
                await _zhprojectGameModel.LoadGameAsync(e.Name);

                await Navigation.PopAsync();
                await DisplayAlert("ZHProject játék", "Sikeres betöltés.", "OK");
            }
            catch
            {
                await DisplayAlert("ZHProject játék", "Sikertelen betöltés.", "OK");
            }
        }

        private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync();

            try
            {
                await _zhprojectGameModel.SaveGameAsync(e.Name);
                await DisplayAlert("ZHProject játék", "Sikeres mentés.", "OK");
            }
            catch
            {
                await DisplayAlert("ZHProject játék", "Sikertelen mentés.", "OK");
            }
        }
    }
}