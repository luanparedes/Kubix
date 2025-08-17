
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace Kubix.ViewModel
{
    public partial class YoutubeViewModel : ObservableObject
    {
        public string YoutubeURL = "https://youtube.com";
        private WebView2 _webView;

        [ObservableProperty]
        private bool isBackEnabled = false;
        [ObservableProperty]
        private bool isForwardEnabled = false;

        public void YoutubeAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            _webView = sender as WebView2;

            _webView.CoreWebView2.Settings.IsWebMessageEnabled = false;
            _webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            _webView.CoreWebView2.Settings.IsScriptEnabled = true; // Só ative se precisar de scripts
            _webView.CoreWebView2.Settings.AreHostObjectsAllowed = false;
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
                _webView.GoBack();         
            }
        }

        public void ForwardButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (_webView.CanGoForward)
            {
                _webView.GoForward();
            }
        }

        private void CoreWebView2_SourceChanged(Microsoft.Web.WebView2.Core.CoreWebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs args)
        {
            IsBackEnabled = _webView.CanGoBack;
            IsForwardEnabled = _webView.CanGoForward;
        }
    }
}
