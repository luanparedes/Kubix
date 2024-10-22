using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using Windows.Storage;
using Microsoft.UI.Input;
using Windows.System;
using Windows.UI.Core;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Text;
using System.IO;
using KanBoard.Helpers;

namespace KanBoard.Controls
{
    public class KNote : Control
    {
        #region Fields & Properties

        private TabView customTabView; // TODO sorking open and save but save IsEnabled stop working
        private Button openButton;     // and events.
        private Button saveButton;
        private FormatTextControl formatTextControl;

        private string Text {  get; set; }
        private CustomTabViewItem ActualTabItem { get; set; }

        #endregion

        #region Constructor

        public KNote()
        {
            KeyUp += KNote_KeyUp;
        }

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
            openPicker.FileTypeFilter.Add(".rtf");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Instance.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                CreateTab(file);

                switch (file.FileType)
                {
                    case ".txt":
                        string text = await Windows.Storage.FileIO.ReadTextAsync(file);
                        (ActualTabItem.Content as RichEditBox).Document.SetText(TextSetOptions.None, text);
                        break;

                    case ".rtf":
                        using (var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                        {
                            (ActualTabItem.Content as RichEditBox).Document.LoadFromStream(TextSetOptions.FormatRtf, stream);
                        }
                        break;
                }
            }
        }

        private async void SaveFile()
        {
            string text = string.Empty;

            if (ActualTabItem.TabFile != null)
            {
                (ActualTabItem.Content as RichEditBox).Document.GetText(TextGetOptions.FormatRtf, out text);
                await FileIO.WriteTextAsync(ActualTabItem.TabFile, text);
            }
            else
            {
                var savePicker = new FileSavePicker();

                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("Text File", new List<string>() { ".txt", ".rtf" });
                savePicker.SuggestedFileName = Stringer.GetString("KB_NewDocumentText");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Instance.MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

                ActualTabItem.TabFile = await savePicker.PickSaveFileAsync();

                switch (ActualTabItem.TabFile.FileType)
                {
                    case ".txt":
                        (ActualTabItem.Content as RichEditBox).Document.GetText(TextGetOptions.None, out text);
                        break;
                    case ".rtf":
                        (ActualTabItem.Content as RichEditBox).Document.GetText(TextGetOptions.FormatRtf, out text);
                        break;
                }

                await FileIO.WriteTextAsync(ActualTabItem.TabFile, text);
            }

            if (ActualTabItem.TabFile == null)
                return;

            ActualTabItem.InitialText = (ActualTabItem.Content as RichEditBox).Document.ToString();
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

        private void KNote_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            bool isCtrlPressed = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);

            if (isCtrlPressed && e.Key == VirtualKey.S && ActualTabItem.HasChanges)
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
            formatTextControl = GetTemplateChild("FormatControl") as FormatTextControl;

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

            if(formatTextControl != null)
            {
                formatTextControl.KFamilyFontChanged += FormatTextControl_KFamilyFontChanged;
            }
        }

        private void FormatTextControl_KFamilyFontChanged(object sender, FontFamily e)
        {
            (ActualTabItem.Content as RichEditBox).FontFamily = e;
        }

        #endregion
    }
}
