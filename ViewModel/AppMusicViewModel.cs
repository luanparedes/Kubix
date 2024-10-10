using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KanBoard.ViewModel
{
    public class AppMusicViewModel : ObservableObject
    {
        #region Constants

        public const string STATE_CHOICE_APP = "ChoiceAppState";
        public const string STATE_SPOTIFY_APP = "SpotifyAppState";
        public const string STATE_DEEZER_APP = "DeezerAppState";

        public readonly string SpotifyURL = "http://www.spotify.com";
        public readonly string DeezerURL = "http://www.deezer.com";
        public readonly string NullURL = "http://www.null.com";

        #endregion

        #region Fields & Properties

        private Control pageControl;
        private WebView2 webView;

        public MusicApp ActualMusicApp;

        public string CurrentState { get; set; } = STATE_CHOICE_APP;

        #endregion

        #region Event Handlers

        public void WebView2_Loaded(object sender, RoutedEventArgs e)
        {
            webView = sender as WebView2;
        }

        public void PageControl_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            pageControl = sender as UserControl;
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Tag)
            {
                case "SpotifyBtn":
                    webView.Source = new Uri(SpotifyURL);
                    ActualMusicApp = MusicApp.Spotify;
                    CurrentState = STATE_SPOTIFY_APP;
                    break;
                case "DeezerBtn":
                    webView.Source = new Uri(DeezerURL);
                    ActualMusicApp = MusicApp.Deezer;
                    CurrentState = STATE_DEEZER_APP;
                    break;
                case "BackButton":
                    webView.Source = new Uri(NullURL);
                    CurrentState = STATE_CHOICE_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
            }
        }

        public void MusicAppWeb_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        #endregion
    }

    public enum MusicApp
    {
        Spotify,
        Deezer
    }
}
