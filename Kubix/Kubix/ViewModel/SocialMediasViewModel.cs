
using Kubix.Services.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
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

        private readonly ILogger _logger;
        private Control pageControl;
        private WebView2 webView;

        public SocialMediaApp ActualSocialMediaApp;

        public string CurrentState { get; set; } = STATE_CHOICE_APP;

        #endregion

        #region Constructor
        public SocialMediasViewModel(ILogger logger)
        {
            _logger = logger;
            _logger.InfoLog("SocialMediasViewModel initialized");
        }
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
                    _logger.InfoLog("Navigating to Instagram");
                    break;
                case "FacebookBtn":
                    ActualSocialMediaApp = SocialMediaApp.Facebook;
                    CurrentState = STATE_FACEBOOK_APP;
                    webView.Source = new Uri(facebookURL);
                    _logger.InfoLog("Navigating to Facebook");
                    break;
                case "XBtn":
                    ActualSocialMediaApp = SocialMediaApp.X;
                    CurrentState = STATE_X_APP;
                    webView.Source = new Uri(XURL);
                    _logger.InfoLog("Navigating to X");
                    break;
                case "BackButton":
                    CurrentState = STATE_CHOICE_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    _logger.InfoLog("Returning to social media choice");
                    break;
            }
        }

        public void SocialMedia_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        public void SocialMediaAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            CoreWebView2Settings settings = sender.CoreWebView2.Settings;

            settings.IsWebMessageEnabled = false;
            settings.AreDefaultScriptDialogsEnabled = false;
            settings.IsScriptEnabled = true;
            settings.AreHostObjectsAllowed = false;

            _logger.InfoLog("WebView2 AIView CoreWebView2 initialized with configuration:");
            _logger.InfoLog($"WebView2 AIView initialized with custom settings:{settings.IsWebMessageEnabled}");
            _logger.InfoLog($"WebView2 AIView initialized with custom settings:{settings.AreDefaultScriptDialogsEnabled}");
            _logger.InfoLog($"WebView2 AIView initialized with custom settings:{settings.IsScriptEnabled}");
            _logger.InfoLog($"WebView2 AIView initialized with custom settings:{settings.AreHostObjectsAllowed}");
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
