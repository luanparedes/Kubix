using Microsoft.UI.Xaml.Controls;
using System;

namespace Kubix.ViewModel
{
    public class CompilersViewModel
    {
        public readonly string CompilerURL = "https://www.programiz.com/csharp-programming/online-compiler/";

        private WebView2 webView;

        public void WebView_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            webView = sender as WebView2;
        }

        public void AIAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            webView.CoreWebView2.Settings.IsWebMessageEnabled = false;
            webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            webView.CoreWebView2.Settings.IsScriptEnabled = true;

            webView.CoreWebView2.Settings.AreHostObjectsAllowed = false;

            webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;

        }

        public void AIAppWeb_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (!args.Uri.StartsWith("https://"))
                args.Cancel = true;
        }

        private void CoreWebView2_NewWindowRequested(Microsoft.Web.WebView2.Core.CoreWebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs args)
        {
            args.Handled = true;
            webView.Source = new Uri(args.Uri);
        }
    }
}
