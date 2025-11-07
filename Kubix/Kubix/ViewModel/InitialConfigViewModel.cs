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
        private Panel panel;
        private CheckBox selectAllCheckbox;

        private readonly ILogger _logger;
        private readonly IDataInitial _dataInitial;
        private readonly INavigationService _navigationService;

        private bool _hasItemSelected = true;
        public bool HasItemSelected
        {
            get { return _hasItemSelected; }
            set { SetProperty(ref _hasItemSelected, value); }
        }

        private bool _isAllItensSelected = true;
        public bool IsAllItensSelected
        {
            get { return _isAllItensSelected; }
            set { SetProperty(ref _isAllItensSelected, value); }
        }

        private string _currentState = WELCOME_STATE;
        public string CurrentState
        {
            get { return _currentState; }
            set
            {
                SetProperty(ref _currentState, value);
                VisualStateManager.GoToState(pageControl, value, true);
            }
        }

        #endregion

        #region Constructor

        public InitialConfigViewModel(INavigationService navigationService, IDataInitial dataInitial, ILogger logger)
        {
            _navigationService = navigationService;
            _dataInitial = dataInitial;
            _logger = logger;
            _logger.InfoLog("InitialConfigViewModel initialized.");
        }

        #endregion

        #region Methods

        private void VerifyIfAnyIsChecked()
        {
            foreach (var item in panel.Children)
            {
                if (item is CheckBox)
                {
                    if ((item as CheckBox).IsChecked.Value)
                    {
                        HasItemSelected = true;
                        return;
                    }
                }
            }

            HasItemSelected = false;
        }

        private void IsAllItensChecked()
        {
            foreach (var item in panel.Children)
            {
                if (item is CheckBox check)
                {
                    if (!check.IsChecked.Value)
                    {
                        IsAllItensSelected = false;
                        selectAllCheckbox.IsChecked = false;
                        selectAllCheckbox.Visibility = Visibility.Visible;
                        return;
                    }
                }
            }

            IsAllItensSelected = true;
            selectAllCheckbox.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Event Handlers

        public void PageControl_Loaded(object sender, RoutedEventArgs e)
        {
            pageControl = sender as UserControl;
        }

        public void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            panel = sender as Panel;
            IsAllItensChecked();
        }

        public void CheckBox_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            selectAllCheckbox = sender as CheckBox;
            selectAllCheckbox.Checked += CheckBox_Checked;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Tag)
            {
                case "ContinueBtn":
                    CurrentState = CHOICES_STATE;
                    _logger.InfoLog("Switched to Choices state in Initial Configuration.");
                    break;
                case "FinishBtn":
                    _dataInitial.IsFirstTimeOpening = false;
                    _navigationService.BackToBoard();
                    _logger.InfoLog("Finished Initial Configuration and navigated to Board.");
                    break;
            }
        }

        public void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;

            switch (checkbox.Tag)
            {
                case "WebCheck":
                    _dataInitial.HasWebBrowser = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Web Browser selection changed: {_dataInitial.HasWebBrowser}");
                    break;
                case "AICheck":
                    _dataInitial.HasAI = checkbox.IsChecked.Value;
                    _logger.InfoLog($"AI selection changed: {_dataInitial.HasAI}");
                    break;
                case "MusicCheck":
                    _dataInitial.HasMusic = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Music selection changed: {_dataInitial.HasMusic}");
                    break;
                case "YoutubeCheck":
                    _dataInitial.HasYoutube = checkbox.IsChecked.Value;
                    _logger.InfoLog($"YouTube selection changed: {_dataInitial.HasYoutube}");
                    break;
                case "StreamingCheck":
                    _dataInitial.HasStreaming = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Streaming selection changed: {_dataInitial.HasStreaming}");
                    break;
                case "SocialMediaCheck":
                    _dataInitial.HasSocialMedia = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Social Media selection changed: {_dataInitial.HasSocialMedia}");
                    break;
                case "KNoteCheck":
                    _dataInitial.HasKNote = checkbox.IsChecked.Value;
                    _logger.InfoLog($"KNote selection changed: {_dataInitial.HasKNote}");
                    break;
                case "OfficeCheck":
                    _dataInitial.HasOffice = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Office selection changed: {_dataInitial.HasOffice}");
                    break;
                case "GoogleCheck":
                    _dataInitial.HasGoogle = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Google selection changed: {_dataInitial.HasGoogle}");
                    break;
                case "CompilersCheck":
                    _dataInitial.HasCompilers = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Compilers selection changed: {_dataInitial.HasCompilers}");
                    break;
                case "TerminalCheck":
                    _dataInitial.HasTerminal = checkbox.IsChecked.Value;
                    _logger.InfoLog($"Terminal selection changed: {_dataInitial.HasTerminal}");
                    break;
                case "KDiffCheck":
                    _dataInitial.HasKDiff = checkbox.IsChecked.Value;
                    _logger.InfoLog($"KDiff selection changed: {_dataInitial.HasKDiff}");
                    break;
            }

            VerifyIfAnyIsChecked();
            IsAllItensChecked();
        }

        public void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in panel.Children)
            {
                if (item is CheckBox)
                {
                    (item as CheckBox).IsChecked = true;
                }
            }

            selectAllCheckbox.Visibility = Visibility.Collapsed;
            IsAllItensChecked();
            VerifyIfAnyIsChecked();
        }

        #endregion

    }
}
