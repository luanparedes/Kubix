using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.Helpers;
using Kubix.Services.Classes;
using Kubix.Services.Interfaces;
using Kubix.View;
using Kubix.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.Graphics;

namespace Kubix
{
    public partial class App : Application
    {
        #region Fields & Properties

        public static App Instance;

        public Window MainWindow { get; private set; }

        private IAppInfo _appInfo;
        private INavigationService _navigationService;
        private readonly IThemeService _themeService;
        private readonly IDataInitial _dataInitial;

        #endregion

        #region Constructor

        public App()
        {
            this.InitializeComponent();
            ConfigureServices();
            _themeService = Ioc.Default.GetService<IThemeService>();
            _dataInitial = Ioc.Default.GetService<IDataInitial>();

            App.Instance = this;
        }

        #endregion

        #region Methods

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            services
                .AddSingleton<IDataInitial, DataInitial>()
                .AddSingleton<IAppInfo, AppInfo>()
                .AddSingleton<ILogger, LogService>()
                .AddSingleton<IThemeService, ThemeService>()
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<IExcelService, ExcelService>()
                .AddSingleton<Window>()
                .AddSingleton<InitialConfigViewModel>()
                .AddSingleton<HomeViewModel>()
                .AddSingleton<MainBoardViewModel>()
                .AddSingleton<SettingsViewModel>()
                .AddSingleton<UserInfoViewModel>()
                .AddSingleton<AppMusicViewModel>()
                .AddSingleton<BrowserViewModel>()
                .AddSingleton<YoutubeViewModel>()
                .AddSingleton<StreamingsViewModel>()
                .AddSingleton<KNoteViewModel>()
                .AddSingleton<AIViewModel>()
                .AddSingleton<Office365ViewModel>()
                .AddSingleton<GoogleViewModel>()
                .AddSingleton<SocialMediasViewModel>()
                .AddSingleton<CompilersViewModel>()
                .AddSingleton<TerminalViewModel>()
                .AddTransient<ColorPickerViewModel>();

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
            SetInitialTheme();

            MainWindow = Ioc.Default.GetService<Window>();

            MainWindow.Title = _appInfo.GetAppFullNameVersion();
            MainWindow.ExtendsContentIntoTitleBar = true;
            MainWindow.Content = new Frame();
            MainWindow.Activate();

            CenterWindow();
        }

        public void CenterWindow()
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            var centerX = (displayArea.WorkArea.Width - appWindow.Size.Width) / 2;
            var centerY = (displayArea.WorkArea.Height - appWindow.Size.Height) / 2;

            appWindow.Move(new PointInt32(centerX, centerY));
        }

        private void SetInitialTheme()
        {
            if (_dataInitial.IsDefaultThemeChecked)
                _themeService.DefaultTheme();
            else if (_dataInitial.IsLightThemeChecked)
                _themeService.LightTheme();
            else if (_dataInitial.IsDarkThemeChecked)
                _themeService.DarkTheme();
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

            if (_dataInitial.IsFirstTimeOpening)
                _navigationService.GoToPage(typeof(InitialConfigPage));
            else
                _navigationService.BackToBoard();
        }

        #endregion
    }
}
