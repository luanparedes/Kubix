using CommunityToolkit.Mvvm.ComponentModel;
using Kubix.Services.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;

namespace Kubix.ViewModel
{
    public class AppMusicViewModel : ObservableObject
    {
        #region Constants

        public const string STATE_CHOICE_APP = "ChoiceAppState";
        public const string STATE_YOUTUBEMUSIC_APP = "YoutubeMusicAppState";
        public const string STATE_SPOTIFY_APP = "SpotifyAppState";
        public const string STATE_DEEZER_APP = "DeezerAppState";

        public readonly string YoutubeMusicURL = "https://music.youtube.com/";
        public readonly string SpotifyURL = "https://www.spotify.com";
        public readonly string DeezerURL = "https://www.deezer.com";

        #endregion

        #region Fields & Properties

        private readonly ILogger _logger;
        private Control pageControl;
        private WebView2 webView;

        public MusicApp ActualMusicApp;

        public string CurrentState { get; set; } = STATE_CHOICE_APP;

        #endregion

        #region Constructor
        public AppMusicViewModel(ILogger logger)
        {
            _logger = logger;
            _logger.InfoLog("AppMusicViewModel initialized.");
        }
        #endregion

        #region Event Handlers

        public void PageControl_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            pageControl = sender as UserControl;
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        public void WebView2_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            webView = sender as WebView2;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Tag)
            {
                case "YoutubeMusicBtn":
                    ActualMusicApp = MusicApp.YoutubeMusic;
                    CurrentState = STATE_YOUTUBEMUSIC_APP;
                    webView.Source = new Uri(YoutubeMusicURL);
                    _logger.InfoLog("Navigating to YouTube Music.");
                    break;
                case "SpotifyBtn":
                    ActualMusicApp = MusicApp.Spotify;
                    CurrentState = STATE_SPOTIFY_APP;
                    webView.Source = new Uri(SpotifyURL);
                    _logger.InfoLog("Navigating to Spotify.");
                    break;
                case "DeezerBtn":
                    ActualMusicApp = MusicApp.Deezer;
                    CurrentState = STATE_DEEZER_APP;
                    webView.Source = new Uri(DeezerURL);
                    _logger.InfoLog("Navigating to Deezer.");
                    break;
                case "BackButton":
                    CurrentState = STATE_CHOICE_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    _logger.InfoLog("Returning to app choice.");
                    break;
            }
        }

        public void GoogleAppWeb_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        public void AppMusicAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            CoreWebView2Settings settings = sender.CoreWebView2.Settings;

            settings.IsWebMessageEnabled = false;
            settings.AreDefaultScriptDialogsEnabled = false;
            settings.IsScriptEnabled = true;
            settings.AreHostObjectsAllowed = false;

            _logger.InfoLog("WebView2 AppMusic CoreWebView2 initialized with configuration:");
            _logger.InfoLog($"WebView2 AppMusic initialized with custom settings:{settings.IsWebMessageEnabled}");
            _logger.InfoLog($"WebView2 AppMusic initialized with custom settings:{settings.AreDefaultScriptDialogsEnabled}");
            _logger.InfoLog($"WebView2 AppMusic initialized with custom settings:{settings.IsScriptEnabled}");
            _logger.InfoLog($"WebView2 AppMusic initialized with custom settings:{settings.AreHostObjectsAllowed}");
        }

        public void AppMusicAppWeb_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (!args.Uri.StartsWith("https://"))
                args.Cancel = true;
        }

        #endregion
    }

    public enum MusicApp
    {
        YoutubeMusic,
        Spotify,
        Deezer
    }
}
