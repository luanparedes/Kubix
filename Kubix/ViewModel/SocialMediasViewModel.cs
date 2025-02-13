
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Kubix.ViewModel
{
    public class SocialMediasViewModel
    {
        #region Constants

        public const string STATE_CHOICE_APP = "ChoiceAppState";
        public const string STATE_INSTAGRAM_APP = "InstagramAppState";
        public const string STATE_FACEBOOK_APP = "FacebookAppState";
        public const string STATE_X_APP = "XAppState";

        public readonly string InstagramURL = "https://www.instagram.com/";
        public readonly string facebookURL = "https://www.facebook.com/";
        public readonly string XURL = "https://x.com/";

        #endregion

        #region Fields & Properties

        private Control pageControl;
        private WebView2 webView;

        public SocialMediaApp ActualSocialMediaApp;

        public string CurrentState { get; set; } = STATE_CHOICE_APP;

        #endregion

        #region Event Handlers

        public void PageControl_Loaded(object sender, RoutedEventArgs e)
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
                case "InstagramBtn":
                    ActualSocialMediaApp = SocialMediaApp.Instagram;
                    CurrentState = STATE_INSTAGRAM_APP;
                    webView.Source = new Uri(InstagramURL);
                    break;
                case "FacebookBtn":
                    ActualSocialMediaApp = SocialMediaApp.Facebook;
                    CurrentState = STATE_FACEBOOK_APP;
                    webView.Source = new Uri(facebookURL);
                    break;
                case "XBtn":
                    ActualSocialMediaApp = SocialMediaApp.X;
                    CurrentState = STATE_X_APP;
                    webView.Source = new Uri(XURL);
                    break;
                case "BackButton":
                    CurrentState = STATE_CHOICE_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
            }
        }

        public void SocialMedia_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        public void SocialMediaAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            webView.CoreWebView2.Settings.IsWebMessageEnabled = false;
            webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            webView.CoreWebView2.Settings.IsScriptEnabled = true; // Só ative se precisar de scripts

            webView.CoreWebView2.Settings.AreHostObjectsAllowed = false;
        }

        public void SocialMediaAppWeb_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (!args.Uri.StartsWith("https://"))
                args.Cancel = true;   
        }

        #endregion
    }

    public enum SocialMediaApp
    {
        Instagram,
        Facebook,
        X
    }
}
