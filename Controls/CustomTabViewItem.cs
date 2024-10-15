using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using Windows.Storage;

namespace KanBoard.Controls
{
    public class CustomTabViewItem : TabViewItem
    {
        private Button closeButton;
        public string InitialText {  get; set; }
        public StorageFile TabFile { get; set; }
        public bool HasChanges => InitialText != (Content as TextBox).Text;

        public event EventHandler HasChangesChanged;
        public event EventHandler<Button> CloseTab;

        public CustomTabViewItem()
        {
            Content = new TextBox();
            (Content as TextBox).Style = (Style)App.Current.Resources["NotepadTextBox"];

            (Content as TextBox).TextChanged += CustomTabViewItem_TextChanged;
            
            Header = "New file";
            InitialText = (Content as TextBox).Text;
        }

        public CustomTabViewItem(string header, string text)
        {
            Content = new TextBox();
            (Content as TextBox).Style = (Style)App.Current.Resources["NotepadTextBox"];
            (Content as TextBox).Text = text;

            (Content as TextBox).TextChanged += CustomTabViewItem_TextChanged;

            Header = header;
            InitialText = (Content as TextBox).Text;
        }

        public bool HasTextChanged()
        {
            return InitialText != (Content as TextBox).Text;
        }

        private void CustomTabViewItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            HasChangesChanged?.Invoke(this, null);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            closeButton = GetTemplateChild("CloseButton") as Button;
            closeButton.Click += CloseButton_Click;

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseTab?.Invoke(this, closeButton);
        }
    }
}
