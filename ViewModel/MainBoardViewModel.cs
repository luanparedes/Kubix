
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using KanBoard.Services.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Security.Cryptography.Core;

namespace KanBoard.ViewModel
{
    public class MainBoardViewModel : ObservableObject
    {
        #region Fields & Properties

        private readonly IThemeService _themeService = Ioc.Default.GetService<IThemeService>();
        private readonly ILogger _logger = Ioc.Default.GetService<ILogger>();

        #endregion

        #region Contructor

        public MainBoardViewModel()
        {
            _logger.InfoLog("Entered Constructor ViewModel!");
        }

        #endregion

        #region Event Handlers

        private void myButton_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        //CODE THAT NEEDS TO MIGRATE TO SETTINGS PAGE
        //CODE THAT NEEDS TO MIGRATE TO SETTINGS PAGE
        //CODE THAT NEEDS TO MIGRATE TO SETTINGS PAGE

        #region Fields & Properties

        private ElementTheme _themeElement = ElementTheme.Dark;
        public ElementTheme ThemeElement

        {
            get { return _themeElement; }
            set
            {
                SetProperty(ref _themeElement, value);
            }
        }

        private void UpdateThemeColor()
        {

        }

        #endregion

        #region Commands

        private ICommand _switchThemeCommand;
        public ICommand SwitchThemeCommand { get => _switchThemeCommand ?? (_switchThemeCommand = new RelayCommand<ElementTheme>(ChangeAppTheme)); }

        private void ChangeAppTheme(ElementTheme theme)
        {
            _themeService.LightTheme();
        }

        #endregion
    }
}

