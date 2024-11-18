
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.Helpers;
using Kubix.Model;
using Kubix.Services.Interfaces;
using Kubix.View;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
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
        }

        private List<FeatureModel> GetAllFeatures()
        {
            List<FeatureModel> featuresList = new List<FeatureModel>();

            featuresList.Add(new FeatureModel()
            {
                FeatureName = Stringer.GetString("KB_HomeText"),
                FeatureIcon = new BitmapImage(new Uri($"ms-appx:///Assets/home_feature.png")),
                FeatureAlias = "HomeId",
            });

            featuresList.Add(new FeatureModel()
            {
                FeatureName = Stringer.GetString("KB_WebBrowserText"),
                FeatureIcon = new BitmapImage(new Uri($"ms-appx:///Assets/globe_feature.png")),
                FeatureAlias = "BrowserId",
            });

            featuresList.Add(new FeatureModel()
            {
                FeatureName = Stringer.GetString("KB_AIText"),
                FeatureIcon = new BitmapImage(new Uri($"ms-appx:///Assets/ai2_feature.png")),
                FeatureAlias = "AiId",
            });

            featuresList.Add(new FeatureModel()
            {
                FeatureName = Stringer.GetString("KB_MusicText"),
                FeatureIcon = new BitmapImage(new Uri($"ms-appx:///Assets/music_feature.png")),
                FeatureAlias = "MusicId",
            });

            featuresList.Add(new FeatureModel()
            {
                FeatureName = Stringer.GetString("KB_YoutubeText"),
                FeatureIcon = new BitmapImage(new Uri($"ms-appx:///Assets/youtube_feature.png")),
                FeatureAlias = "YoutubeId",
            });

            featuresList.Add(new FeatureModel()
            {
                FeatureName = Stringer.GetString("KB_StreamingText"),
                FeatureIcon = new BitmapImage(new Uri($"ms-appx:///Assets/streaming_feature.png")),
                FeatureAlias = "StreamingId",
            });

            featuresList.Add(new FeatureModel()
            {
                FeatureName = Stringer.GetString("KB_SocialMediaText"),
                FeatureIcon = new BitmapImage(new Uri($"ms-appx:///Assets/socialmedia_feature.png")),
                FeatureAlias = "SocialMediaId",
            });

            featuresList.Add(new FeatureModel()
            {
                FeatureName = Stringer.GetString("KB_KNoteText"),
                FeatureIcon = new BitmapImage(new Uri($"ms-appx:///Assets/note_feature.png")),
                FeatureAlias = "KNoteId",
            });

            featuresList.Add(new FeatureModel()
            {
                FeatureName = Stringer.GetString("KB_Office365Text"),
                FeatureIcon = new BitmapImage(new Uri($"ms-appx:///Assets/office_feature.png")),
                FeatureAlias = "OfficeId",
            });

            featuresList.Add(new FeatureModel()
            {
                FeatureName = Stringer.GetString("KB_GoogleText"),
                FeatureIcon = new BitmapImage(new Uri($"ms-appx:///Assets/google_feature.png")),
                FeatureAlias = "GoogleId",
            });

            featuresList.Add(new FeatureModel()
            {
                FeatureName = Stringer.GetString("KB_CompilersText"),
                FeatureIcon = new BitmapImage(new Uri($"ms-appx:///Assets/compiler_feature.png")),
                FeatureAlias = "CompilersId",
            });

            return featuresList;
        }

        #endregion

        #region Event Handlers

        //TODO: Agora perdeu a referencia entre a choices page, settings e menu
        //após ter sido excluído os items.

        public void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationView navigationView = sender as NavigationView;

            FeaturesList = GetAllFeatures();
            navigationView.MenuItemsSource = FeaturesList;
            navigationView.FooterMenuItems.Add(new FeatureModel()
            {
                FeatureName = Stringer.GetString("KB_SettingsText"),
                FeatureIcon = new BitmapImage(new Uri($"ms-appx:///Assets/settings_feature.png")),
                FeatureAlias = "SettingsId",
            });

            _navigationService.SetFrame((Frame)(sender as NavigationView).Content, FrameTypeEnum.NavigationViewFrame);
            navigationView.SelectedItem = (navigationView.MenuItemsSource as List<FeatureModel>)[0];
        }

        public void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Frame frame = (sender.Content) as Frame;

            string alias = (args.SelectedItem as FeatureModel).FeatureAlias;

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
            }
        }

        private void _dataInitial_UIUpdateChanged(object sender, System.EventArgs e)
        {
            GetChoicesFeatures();
        }

        #endregion
    }
}

