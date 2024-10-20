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

        #endregion

        #region Fields & Properties

        private Control pageControl;

        public MusicApp ActualMusicApp;

        public string CurrentState { get; set; } = STATE_CHOICE_APP;

        #endregion

        #region Event Handlers

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
                    ActualMusicApp = MusicApp.Spotify;
                    CurrentState = STATE_SPOTIFY_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "DeezerBtn":
                    ActualMusicApp = MusicApp.Deezer;
                    CurrentState = STATE_DEEZER_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "BackButton":
                    CurrentState = STATE_CHOICE_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
            }
        }

        #endregion
    }

    public enum MusicApp
    {
        Spotify,
        Deezer
    }
}
