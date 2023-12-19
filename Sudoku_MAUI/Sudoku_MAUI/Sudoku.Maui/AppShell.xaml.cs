using ELTE.Sudoku.Model;
using ELTE.Sudoku.Persistence;
using ELTE.Sudoku.ViewModel;
using ELTE.Sudoku.View;

namespace ELTE.Sudoku;

public partial class AppShell : Shell
{
    #region Fields

    private ISudokuDataAccess _sudokuDataAccess;
    private readonly SudokuGameModel _sudokuGameModel;
    private readonly SudokuViewModel _sudokuViewModel;

    private readonly IDispatcherTimer _timer;

    private readonly IStore _store;
    private readonly StoredGameBrowserModel _storedGameBrowserModel;
    private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;

    #endregion

    #region Application methods

    public AppShell(IStore sudokuStore,
        ISudokuDataAccess sudokuDataAccess,
        SudokuGameModel sudokuGameModel,
        SudokuViewModel sudokuViewModel)
    {
        InitializeComponent();

        // játék összeállítása
        _store = sudokuStore;
        _sudokuDataAccess = sudokuDataAccess;
        _sudokuGameModel = sudokuGameModel;
        _sudokuViewModel = sudokuViewModel;

        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (_, _) => _sudokuGameModel.AdvanceTime();

        _sudokuGameModel.GameOver += SudokuGameModel_GameOver;

        _sudokuViewModel.NewGame += SudokuViewModel_NewGame;
        _sudokuViewModel.LoadGame += SudokuViewModel_LoadGame;
        _sudokuViewModel.SaveGame += SudokuViewModel_SaveGame;
        _sudokuViewModel.ExitGame += SudokuViewModel_ExitGame;

        // a játékmentések kezelésének összeállítása
        _storedGameBrowserModel = new StoredGameBrowserModel(_store);
        _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
        _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
        _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
    }

    #endregion

    #region Internal methods

    /// <summary>
    ///     Elindtja a játék léptetéséhez használt időzítőt.
    /// </summary>
    internal void StartTimer() => _timer.Start();

    /// <summary>
    ///     Megállítja a játék léptetéséhez használt időzítőt.
    /// </summary>
    internal void StopTimer() => _timer.Stop();

    #endregion

    #region Model event handlers

    /// <summary>
    ///     Játék végének eseménykezelője.
    /// </summary>
    private async void SudokuGameModel_GameOver(object? sender, SudokuEventArgs e)
    {
        StopTimer();

        if (e.IsWon)
        {
            // győzelemtől függő üzenet megjelenítése
            await DisplayAlert("Sudoku játék",
                "Gratulálok, győztél!" + Environment.NewLine +
                "Összesen " + e.GameStepCount + " lépést tettél meg és " +
                TimeSpan.FromSeconds(e.GameTime).ToString("g") + " ideig játszottál.",
                "OK");
        }
        else
        {
            await DisplayAlert("Sudoku játék", "Sajnálom, vesztettél, lejárt az idő!", "OK");
        }
    }

    #endregion

    #region ViewModel event handlers

    /// <summary>
    ///     Új játék indításának eseménykezelője.
    /// </summary>
    private void SudokuViewModel_NewGame(object? sender, EventArgs e)
    {
        _sudokuGameModel.NewGame();

        StartTimer();
    }

    /// <summary>
    ///     Játék betöltésének eseménykezelője.
    /// </summary>
    private async void SudokuViewModel_LoadGame(object? sender, EventArgs e)
    {
        await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
        await Navigation.PushAsync(new LoadGamePage
        {
            BindingContext = _storedGameBrowserViewModel
        }); // átnavigálunk a lapra
    }

    /// <summary>
    ///     Játék mentésének eseménykezelője.
    /// </summary>
    private async void SudokuViewModel_SaveGame(object? sender, EventArgs e)
    {
        await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
        await Navigation.PushAsync(new SaveGamePage
        {
            BindingContext = _storedGameBrowserViewModel
        }); // átnavigálunk a lapra
    }

    private async void SudokuViewModel_ExitGame(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage
        {
            BindingContext = _sudokuViewModel
        }); // átnavigálunk a beállítások lapra
    }


    /// <summary>
    ///     Betöltés végrehajtásának eseménykezelője.
    /// </summary>
    private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
    {
        await Navigation.PopAsync(); // visszanavigálunk

        // betöltjük az elmentett játékot, amennyiben van
        try
        {
            await _sudokuGameModel.LoadGameAsync(e.Name);

            // sikeres betöltés
            await Navigation.PopAsync(); // visszanavigálunk a játék táblára
            await DisplayAlert("Sudoku játék", "Sikeres betöltés.", "OK");

            // csak akkor indul az időzítő, ha sikerült betölteni a játékot
            StartTimer();
        }
        catch
        {
            await DisplayAlert("Sudoku játék", "Sikertelen betöltés.", "OK");
        }
    }

    /// <summary>
    ///     Mentés végrehajtásának eseménykezelője.
    /// </summary>
    private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
    {
        await Navigation.PopAsync(); // visszanavigálunk
        StopTimer();

        try
        {
            // elmentjük a játékot
            await _sudokuGameModel.SaveGameAsync(e.Name);
            await DisplayAlert("Sudoku játék", "Sikeres mentés.", "OK");
        }
        catch
        {
            await DisplayAlert("Sudoku játék", "Sikertelen mentés.", "OK");
        }
    }

    #endregion
}