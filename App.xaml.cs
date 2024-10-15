using CommunityToolkit.Mvvm.DependencyInjection;
using KanBoard.Helpers;
using KanBoard.Services.Classes;
using KanBoard.Services.Interfaces;
using KanBoard.View;
using KanBoard.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace KanBoard
{
    public partial class App : Application
    {
        #region Fields & Properties

        public static App Instance;

        public Window MainWindow { get; private set; }

        private IAppInfo _appInfo;
        private INavigationService _navigationService;

        #endregion

        #region Constructor

        public App()
        {
            this.InitializeComponent();
            ConfigureServices();

            App.Instance = this;
        }

        #endregion

        #region Methods

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            services
                .AddSingleton<IAppInfo, AppInfo>()
                .AddSingleton<ILogger, LogService>()
                .AddSingleton<IThemeService, ThemeService>()
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<Window>()
                .AddSingleton<MainBoardViewModel>()
                .AddSingleton<SettingsViewModel>()
                .AddSingleton<UserInfoViewModel>()
                .AddSingleton<AppMusicViewModel>()
                .AddSingleton<BrowserViewModel>()
                .AddSingleton<YoutubeViewModel>()
                .AddSingleton<StreamingsViewModel>()
                .AddSingleton<KNoteViewModel>();

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }

        public void ChangeTheme(ElementTheme theme)
        {

            if (MainWindow.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = theme;
            }
        }

        private void ConfigureMainWindow()
        {
            MainWindow = Ioc.Default.GetService<Window>();

            MainWindow.Title = _appInfo.GetAppFullNameVersion();
            MainWindow.ExtendsContentIntoTitleBar = true;
            MainWindow.Content = new Frame();
            MainWindow.Activate();
        }

        #endregion

        #region Event Handlers

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            _appInfo = Ioc.Default.GetService<IAppInfo>();
            _navigationService = Ioc.Default.GetService<INavigationService>();

            ConfigureMainWindow();
     
            _navigationService.SetFrame(MainWindow.Content as Frame, FrameTypeEnum.MainFrame);
            _navigationService.GoToPage(typeof(SplashScreenPage));
            await Task.Delay(3000);
            _navigationService.BackToBoard();
        }

        #endregion
    }
}
