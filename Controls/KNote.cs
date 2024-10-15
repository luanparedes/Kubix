using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using Windows.Storage;
using static System.Net.Mime.MediaTypeNames;

namespace KanBoard.Controls
{
    class KNote : Control
    {
        #region Fields & Properties

        private TabView customTabView;
        private Button openButton;
        private Button saveButton;

        private CustomTabViewItem ActualTabItem { get; set; }

        #endregion

        #region Methods

        private async void CreateTab(StorageFile file = null)
        {
            CustomTabViewItem newTabItem;

            if (file != null)
            {
                newTabItem = new(file.Name, await FileIO.ReadTextAsync(file));
                newTabItem.TabFile = file;
            }
            else
                newTabItem = new();

            newTabItem.HasChangesChanged += CustomTabView_IsTextChanged;
            newTabItem.CloseTab += NewTabItem_CloseTab;

            customTabView.TabItems.Add(newTabItem);
            customTabView.SelectedItem = newTabItem;
        }

        private async void OpenFile()
        {
            var openPicker = new FileOpenPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            openPicker.FileTypeFilter.Add(".txt");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Instance.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                CreateTab(file);
            }
        }

        private async void SaveFile()
        {
            if (ActualTabItem.TabFile != null)
            {
                await FileIO.WriteTextAsync(ActualTabItem.TabFile, (ActualTabItem.Content as TextBox).Text);
            }
            else
            {
                var savePicker = new FileSavePicker();

                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("Text File", new List<string>() { ".txt" });
                savePicker.SuggestedFileName = "New Document";

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Instance.MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

                ActualTabItem.TabFile = await savePicker.PickSaveFileAsync();
            }

            ActualTabItem.InitialText = (ActualTabItem.Content as TextBox).Text;
            ActualTabItem.Header = ActualTabItem.TabFile.Name;
            saveButton.IsEnabled = ActualTabItem.HasChanges;
        }

        #endregion

        #region Event Handlers

        private void CustomTabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(customTabView.SelectedItem != null) 
            {
                ActualTabItem = customTabView.SelectedItem as CustomTabViewItem;
                saveButton.IsEnabled = ActualTabItem.HasChanges;
            }
        }

        private void CustomTabView_AddTabButtonClick(TabView sender, object args)
        {
            CreateTab();
        }

        private void CustomTabView_IsTextChanged(object sender, EventArgs e)
        {
            saveButton.IsEnabled = ActualTabItem.HasChanges;
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

                CreateTab();
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
