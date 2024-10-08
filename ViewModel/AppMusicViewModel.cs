using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KanBoard.ViewModel
{
    public class AppMusicViewModel : ObservableObject
    {       
        public readonly string SpotifyURL = "http://www.spotify.com";
        public readonly string DeezerURL = "http://deezer.com";

        public MusicApp ActualMusicApp;

        StackPanel stackPanel;
        WebView2 webView;
        Button spotifyButtonBar;
        Button deezerButtonBar;

        public void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            stackPanel = sender as StackPanel;
        }

        public void WebView2_Loaded(object sender, RoutedEventArgs e)
        {
            webView = sender as WebView2;
        }

        public void SpotifyButtonBar_Loaded(object sender, RoutedEventArgs e)
        {
            spotifyButtonBar = sender as Button;
        }

        public void DeezerButtonBar_Loaded(object sender, RoutedEventArgs e)
        {
            deezerButtonBar = sender as Button;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Tag)
            {
                case "SpotifyBtn":
                    webView.Source = new Uri(SpotifyURL);
                    ActualMusicApp = MusicApp.Spotify;
                    break;
                case "DeezerBtn":
                    webView.Source = new Uri(DeezerURL);
                    ActualMusicApp = MusicApp.Deezer;
                    break;
            }

            ChangePageState();
        }

        private void ChangePageState()
        {
            stackPanel.Visibility = Visibility.Collapsed;
            webView.Visibility = Visibility.Visible;

            if (ActualMusicApp == MusicApp.Spotify)
            {
                spotifyButtonBar.Visibility = Visibility.Collapsed;
                deezerButtonBar.Visibility = Visibility.Visible;
            }
            else
            {
                spotifyButtonBar.Visibility = Visibility.Visible;
                deezerButtonBar.Visibility = Visibility.Collapsed;
            }
                
        }
    }

    public enum MusicApp
    {
        Spotify,
        Deezer
    }
}
