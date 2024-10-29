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

        public readonly string SpotifyURL = "https://www.spotify.com";
        public readonly string DeezerURL = "https://www.deezer.com";

        #endregion

        #region Fields & Properties

        private Control pageControl;
        private WebView2 webView;

        public MusicApp ActualMusicApp;

        public string CurrentState { get; set; } = STATE_CHOICE_APP;

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
                case "SpotifyBtn":
                    ActualMusicApp = MusicApp.Spotify;
                    CurrentState = STATE_SPOTIFY_APP;
                    webView.Source = new Uri(SpotifyURL);
                    break;
                case "DeezerBtn":
                    ActualMusicApp = MusicApp.Deezer;
                    CurrentState = STATE_DEEZER_APP;
                    webView.Source = new Uri(DeezerURL);
                    break;
                case "BackButton":
                    CurrentState = STATE_CHOICE_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
            }
        }

        public void GoogleAppWeb_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        public void AppMusicAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            webView.CoreWebView2.Settings.IsWebMessageEnabled = false;
            webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            webView.CoreWebView2.Settings.IsScriptEnabled = true; // Só ative se precisar de scripts

            webView.CoreWebView2.Settings.AreHostObjectsAllowed = false;
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
        Spotify,
        Deezer
    }
}
