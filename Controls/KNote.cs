using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage;
using Microsoft.UI.Input;
using Windows.System;
using Windows.UI.Core;
using Kubix.Helpers;

namespace Kubix.Controls
{
    public class KNote : Control
    {
        #region Fields & Properties

        private TabView customTabView;
        private Button openButton;
        private Button saveButton;

        private CustomTabViewItem ActualTabItem { get; set; }

        #endregion

        #region Constructor

        public KNote()
        {
            KeyUp += KNote_KeyUp;
            Loaded += KNote_Loaded;
        }

        private void KNote_Loaded(object sender, RoutedEventArgs e)
        {
            CreateTab();
        }

        #endregion

        #region Methods

        private async void CreateTab(StorageFile file = null)
        {
            CustomTabViewItem newTabItem;

            if (file != null)
            {
                newTabItem = new(CreateFileEnum.OpenFile, file.Name, await FileIO.ReadTextAsync(file));
                newTabItem.TabFile = file;
            }
            else
                newTabItem = new(CreateFileEnum.NewFile);

            newTabItem.HasChangesChanged += CustomTabView_IsTextChanged;
            newTabItem.CloseTab += NewTabItem_CloseTab;
            newTabItem.formatControl.KTextChanged += TabContent_TextChanged;

            customTabView.TabItems.Add(newTabItem);
            customTabView.SelectedItem = newTabItem;
        }

        private void TabContent_TextChanged(object sender, string e)
        {
            saveButton.IsEnabled = ActualTabItem.formatControl.HasChanges;
        }

        private async void OpenFile()
        {
            StorageFile file = await ActualTabItem.formatControl.OpenFile();
            CreateTab(file);           
        }

        private void SaveFile()
        {
            ActualTabItem.Header = ActualTabItem.formatControl.SaveFile();
            saveButton.IsEnabled = ActualTabItem.formatControl.HasChanges;
        }

        #endregion

        #region Event Handlers

        private void CustomTabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (customTabView.SelectedItem != null)
            {
                ActualTabItem = customTabView.SelectedItem as CustomTabViewItem;
                saveButton.IsEnabled = ActualTabItem.formatControl.HasChanges;
            }
        }

        private void CustomTabView_AddTabButtonClick(TabView sender, object args)
        {
            CreateTab();
        }

        private void CustomTabView_IsTextChanged(object sender, EventArgs e)
        {
            saveButton.IsEnabled = ActualTabItem.formatControl.HasChanges;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void NewTabItem_CloseTab(object sender, Button e)
        {
            CustomTabViewItem tabItem = sender as CustomTabViewItem;

            customTabView.TabItems.Remove(tabItem);

            if (customTabView.TabItems.Count == 0)
                CreateTab();
        }

        private void KNote_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            bool isCtrlPressed = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);

            if (isCtrlPressed && e.Key == VirtualKey.S && ActualTabItem.formatControl.HasChanges)
            {
                SaveFile();
            }
        }

        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            customTabView = GetTemplateChild("TabViewCustom") as TabView;
            openButton = GetTemplateChild("OpenButton") as Button;
            saveButton = GetTemplateChild("SaveButton") as Button;

            if (customTabView != null)
            {
                customTabView.SelectionChanged += CustomTabView_SelectionChanged;
                customTabView.AddTabButtonClick += CustomTabView_AddTabButtonClick;
            }

            if (openButton != null)
            {
                openButton.Click += OpenButton_Click;
            }

            if (saveButton != null)
            {
                saveButton.Click += SaveButton_Click;
                saveButton.IsEnabled = false;
            }
        }

        #endregion
    }
}
