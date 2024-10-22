using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using Windows.Storage;

namespace KanBoard.Controls
{
    public class CustomTabViewItem : TabViewItem
    {
        #region Fields & Properties

        public bool HasChanges => InitialText != string.Empty;

        private Button closeButton;
        public string InitialText {  get; set; }
        public StorageFile TabFile { get; set; }

        public event EventHandler HasChangesChanged;
        public event EventHandler<Button> CloseTab;

        #endregion

        #region Constructor

        public CustomTabViewItem()
        {
            Content = new RichEditBox();
            (Content as RichEditBox).Style = (Style)App.Current.Resources["RichEditTextBox"];

            //(Content as RichEditBox).TextChanged += CustomTabViewItem_TextChanged;
            
            Header = "New file";
            //InitialText = (Content as RichEditBox).Text;
        }

        public CustomTabViewItem(string header, string text)
        {
            Content = new RichEditBox();
            (Content as RichEditBox).Style = (Style)App.Current.Resources["RichEditTextBox"];
            (Content as RichEditBox).Document.SetText(Microsoft.UI.Text.TextSetOptions.None, text);
            //TODO Change to RichEditBox
            //(Content as RichEditBox).TextChanged += CustomTabViewItem_TextChanged;

            Header = header;
            string teste = text;
            (Content as RichEditBox).Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out text);
        }

        #endregion

        #region Event Handlers

        private void CustomTabViewItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            HasChangesChanged?.Invoke(this, null);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseTab?.Invoke(this, closeButton);
        }

        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            closeButton = GetTemplateChild("CloseButton") as Button;
            closeButton.Click += CloseButton_Click;
        }

        #endregion
    }
}
