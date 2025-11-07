using Kubix.Services.Interfaces;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;

namespace Kubix.ViewModel
{
    public class CompilersViewModel
    {
        #region Constants
        public readonly string CompilerURL = "https://www.programiz.com/csharp-programming/online-compiler/";
        #endregion

        #region Fields & Properties

        private ILogger _logger;
        private WebView2 webView;

        #endregion

        #region Constructor
        public CompilersViewModel(ILogger logger)
        {
            _logger = logger;
            _logger.InfoLog("CompilersViewModel initialized.");
        }
        #endregion

        #region Private Methods

        private async void JavascriptInjection()
        {
            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
                window.addEventListener('DOMContentLoaded', () => {
                    const el = document.querySelector('.header');
                    if (el) el.remove();

                const saleBanner = document.querySelector('#sale-top-banner');
                
                if (saleBanner) 
                    saleBanner.remove();
                });
            ");
        }

        #endregion

        #region Event Handlers

        public void WebView_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            webView = sender as WebView2;
        }

        public void AIAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            JavascriptInjection();

            webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;

            CoreWebView2Settings settings = sender.CoreWebView2.Settings;

            settings.IsWebMessageEnabled = false;
            settings.AreDefaultScriptDialogsEnabled = false;
            settings.IsScriptEnabled = true;
            settings.AreHostObjectsAllowed = false;

            _logger.InfoLog("WebView2 Compilers CoreWebView2 initialized with configuration:");
            _logger.InfoLog($"WebView2 Compilers initialized with custom settings:{settings.IsWebMessageEnabled}");
            _logger.InfoLog($"WebView2 Compilers initialized with custom settings:{settings.AreDefaultScriptDialogsEnabled}");
            _logger.InfoLog($"WebView2 Compilers initialized with custom settings:{settings.IsScriptEnabled}");
            _logger.InfoLog($"WebView2 Compilers initialized with custom settings:{settings.AreHostObjectsAllowed}");
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

        #endregion
    }
}
