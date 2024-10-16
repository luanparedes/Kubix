using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage;
using Windows.UI.WebUI;

namespace KanBoard.Controls
{
    public class KBrowser : Control
    {
        #region Constants

        private const string GoogleURL = "http://google.com";

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
            (newTabItem.Content as WebView2).Source = new Uri($"{website}");
            newTabItem.Header = (newTabItem.Content as WebView2).Source.Host;
            newTabItem.CloseTab += NewTabItem_CloseTab;

            customTabView.TabItems.Add(newTabItem);
            customTabView.SelectedItem = newTabItem;
            ActualTabItem = newTabItem;
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

            if (searchButton != null)
            {
                searchButton.Click += SearchButton_Click;
            }
        }

        #endregion
    }
}
