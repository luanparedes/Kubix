using CommunityToolkit.Mvvm.DependencyInjection;
using KanBoard.Helpers;
using KanBoard.Services.Classes;
using KanBoard.Services.Interfaces;
using KanBoard.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
                .AddSingleton<BrowserViewModel>();

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

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _appInfo = Ioc.Default.GetService<IAppInfo>();
            _navigationService = Ioc.Default.GetService<INavigationService>();

            ConfigureMainWindow();
     
            _navigationService.SetFrame(MainWindow.Content as Frame, FrameTypeEnum.MainFrame);
            _navigationService.BackToBoard();
        }

        #endregion
    }
}
