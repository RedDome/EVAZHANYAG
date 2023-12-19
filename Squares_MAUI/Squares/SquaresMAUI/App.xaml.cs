using Squares.Persistence;
using Squares.Model;
using SquaresMAUI.ViewModel;

namespace SquaresMAUI
{
    public partial class App : Application
    {
        // InitializeComponent();

        // MainPage = new AppShell();

        /// <summary>
        /// Erre az útvonalra mentjük a félbehagyott játékokat
        /// </summary>
        private const string SuspendedGameSavePath = "SuspendedGame";

        private readonly AppShell _appShell;
        private readonly ISquaresDataAccess _squaresDataAccess;
        private readonly SquaresModel _squaresModel;
        private readonly IStore _squaresStore;
        private readonly SquaresViewModel _squaresViewModel;

        public App()
        {
            InitializeComponent();

            _squaresStore = new SquaresStore();
            _squaresDataAccess = new SquaresFileDataAccess(FileSystem.AppDataDirectory);

            _squaresModel = new SquaresModel(_squaresDataAccess);
            _squaresViewModel = new SquaresViewModel(_squaresModel);

            _appShell = new AppShell(_squaresStore, _squaresDataAccess, _squaresModel, _squaresViewModel)
            {
                BindingContext = _squaresViewModel
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
                _squaresModel.NewGame();
                // _appShell.StartTimer();
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
                        await _squaresModel.LoadGameAsync(SuspendedGameSavePath);

                        // csak akkor indul az időzítő, ha sikerült betölteni a játékot
                        // _appShell.StartTimer();
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
                        // _appShell.StopTimer();
                        await _squaresModel.SaveGameAsync(SuspendedGameSavePath);
                    }
                    catch
                    {
                    }
                });
            };

            return window;
        }
    }
}