using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KanBoard.ViewModel
{
    public class AppMusicViewModel : ObservableObject
    {
        #region Constants

        public readonly string SpotifyURL = "http://www.spotify.com";
        public readonly string DeezerURL = "http://deezer.com";

        #endregion

        public string CurrentState { get; set; } = "ChoiceAppState";

        public MusicApp ActualMusicApp;

        Control pageControl;
        WebView2 webView;

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
                    CurrentState = "SpotifyAppState";
                    break;
                case "DeezerBtn":
                    webView.Source = new Uri(DeezerURL);
                    ActualMusicApp = MusicApp.Deezer;
                    CurrentState = "DeezerAppState";
                    break;
                case "BackButton":
                    CurrentState = "ChoiceAppState";
                    break;
            }

            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }
    }

    public enum MusicApp
    {
        Spotify,
        Deezer
    }
}
