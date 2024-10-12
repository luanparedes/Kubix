
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace KanBoard.ViewModel
{
    public class NotepadViewModel : ObservableObject
    {
        #region Fields & Properties

        private string NotepadText {  get; set; }
        private StorageFile CurrentFile { get; set; }

        private bool _hasTextChange = false;
        public bool HasTextChange
        {
            get { return _hasTextChange; }
            set { SetProperty(ref _hasTextChange, value); }
        }

        #endregion

        #region Methods

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
                CurrentFile = file;
                NotepadText = await FileIO.ReadTextAsync(file);
            }
        }

        private async void SaveFile()
        {
            var savePicker = new FileSavePicker();

            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Text File", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "New Document";

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Instance.MainWindow);  // Obtém o handle da janela principal
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

            StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                await FileIO.WriteTextAsync(file, NotepadText);
            }
        }

        #endregion

        #region Event Handlers

        public void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NotepadText = (sender as TextBox).Text;
            HasTextChange = true;
        }

        public void NotepadControl_NoteSelectionChange(object sender, SelectionChangedEventArgs e)
        {

        }

        public void NotepadControl_NoteTextChanged(object sender, TabViewItem e)
        {
            NotepadText = (sender as TextBox).Text;
            HasTextChange = true;
        }

        #endregion
    }
}
