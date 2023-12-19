using ELTE.Sudoku.Persistence;
using ELTE.Sudoku.ViewModel;
using ELTE.Sudoku.Model;

namespace ELTE.Sudoku;

public partial class App : Application
{
    /// <summary>
    /// Erre az útvonalra mentjük a félbehagyott játékokat
    /// </summary>
    private const string SuspendedGameSavePath = "SuspendedGame";

    private readonly AppShell _appShell;
    private readonly ISudokuDataAccess _sudokuDataAccess;
    private readonly SudokuGameModel _sudokuGameModel;
    private readonly IStore _sudokuStore;
    private readonly SudokuViewModel _sudokuViewModel;

    public App()
    {
        InitializeComponent();

        _sudokuStore = new SudokuStore();
        _sudokuDataAccess = new SudokuFileDataAccess(FileSystem.AppDataDirectory);

        _sudokuGameModel = new SudokuGameModel(_sudokuDataAccess);
        _sudokuViewModel = new SudokuViewModel(_sudokuGameModel);

        _appShell = new AppShell(_sudokuStore, _sudokuDataAccess, _sudokuGameModel, _sudokuViewModel)
        {
            BindingContext = _sudokuViewModel
        };
        MainPage = _appShell;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = base.CreateWindow(activationState);

        // az alkalmazás indításakor
        window.Created += (s, e) =>
        {
            // új játékot indítunk
            _sudokuGameModel.NewGame();
            _appShell.StartTimer();
        };

        // amikor az alkalmazás fókuszba kerül
        window.Activated += (s, e) =>
        {
            if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath)))
                return;

            Task.Run(async () =>
            {
                // betöltjük a felfüggesztett játékot, amennyiben van
                try
                {
                    await _sudokuGameModel.LoadGameAsync(SuspendedGameSavePath);

                    // csak akkor indul az időzítő, ha sikerült betölteni a játékot
                    _appShell.StartTimer();
                }
                catch
                {
                }
            });
        };

        // amikor az alkalmazás fókuszt veszt
        window.Deactivated += (s, e) =>
        {
            Task.Run(async () =>
            {
                try
                {
                    // elmentjük a jelenleg folyó játékot
                    _appShell.StopTimer();
                    await _sudokuGameModel.SaveGameAsync(SuspendedGameSavePath);
                }
                catch
                {
                }
            });
        };

        return window;
    }
}
