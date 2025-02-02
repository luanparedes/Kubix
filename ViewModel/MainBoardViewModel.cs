
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.Controls;
using Kubix.Helpers;
using Kubix.Model;
using Kubix.Services.Interfaces;
using Kubix.View;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace Kubix.ViewModel
{
    public partial class MainBoardViewModel : ObservableObject
    {
        #region Fields & Properties

        [ObservableProperty]
        private bool isBrowserShowing;
        
        [ObservableProperty]
        private bool isAIShowing;

        [ObservableProperty]
        private bool isMusicShowing;

        [ObservableProperty]
        private bool isYoutubeShowing;

        [ObservableProperty]
        private bool isStreamingShowing;

        [ObservableProperty]
        private bool isSocialMediaShowing;

        [ObservableProperty]
        private bool isKNoteShowing;

        [ObservableProperty]
        private bool isOffice365Showing;

        [ObservableProperty]
        private bool isGoogleShowing;

        [ObservableProperty]
        private bool isCompilersShowing;

        [ObservableProperty]
        private bool isTerminalShowing;

        [ObservableProperty]
        private bool isKDiffShowing;

        public bool IsToolsShowing => IsKNoteShowing || IsOffice365Showing || IsTerminalShowing || IsCompilersShowing || IsKDiffShowing;

        public readonly IDataInitial _dataInitial = Ioc.Default.GetService<IDataInitial>();
        private readonly INavigationService _navigationService = Ioc.Default.GetService<INavigationService>();
        private readonly ILogger _logger = Ioc.Default.GetService<ILogger>();

        public List<FeatureModel> FeaturesList;

        #endregion

        #region Contructor

        public MainBoardViewModel()
        {
            _logger.InfoLog("Entered Constructor ViewModel!");
            _dataInitial.UIUpdateChanged += _dataInitial_UIUpdateChanged;

            GetChoicesFeatures();
        }

        #endregion

        #region Methods

        private void GetChoicesFeatures()
        {
            IsBrowserShowing = _dataInitial.HasWebBrowser;
            IsAIShowing = _dataInitial.HasAI;
            IsMusicShowing = _dataInitial.HasMusic;
            IsYoutubeShowing = _dataInitial.HasYoutube;
            IsStreamingShowing = _dataInitial.HasStreaming;
            IsSocialMediaShowing = _dataInitial.HasSocialMedia;
            IsKNoteShowing = _dataInitial.HasKNote;
            IsOffice365Showing = _dataInitial.HasOffice;
            IsGoogleShowing = _dataInitial.HasGoogle;
            IsCompilersShowing = _dataInitial.HasCompilers;
            IsTerminalShowing = _dataInitial.HasTerminal;
            IsKDiffShowing = _dataInitial.HasKDiff;
        }

        #endregion

        #region Event Handlers

        public void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationView navigationView = sender as NavigationView;

            _navigationService.SetFrame((Frame)(sender as NavigationView).Content, FrameTypeEnum.NavigationViewFrame);
            navigationView.SelectedItem = navigationView.MenuItems[0];
        }

        public void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Frame frame = (sender.Content) as Frame;
            string alias = (args.SelectedItem as MenuNavigationItem).Alias;

            switch(alias)
            {
                case "HomeId":
                    _navigationService.GoToNavigationView(typeof(HomePage));
                    break;
                case "SettingsId":
                    _navigationService.GoToNavigationView(typeof(SettingsPage));
                    break;
                case "MusicId":
                    _navigationService.GoToNavigationView(typeof(AppMusicPage));
                    break;
                case "BrowserId":
                    _navigationService.GoToNavigationView(typeof(BrowserPage));
                    break;
                case "YoutubeId":
                    _navigationService.GoToNavigationView(typeof(YoutubePage));
                    break;
                case "StreamingId":
                    _navigationService.GoToNavigationView(typeof(StreamingsPage));
                    break;
                case "KNoteId":
                    _navigationService.GoToNavigationView(typeof(KNotePage));
                    break;
                case "AiId":
                    _navigationService.GoToNavigationView(typeof(AIPage));
                    break;
                case "OfficeId":
                    _navigationService.GoToNavigationView(typeof(Office365Page));
                    break;
                case "GoogleId":
                    _navigationService.GoToNavigationView(typeof(GooglePage));
                    break;
                case "SocialMediaId":
                    _navigationService.GoToNavigationView(typeof(SocialMediasPage));
                    break;
                case "CompilersId":
                    _navigationService.GoToNavigationView(typeof(CompilersPage));
                    break;
                case "TerminalId":
                    _navigationService.GoToNavigationView(typeof(TerminalPage));
                    break;
                case "KDiffId":
                    _navigationService.GoToNavigationView(typeof(KDiffPage));
                    break;
            }
        }

        private void _dataInitial_UIUpdateChanged(object sender, System.EventArgs e)
        {
            GetChoicesFeatures();
        }

        #endregion
    }
}

