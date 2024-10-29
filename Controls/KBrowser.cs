using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using Windows.Globalization;
using Windows.UI.WebUI;

namespace KanBoard.Controls
{
    public class KBrowser : Control
    {
        #region Constants

        private const string GoogleURL = "https://google.com";

        #endregion

        #region Fields & Properties

        private TabView customTabView;
        private TextBox searchTextBox;
        private Button searchButton;

        BrowserTabViewItem ActualTabItem {  get; set; }

        #endregion

        #region Constructor

        public KBrowser() 
        {
            
        }

        #endregion

        #region Methods

        private void CreateTab(string website)
        {
            BrowserTabViewItem newTabItem = new BrowserTabViewItem();
            (newTabItem.Content as WebView2).NavigationCompleted += KBrowser_NavigationCompleted;
            (newTabItem.Content as WebView2).CoreWebView2Initialized += KBrowser_CoreWebView2Initialized;
            (newTabItem.Content as WebView2).NavigationStarting += KBrowser_NavigationStarting;

            (newTabItem.Content as WebView2).Source = new Uri($"{website}");
            newTabItem.Header = (newTabItem.Content as WebView2).Source.Host;
            newTabItem.CloseTab += NewTabItem_CloseTab;

            customTabView.TabItems.Add(newTabItem);
            customTabView.SelectedItem = newTabItem;
            ActualTabItem = newTabItem;
        }

        private async void KBrowser_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            string currentLanguage = ApplicationLanguages.Languages.FirstOrDefault() ?? "en-US";
            await (ActualTabItem.Content as WebView2).CoreWebView2.ExecuteScriptAsync($"document.documentElement.lang = '{currentLanguage}';");
        }

        public void KBrowser_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            (sender as WebView2).CoreWebView2.Settings.IsWebMessageEnabled = false;
            (sender as WebView2).CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            (sender as WebView2).CoreWebView2.Settings.IsScriptEnabled = true;

            (sender as WebView2).CoreWebView2.Settings.AreHostObjectsAllowed = false;
        }

        public void KBrowser_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (!args.Uri.StartsWith("https://"))
                args.Cancel = true;
        }

        private void GoToWebsite(string website)
        {
            (ActualTabItem.Content as WebView2).Source = new Uri($"http://{website}");
            ActualTabItem.Header = (ActualTabItem.Content as WebView2).Source.Host;
        }

        #endregion

        #region Event Handlers

        private void CustomTabView_AddTabButtonClick(TabView sender, object args)
        {
            CreateTab(GoogleURL);
        }

        private void NewTabItem_CloseTab(object sender, Button e)
        {
            BrowserTabViewItem tabItem = sender as BrowserTabViewItem;

            customTabView.TabItems.Remove(tabItem);

            if (customTabView.TabItems.Count == 0)
                CreateTab(GoogleURL);
        }

        private void CustomTabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (customTabView.SelectedItem != null)
            {
                ActualTabItem = customTabView.SelectedItem as BrowserTabViewItem;
                searchTextBox.Text = (ActualTabItem.Content as WebView2).Source.Host;
            }
        }

        private void SearchButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            GoToWebsite(searchTextBox.Text);
        }

        private void SearchTextBox_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                GoToWebsite(searchTextBox.Text);
            }
        }

        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            customTabView = GetTemplateChild("TabViewCustom") as TabView;
            searchTextBox = GetTemplateChild("TextBoxCustom") as TextBox;
            searchButton = GetTemplateChild("SearchPageButton") as Button;

            if (customTabView != null)
            {
                customTabView.AddTabButtonClick += CustomTabView_AddTabButtonClick;
                customTabView.SelectionChanged += CustomTabView_SelectionChanged;

                CreateTab(GoogleURL);
            }

            if (searchTextBox != null)
            {
                searchTextBox.KeyUp += SearchTextBox_KeyUp;
            }

            if (searchButton != null)
            {
                searchButton.Click += SearchButton_Click;
            }
        }

        #endregion
    }
}
