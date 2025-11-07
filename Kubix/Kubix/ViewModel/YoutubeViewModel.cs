
using CommunityToolkit.Mvvm.ComponentModel;
using Kubix.Services.Interfaces;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

namespace Kubix.ViewModel
{
    public partial class YoutubeViewModel : ObservableObject
    {
        #region Fields & Properties
        private readonly ILogger _logger;
        public string YoutubeURL = "https://youtube.com";
        private WebView2 _webView;

        [ObservableProperty]
        private bool isBackEnabled = false;
        [ObservableProperty]
        private bool isForwardEnabled = false;
        #endregion

        #region Constructor
        public YoutubeViewModel(ILogger logger)
        {
            _logger = logger;
            _logger.InfoLog("YoutubeViewModel initialized.");
        }
        #endregion

        #region Event Handlers
        public void YoutubeAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
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
            _webView.CoreWebView2.SourceChanged += CoreWebView2_SourceChanged;
        }

        public void YoutubeAppWeb_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (!args.Uri.StartsWith("https://"))
                args.Cancel = true;
        }

        public void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (_webView.CanGoBack)
            {
                _logger.InfoLog("Navigating back in WebView2 history.");
                _webView.GoBack();         
            }
        }

        public void ForwardButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (_webView.CanGoForward)
            {
                _logger.InfoLog("Navigating forward in WebView2 history.");
                _webView.GoForward();
            }
        }

        private void CoreWebView2_SourceChanged(Microsoft.Web.WebView2.Core.CoreWebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs args)
        {
            IsBackEnabled = _webView.CanGoBack;
            IsForwardEnabled = _webView.CanGoForward;
            _logger.InfoLog($"WebView2 navigate to {sender.Source}");
        }

        #endregion
    }
}
