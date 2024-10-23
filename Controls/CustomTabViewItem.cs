using KanBoard.Helpers;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage;
using static System.Net.Mime.MediaTypeNames;

namespace KanBoard.Controls
{
    public class CustomTabViewItem : TabViewItem
    {
        #region Fields & Properties

        public FormatTextControl formatControl;
        private Button closeButton;

        public StorageFile TabFile { get; set; }

        public event EventHandler HasChangesChanged;
        public event EventHandler<ITextSelection> SelectionTextChanged;
        public event EventHandler<Button> CloseTab;

        #endregion

        #region Constructor

        public CustomTabViewItem(CreateFileEnum file, string header = null, string text = null)
        {
            formatControl = new FormatTextControl();
            formatControl.EditBox.Style = (Style)App.Current.Resources["RichEditTextBox"];
            formatControl.EditBox.SelectionFlyout = null;

            switch (file)
            {
                case CreateFileEnum.NewFile:
                    formatControl.EditBox.Document.GetText(TextGetOptions.None, out text);
                    Header = Stringer.GetString("KB_NewDocumentText");
                    break;
                case CreateFileEnum.OpenFile:
                    formatControl.EditBox.Document.SetText(TextSetOptions.None, text);
                    Header = header;
                    break;
            }

            formatControl.EditBox.TextChanged += CustomTabViewItem_TextChanged;
            formatControl.EditBox.SelectionChanged += CustomTabViewItem_SelectionChanged;
            formatControl.InitialText = text;

            Content = formatControl.EditBox;
        }

        #endregion


        #region Event Handlers

        private void CustomTabViewItem_TextChanged(object sender, RoutedEventArgs e)
        {
            HasChangesChanged?.Invoke(this, null);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseTab?.Invoke(this, closeButton);
        }

        private void CustomTabViewItem_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = formatControl.EditBox.Document.Selection;
            SelectionTextChanged?.Invoke(formatControl.EditBox, selectedText);
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
