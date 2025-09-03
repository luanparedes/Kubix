using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage;
using Microsoft.UI.Input;
using Windows.System;
using Windows.UI.Core;
using Kubix.Helpers;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Kubix.Controls
{
    public class KNote : Control
    {
        #region Fields & Properties

        private TabView customTabView;
        private Button openButton;
        private Button saveButton;
        private ColorPickerControl colorPickerControl;

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
                newTabItem.FormatControl.TabFile = file;
            }
            else
            {
                newTabItem = new(CreateFileEnum.NewFile);
                newTabItem.FormatControl.FileType = ".txt";
            }

            ActivateEvents(newTabItem);

            customTabView.TabItems.Add(newTabItem);
            customTabView.SelectedItem = newTabItem;

            if (file != null)
            {
                switch (file.FileType)
                {
                    case ".txt":
                        string text = await FileIO.ReadTextAsync(file);
                        ActualTabItem.FormatControl.EditBox.Document.SetText(TextSetOptions.None, text);
                        ActualTabItem.FormatControl.FileType = ".txt";
                        ActualTabItem.FormatControl.HasChanges = false;
                        break;

                    case ".rtf":
                        using (var stream = await file.OpenAsync(FileAccessMode.Read))
                        {
                            ActualTabItem.FormatControl.EditBox.Document.LoadFromStream(TextSetOptions.FormatRtf, stream);
                            ActualTabItem.FormatControl.FileType = ".rtf";
                            ActualTabItem.FormatControl.HasChanges = false;
                        }
                        break;
                }
            }
        }

        private void ActivateEvents(CustomTabViewItem newTabItem)
        {
            newTabItem.CloseTab += NewTabItem_CloseTab;
            newTabItem.FormatControl.KColorPickerOpenChanged += FormatControl_KColorPickerOpenChanged;
            newTabItem.FormatControl.HasChanged += FormatControl_HasChanged;
        }

        private async void OpenFile()
        {
            StorageFile file = await ActualTabItem.FormatControl.OpenFile();

            if (file != null)
                CreateTab(file);
        }

        private async void SaveFile()
        {
            StorageFile file = await ActualTabItem.FormatControl.SaveFile();
            ActualTabItem.Header = file?.Name;
            string text = await FileIO.ReadTextAsync(file);
            string actualText;
            ActualTabItem.FormatControl.EditBox.Document.GetText(TextGetOptions.None, out actualText);
            ActualTabItem.FormatControl.InitialText = text;
            ActualTabItem.FormatControl.HasChanges = false;
            saveButton.IsEnabled = ActualTabItem.FormatControl.HasChanges;
        }

        #endregion

        #region Event Handlers

        private void FormatControl_KColorPickerOpenChanged(object sender, bool isChecked)
        {
            if (sender is ToggleButton button)
            {
                colorPickerControl.Visibility = isChecked ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void ColorPickerControl_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            ActualTabItem.FormatControl.KForeground = new SolidColorBrush(args.NewColor);
        }

        private void CustomTabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (customTabView.SelectedItem != null)
            {
                ActualTabItem = customTabView.SelectedItem as CustomTabViewItem;
                saveButton.IsEnabled = ActualTabItem.FormatControl.HasChanges;
            }
        }

        private void CustomTabView_AddTabButtonClick(TabView sender, object args)
        {
            CreateTab();
        }

        private void FormatControl_HasChanged(object sender, bool hasChange)
        {
            saveButton.IsEnabled = hasChange;
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

            if (isCtrlPressed && e.Key == VirtualKey.S && ActualTabItem.FormatControl.HasChanges)
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
            colorPickerControl = GetTemplateChild("ColorPickerControlName") as ColorPickerControl;

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

            if (colorPickerControl != null)
            {
                colorPickerControl.ColorChanged += ColorPickerControl_ColorChanged;
                colorPickerControl.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}
