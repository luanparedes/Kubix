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

        public readonly IDataInitial _dataInitial = Ioc.Default.GetService<IDataInitial>();
        public readonly IAppInfo _appInfo = Ioc.Default.GetService<IAppInfo>();
        private readonly INavigationService _navigation = Ioc.Default.GetService<INavigationService>();
        private readonly IThemeService _themeService = Ioc.Default.GetService<IThemeService>();

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
                    break;
                case ElementTheme.Light:
                    _dataInitial.IsDarkThemeChecked = false;
                    _dataInitial.IsLightThemeChecked = true;
                    _dataInitial.IsDefaultThemeChecked = false;
                    break;
                case ElementTheme.Default:
                    _dataInitial.IsDarkThemeChecked = false;
                    _dataInitial.IsLightThemeChecked = false;
                    _dataInitial.IsDefaultThemeChecked = true;
                    break;
            }
        }

        #endregion

        #region Event Handlers

        public void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
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
                    break;
                case "AICheck":
                    _dataInitial.HasAI = checkbox.IsChecked.Value;
                    break;
                case "MusicCheck":
                    _dataInitial.HasMusic = checkbox.IsChecked.Value;
                    break;
                case "YoutubeCheck":
                    _dataInitial.HasYoutube = checkbox.IsChecked.Value;
                    break;
                case "StreamingCheck":
                    _dataInitial.HasStreaming = checkbox.IsChecked.Value;
                    break;
                case "SocialMediaCheck":
                    _dataInitial.HasSocialMedia = checkbox.IsChecked.Value;
                    break;
                case "KNoteCheck":
                    _dataInitial.HasKNote = checkbox.IsChecked.Value;
                    break;
                case "OfficeCheck":
                    _dataInitial.HasOffice = checkbox.IsChecked.Value;
                    break;
                case "GoogleCheck":
                    _dataInitial.HasGoogle = checkbox.IsChecked.Value;
                    break;
                case "CompilersCheck":
                    _dataInitial.HasCompilers = checkbox.IsChecked.Value;
                    break;
            }

            _dataInitial.OnUpdateUI();
        }

        #endregion
    }
}