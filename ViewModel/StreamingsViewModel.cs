using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.ViewModel
{
    public class StreamingsViewModel
    {
        #region Constants

        public const string STATE_CHOICE_APP = "StateChoiceApp";
        public const string STATE_NETFLIX = "StateNetflix";
        public const string STATE_MAX = "StateMax";
        public const string STATE_DISNEY = "StateDisney";
        public const string STATE_PRIME_VIDEO = "StatePrimeVideo";

        public readonly string NetflixURL = "https://netflix.com";
        public readonly string MaxURL = "https://max.com";
        public readonly string DisneyURL = "https://disneyplus.com";
        public readonly string PrimeVideoURL = "https://primevideo.com";

        #endregion

        #region Fields & Properties

        private Control pageControl;
        private WebView2 webView;

        StreamingApp ActualStreamingApp;

        private string CurrentState { get; set; } = STATE_CHOICE_APP;

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
                case "NetflixBtn":
                    ActualStreamingApp = StreamingApp.Netflix;
                    CurrentState = STATE_NETFLIX;
                    webView.Source = new Uri(NetflixURL);
                    break;
                case "MaxBtn":
                    ActualStreamingApp = StreamingApp.Max;
                    CurrentState = STATE_MAX;
                    webView.Source = new Uri(MaxURL);
                    break;
                case "DisneyBtn":
                    ActualStreamingApp = StreamingApp.Disney;
                    CurrentState = STATE_DISNEY;
                    webView.Source = new Uri(DisneyURL);
                    break;
                case "PrimeVideoBtn":
                    ActualStreamingApp = StreamingApp.PrimeVideo;
                    CurrentState = STATE_PRIME_VIDEO;
                    webView.Source = new Uri(PrimeVideoURL);
                    break;
                case "BackButton":
                    CurrentState = STATE_CHOICE_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
            }
        }

        public void StreamAppWeb_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        public void StreamingsAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            webView.CoreWebView2.Settings.IsWebMessageEnabled = false;
            webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            webView.CoreWebView2.Settings.IsScriptEnabled = true; // Só ative se precisar de scripts

            webView.CoreWebView2.Settings.AreHostObjectsAllowed = false;
        }

        public void StreamingsAppWeb_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (!args.Uri.StartsWith("https://"))
                args.Cancel = true;
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
