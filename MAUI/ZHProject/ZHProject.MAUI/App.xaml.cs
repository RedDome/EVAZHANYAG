using ZHProject.MAUI.View;
using ZHProject.MAUI.ViewModel;
using ZHProject.MAUI.Persistence;
using ZHProject.Model;
using ZHProject.Persistence;

namespace ZHProject.MAUI
{
    public partial class App : Application
    {
        private const string SuspendedGameSavePath = "SuspendedGame";

        private readonly AppShell _appShell;
        private readonly IZHProjectDataAccess _zhprojectDataAccess;
        private readonly ZHProjectModel _zhprojectModel;
        private readonly IStore _zhprojectStore;
        private readonly ZHProjectViewModel _zhprojectViewModel;

        public App()
        {
            InitializeComponent();

            _zhprojectStore = new ZHProjectStore();
            _zhprojectDataAccess = new ZHProjectFileDataAccess(FileSystem.AppDataDirectory);

            _zhprojectModel = new ZHProjectModel(_zhprojectDataAccess);
            _zhprojectViewModel = new ZHProjectViewModel(_zhprojectModel);

            _appShell = new AppShell(_zhprojectStore, _zhprojectDataAccess, _zhprojectModel, _zhprojectViewModel)
            {
                BindingContext = _zhprojectViewModel
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
                _zhprojectModel.NewGame();
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
                        await _zhprojectModel.LoadGameAsync(SuspendedGameSavePath);

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
                        await _zhprojectModel.SaveGameAsync(SuspendedGameSavePath);
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