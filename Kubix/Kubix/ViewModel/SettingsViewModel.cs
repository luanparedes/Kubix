using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Kubix.Services.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using System.Windows.Input;

namespace Kubix.ViewModel
{
    public class SettingsViewModel : ObservableObject, INotifyPropertyChanged
    {
        #region Fields & Properties

        public readonly IDataInitial _dataInitial;
        public readonly IAppInfo _appInfo;
        private readonly INavigationService _navigation;
        private readonly IThemeService _themeService;
        private readonly ILogger _logger;

        private ElementTheme _themeElement = ElementTheme.Dark;
        public ElementTheme ThemeElement

        {
            get { return _themeElement; }
            set
            {
                SetProperty(ref _themeElement, value);
            }
        }

        #endregion

        #region Constructor
        public SettingsViewModel(IDataInitial dataInitial, IAppInfo appInfo, INavigationService navigation, IThemeService themeService, ILogger logger)
        {
            _dataInitial = dataInitial;
            _appInfo = appInfo;
            _navigation = navigation;
            _themeService = themeService;
            _logger = logger;
            _logger.InfoLog("SettingsViewModel initialized.");
        }
        #endregion

        #region Commands

        private ICommand _switchThemeCommand;
        public ICommand SwitchThemeCommand { get => _switchThemeCommand ?? (_switchThemeCommand = new RelayCommand<ElementTheme>(ChangeAppTheme)); }

        private void ChangeAppTheme(ElementTheme theme)
        {
            _themeService.ChangeAppTheme(theme);

            switch (theme)
            {
                case ElementTheme.Dark:
                    _dataInitial.IsDarkThemeChecked = true;
                    _dataInitial.IsLightThemeChecked = false;
                    _dataInitial.IsDefaultThemeChecked = false;
                    _logger.InfoLog("Theme changed to Dark.");
                    break;
                case ElementTheme.Light:
                    _dataInitial.IsDarkThemeChecked = false;
                    _dataInitial.IsLightThemeChecked = true;
                    _dataInitial.IsDefaultThemeChecked = false;
                    _logger.InfoLog("Theme changed to Light.");
                    break;
                case ElementTheme.Default:
                    _dataInitial.IsDarkThemeChecked = false;
                    _dataInitial.IsLightThemeChecked = false;
                    _dataInitial.IsDefaultThemeChecked = true;
                    _logger.InfoLog("Theme changed to Default from system.");
                    break;
            }
        }

        #endregion

        #region Event Handlers

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _navigation.BackPrevious();
        }

        public void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;

            switch (checkbox.Tag)
            {
                case "WebCheck":
                    _dataInitial.HasWebBrowser = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Web Browser setting changed to {_dataInitial.HasWebBrowser}.");
                    break;
                case "AICheck":
                    _dataInitial.HasAI = checkbox.IsChecked.Value;
                    _logger.InfoLog($"AI setting changed to {_dataInitial.HasAI}.");
                    break;
                case "MusicCheck":
                    _dataInitial.HasMusic = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Music setting changed to {_dataInitial.HasMusic}.");
                    break;
                case "YoutubeCheck":
                    _dataInitial.HasYoutube = checkbox.IsChecked.Value;
                    _logger.InfoLog($"YouTube setting changed to {_dataInitial.HasYoutube}.");
                    break;
                case "StreamingCheck":
                    _dataInitial.HasStreaming = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Streaming setting changed to {_dataInitial.HasStreaming}.");
                    break;
                case "SocialMediaCheck":
                    _dataInitial.HasSocialMedia = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Social Media setting changed to {_dataInitial.HasSocialMedia}.");
                    break;
                case "KNoteCheck":
                    _dataInitial.HasKNote = checkbox.IsChecked.Value;
                    _logger.InfoLog($"KNote setting changed to {_dataInitial.HasKNote}.");
                    break;
                case "OfficeCheck":
                    _dataInitial.HasOffice = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Office setting changed to {_dataInitial.HasOffice}.");
                    break;
                case "GoogleCheck":
                    _dataInitial.HasGoogle = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Google setting changed to {_dataInitial.HasGoogle}.");
                    break;
                case "CompilersCheck":
                    _dataInitial.HasCompilers = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Compilers setting changed to {_dataInitial.HasCompilers}.");
                    break;
                case "TerminalCheck":
                    _dataInitial.HasTerminal = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Terminal setting changed to {_dataInitial.HasTerminal}.");
                    break;
                case "KDiffCheck":
                    _dataInitial.HasKDiff = checkbox.IsChecked.Value;
                    _logger.InfoLog($"KDiff setting changed to {_dataInitial.HasKDiff}.");
                    break;
                case "DownloaderCheck":
                    _dataInitial.HasDownloader = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Downloader setting changed to {_dataInitial.HasDownloader}.");
                    break;
            }

            _dataInitial.OnUpdateUI();
        }

        #endregion
    }
}