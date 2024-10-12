using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace KanBoard.Controls
{
    class NotepadControl : Control
    {
        #region Fields & Properties

        private TabView customTabView;

        public event EventHandler<SelectionChangedEventArgs> NoteSelectionChange;
        public event EventHandler<TabViewItem> NoteTextChanged;

        #endregion

        #region Methods

        private TabViewItem AddNewtab()
        {
            TextBox textBox = new TextBox();
            textBox.Style = (Style)App.Current.Resources["NotepadTextBox"];
            textBox.TextChanged += TextBox_TextChanged;

            TabViewItem tabItem = new TabViewItem();
            tabItem.Header = "New";
            tabItem.Content = textBox;

            return tabItem;
        }

        #endregion

        #region Event Handlers

        private void CustomTabView_AddTabButtonClick(TabView sender, object args)
        {
            customTabView.TabItems.Add(AddNewtab());
        }

        private void CustomTabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NoteSelectionChange?.Invoke(this, e);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TabViewItem tabItem = (sender as TextBox).Parent as TabViewItem;
            NoteTextChanged?.Invoke(sender, tabItem);
        }

        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            customTabView = GetTemplateChild("TabViewCustom") as TabView;

            if (customTabView != null)
            {
                customTabView.SelectionChanged += CustomTabView_SelectionChanged;
                customTabView.AddTabButtonClick += CustomTabView_AddTabButtonClick;

                customTabView.TabItems.Add(AddNewtab());
                customTabView.SelectedIndex = 0;
            }
        }

        #endregion
    }
}
