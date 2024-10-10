using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using KanBoard.Services.Classes;
using KanBoard.Services.Interfaces;
using KanBoard.View;
using Microsoft.UI.Xaml;
using System.Windows.Input;

namespace KanBoard.ViewModel
{
    public class SettingsViewModel : ObservableObject
    {
        #region Fields & Properties

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
        }

        #endregion

        #region Event Handlers

        public void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _navigation.BackPrevious();
        }

        #endregion
    }
}