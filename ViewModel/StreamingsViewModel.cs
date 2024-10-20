using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanBoard.ViewModel
{
    public class StreamingsViewModel
    {
        #region Constants

        public const string STATE_CHOICE_APP = "StateChoiceApp";
        public const string STATE_NETFLIX = "StateNetflix";
        public const string STATE_MAX = "StateMax";
        public const string STATE_DISNEY = "StateDisney";
        public const string STATE_PRIME_VIDEO = "StatePrimeVideo";

        public readonly string NetflixURL = "http://netflix.com";
        public readonly string MaxURL = "http://max.com";
        public readonly string DisneyURL = "http://disneyplus.com";
        public readonly string PrimeVideoURL = "http://primevideo.com";

        #endregion

        #region Fields & Properties

        private Control pageControl;

        StreamingApp ActualStreamingApp;

        private string CurrentState { get; set; } = STATE_CHOICE_APP;

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
                case "NetflixBtn":
                    ActualStreamingApp = StreamingApp.Netflix;
                    CurrentState = STATE_NETFLIX;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "MaxBtn":
                    ActualStreamingApp = StreamingApp.Max;
                    CurrentState = STATE_MAX;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "DisneyBtn":
                    ActualStreamingApp = StreamingApp.Disney;
                    CurrentState = STATE_DISNEY;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "PrimeVideoBtn":
                    ActualStreamingApp = StreamingApp.PrimeVideo;
                    CurrentState = STATE_PRIME_VIDEO;
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

    public enum StreamingApp
    {
        Netflix,
        Max,
        Disney,
        PrimeVideo
    }
}
