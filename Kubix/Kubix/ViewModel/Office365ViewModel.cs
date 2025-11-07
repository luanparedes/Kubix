using Kubix.Services.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;

namespace Kubix.ViewModel
{
    public class Office365ViewModel
    {
        #region Constants

        public const string STATE_CHOICE_APP = "ChoiceAppState";
        public const string STATE_OFFICE_APP = "InsideDocument";

        #endregion

        #region Fields & Properties

        private readonly ILogger _logger;
        private UserControl pageControl;
        private WebView2 webView;

        public readonly string OfficeURL = "https://office.com/";

        public string CurrentState { get; set; } = STATE_CHOICE_APP;

        #endregion

        #region Constructor

        public Office365ViewModel(ILogger logger)
        {
            _logger = logger;
            _logger.InfoLog("Office365ViewModel initialized.");
        }

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

        public void OfficeAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
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

            webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }

        public void OfficeAppWeb_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (!args.Uri.StartsWith("https://"))
                args.Cancel = true;
        }

        private void CoreWebView2_NewWindowRequested(Microsoft.Web.WebView2.Core.CoreWebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs args)
        {
            args.Handled = true;
            webView.Source = new Uri(args.Uri);
            CurrentState = STATE_OFFICE_APP;
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            webView.Source = new Uri(OfficeURL);
            CurrentState = STATE_CHOICE_APP;
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        #endregion
    }
}
