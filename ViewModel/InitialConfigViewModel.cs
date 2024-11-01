using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Kubix.Services.Interfaces;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Kubix.ViewModel
{
    public class InitialConfigViewModel : ObservableObject
    {
        #region Constants

        private const string WELCOME_STATE = "WelcomeState";
        private const string CHOICES_STATE = "ChoicesState";

        #endregion

        #region Fields & Properties

        private Control pageControl;

        private readonly INavigationService _navigationService;

        private string _currentState = WELCOME_STATE;
        public string CurrentState
        {
            get {  return _currentState; }
            set 
            {
                SetProperty(ref _currentState, value);
                VisualStateManager.GoToState(pageControl, value, true);
            }
        }

        #endregion

        #region Constructor

        public InitialConfigViewModel()
        {
            _navigationService = Ioc.Default.GetService<INavigationService>();
        }

        #endregion

        #region Event Handlers

        public void PageControl_Loaded(object sender, RoutedEventArgs e)
        {
            pageControl = sender as UserControl;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Tag)
            {
                case "ContinueBtn":
                    CurrentState = CHOICES_STATE;
                    break;
                case "FinishBtn":
                    _navigationService.BackToBoard();
                    break;                    
            }
        }

        #endregion

    }
}
