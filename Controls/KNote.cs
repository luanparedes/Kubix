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
using KanBoard.Helpers;
using Windows.UI;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace KanBoard.Controls
{
    public class KNote : Control
    {
        #region Fields & Properties

        private TabView customTabView; // TODO sorking open and save but save IsEnabled stop working
        private Button openButton;     // and events.
        private Button saveButton;
        private FormatTextControl formatTextControl;

        private string Text { get; set; }
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
                newTabItem = new(CreateFileEnum.OpenFile, file.Name, await FileIO.ReadTextAsync(file));
                newTabItem.TabFile = file;
            }
            else
                newTabItem = new(CreateFileEnum.NewFile);

            newTabItem.HasChangesChanged += CustomTabView_IsTextChanged;
            newTabItem.CloseTab += NewTabItem_CloseTab;
            newTabItem.formatControl.EditBox.TextChanged += TabContent_TextChanged;
            newTabItem.formatControl.EditBox.SelectionChanged += EditBox_SelectionChanged;

            customTabView.TabItems.Add(newTabItem);
            customTabView.SelectedItem = newTabItem;
        }

        private void EditBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedText = ActualTabItem.formatControl.EditBox.Document.Selection;
            //TODO pegar o texto atual e confirmar se já está.
        }

        private void TabContent_TextChanged(object sender, RoutedEventArgs e)
        {
            saveButton.IsEnabled = ActualTabItem.formatControl.HasChanges;
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
                switch (ActualTabItem.TabFile.FileType)
                {
                    case ".txt":
                        ActualTabItem.formatControl.EditBox.Document.GetText(TextGetOptions.None, out text);
                        break;
                    case ".rtf":
                        ActualTabItem.formatControl.EditBox.Document.GetText(TextGetOptions.FormatRtf, out text);
                        break;
                }

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

                if (ActualTabItem.TabFile != null)
                {
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
            }

            if (ActualTabItem.TabFile == null)
                return;

            ActualTabItem.formatControl.InitialText = (ActualTabItem.Content as RichEditBox).Document.ToString();
            ActualTabItem.Header = ActualTabItem.TabFile.Name;
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

        private void FormatTextControl_KFamilyFontChanged(object sender, FontFamily e)
        {
            var selectedText = (ActualTabItem.Content as RichEditBox).Document.Selection;

            if (!selectedText.Equals(""))
            {
                if (!string.IsNullOrEmpty(e.Source))
                {
                    selectedText.CharacterFormat.Name = e.Source;
                }
            }
        }

        private void FormatTextControl_KFontSizeChanged(object sender, int e)
        {
            var selectedText = (ActualTabItem.Content as RichEditBox).Document.Selection;

            if (!selectedText.Equals(""))
                selectedText.CharacterFormat.Size = e;
        }

        private void FormatTextControl_KForegroundChanged(object sender, Color e)
        {
            var selectedText = (ActualTabItem.Content as RichEditBox).Document.Selection;

            if (!selectedText.Equals(""))
            {
                selectedText.CharacterFormat.ForegroundColor = e;
            }
        }

        private void FormatTextControl_KBoldChanged(object sender, ToggleButton e)
        {
            var selectedText = (ActualTabItem.Content as RichEditBox).Document.Selection;

            if (!selectedText.Equals(""))
            {
                var isBold = selectedText.CharacterFormat.Bold == FormatEffect.On;
                e.IsChecked = isBold;
                selectedText.CharacterFormat.Bold = !isBold ? FormatEffect.On : FormatEffect.Off;
            }
        }
        private void FormatTextControl_KItalicChanged(object sender, bool e)
        {
            var selectedText = (ActualTabItem.Content as RichEditBox).Document.Selection;

            if (!selectedText.Equals(""))
            {
                selectedText.CharacterFormat.Italic = e ? FormatEffect.On : FormatEffect.Off;
            }
        }

        private void FormatTextControl_KUnderlineChanged(object sender, bool e)
        {
            var selectedText = (ActualTabItem.Content as RichEditBox).Document.Selection;

            if (!selectedText.Equals(""))
            {
                selectedText.CharacterFormat.Underline = e ? UnderlineType.Single : UnderlineType.None;
            }
        }

        private void FormatTextControl_KStrikethroughChanged(object sender, bool e)
        {
            var selectedText = (ActualTabItem.Content as RichEditBox).Document.Selection;

            if (!selectedText.Equals(""))
            {
                selectedText.CharacterFormat.Strikethrough = e ? FormatEffect.On : FormatEffect.Off;
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

            if (formatTextControl != null)
            {
                formatTextControl.KFamilyFontChanged += FormatTextControl_KFamilyFontChanged;
                formatTextControl.KFontSizeChanged += FormatTextControl_KFontSizeChanged;
                formatTextControl.KForegroundChanged += FormatTextControl_KForegroundChanged;
                formatTextControl.KBoldChanged += FormatTextControl_KBoldChanged;
                formatTextControl.KItalicChanged += FormatTextControl_KItalicChanged;
                formatTextControl.KUnderlineChanged += FormatTextControl_KUnderlineChanged;
                formatTextControl.KStrikethroughChanged += FormatTextControl_KStrikethroughChanged;
            }
        }

        #endregion
    }
}
