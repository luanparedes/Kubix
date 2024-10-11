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
        public readonly string NullURL = "http://null.com";

        #endregion

        #region Fields & Properties

        private Control pageControl;
        private WebView2 webView;

        StreamingApp ActualStreamingApp;

        private string CurrentState { get; set; } = STATE_CHOICE_APP;

        #endregion

        #region Constructor

        public StreamingsViewModel()
        {

        }

        #endregion

        #region Event Handlers

        public void PageControl_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            pageControl = sender as UserControl;
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        public void WebView2_Loaded(object sender, RoutedEventArgs e)
        {
            webView = sender as WebView2;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Tag)
            {
                case "NetflixBtn":
                    webView.Source = new Uri(NetflixURL);
                    ActualStreamingApp = StreamingApp.Netflix;
                    CurrentState = STATE_NETFLIX;
                    break;
                case "MaxBtn":
                    webView.Source = new Uri(MaxURL);
                    ActualStreamingApp = StreamingApp.Max;
                    CurrentState = STATE_MAX;
                    break;
                case "DisneyBtn":
                    webView.Source = new Uri(DisneyURL);
                    ActualStreamingApp = StreamingApp.Disney;
                    CurrentState = STATE_DISNEY;
                    break;
                case "PrimeVideoBtn":
                    webView.Source = new Uri(PrimeVideoURL);
                    ActualStreamingApp = StreamingApp.PrimeVideo;
                    CurrentState = STATE_PRIME_VIDEO;
                    break;
                case "BackButton":
                    webView.Source = new Uri(NullURL);
                    CurrentState = STATE_CHOICE_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
            }
        }

        public void StreamAppWeb_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            VisualStateManager.GoToState(pageControl, CurrentState, true);
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
