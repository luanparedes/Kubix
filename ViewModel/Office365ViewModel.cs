using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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

        private UserControl pageControl;
        private WebView2 webView;

        public readonly string OfficeURL = "https://office.com/";

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

        public void OfficeAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            webView.CoreWebView2.Settings.IsWebMessageEnabled = false;
            webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            webView.CoreWebView2.Settings.IsScriptEnabled = true;
            webView.CoreWebView2.Settings.AreHostObjectsAllowed = false;
            
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
