using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.System;

namespace KanBoard.ViewModel
{
    public class BrowserViewModel : ObservableObject
    {
        #region Fields & Properties

        TextBox searchWebPage;
        WebView2 webView;

        private string _websiteURL = "https://www.google.com";
        public string WebsiteURL
        {
            get { return _websiteURL; }
            set { SetProperty(ref _websiteURL, value); }
        }

        #endregion


        #region Constructor

        public BrowserViewModel() { }

        #endregion

        #region Methods

        private void GoToWebsite(string website)
        {
            webView.Source = new Uri($"http://{website}");
        }

        #endregion

        #region Event Handlers

        public void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            searchWebPage = sender as TextBox;

            if (searchWebPage != null)
            {
                searchWebPage.KeyUp += SearchWebPage_KeyUp;
            }
        }

        private void SearchWebPage_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
                GoToWebsite(searchWebPage.Text);
        }

        public void WebView2_Loaded(object sender, RoutedEventArgs e)
        {
            webView = sender as WebView2;
        }

        public void SearchWebPage_Click(object sender, RoutedEventArgs e)
        {
            GoToWebsite(searchWebPage.Text);
        }

        public void WebView2_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            searchWebPage.Text = webView.Source.ToString();
        }

        #endregion
    }
}
