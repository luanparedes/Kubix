using KanBoard.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;


namespace KanBoard.ViewModel
{
    public class GoogleViewModel
    {
        #region Constants

        public const string STATE_CHOICE_APP = "ChoiceAppState";
        public const string STATE_GOOGLE_APP = "GoogleAppState";
        public const string STATE_GMAIL_APP = "GMailAppState";
        public const string STATE_PHOTOS_APP = "PhotosAppState";
        public const string STATE_MAPS_APP = "MapsAppState";
        public const string STATE_TRANSLATE_APP = "TranslateAppState";

        public string GoogleLoginURL => $"https://accounts.google.com/ServiceLogin?hl={ApplicationLanguages.Languages.FirstOrDefault() ?? "en-US"}&passive=true&continue=https://www.google.com/&ec=GAZAmgQ";
        public readonly string GoogleURL = "https://google.com/";
        public readonly string GMailURL = "https://mail.google.com/";
        public readonly string PhotosURL = "https://photos.google.com/";
        public readonly string MapsURL = "https://google.com/maps/";
        public readonly string TranslateURL = "https://translate.google.com/";

        #endregion

        #region Fields & Properties

        private Control pageControl;
        private WebView2 webView;

        public GoogleApp ActualGoogleApp;

        public string CurrentState { get; set; } = STATE_CHOICE_APP;

        #endregion

        #region Event Handlers

        public void PageControl_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            pageControl = sender as UserControl;
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        public void WebView_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            webView = sender as WebView2;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Tag)
            {
                case "GoogleBtn":
                    ActualGoogleApp = GoogleApp.Google;
                    CurrentState = STATE_GOOGLE_APP;
                    webView.Source = new Uri(GoogleURL);
                    break;
                case "GMailBtn":
                    ActualGoogleApp = GoogleApp.GMail;
                    CurrentState = STATE_GMAIL_APP;
                    webView.Source = new Uri(GMailURL);
                    break;
                case "PhotosBtn":
                    ActualGoogleApp = GoogleApp.Photos;
                    CurrentState = STATE_PHOTOS_APP;
                    webView.Source = new Uri(PhotosURL);
                    break;
                case "MapsBtn":
                    ActualGoogleApp = GoogleApp.Maps;
                    CurrentState = STATE_MAPS_APP;
                    webView.Source = new Uri(MapsURL);
                    break;
                case "TranslateBtn":
                    ActualGoogleApp = GoogleApp.Translate;
                    CurrentState = STATE_TRANSLATE_APP;
                    webView.Source = new Uri(TranslateURL);
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

        public void GoogleAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            webView.CoreWebView2.Settings.IsWebMessageEnabled = false;
            webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            webView.CoreWebView2.Settings.IsScriptEnabled = true;

            webView.CoreWebView2.Settings.AreHostObjectsAllowed = false;
        }

        public void GoogleAppWeb_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (!args.Uri.StartsWith("https://"))
                args.Cancel = true;
        }

        #endregion
    }

    public enum GoogleApp
    {
        Google,
        GMail,
        Photos,
        Maps,
        Translate
    }
}
