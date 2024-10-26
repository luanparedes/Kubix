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

        public CustomTabViewItem(CreateFileEnum file, string header = null, string text = "")
        {
            formatControl = new FormatTextControl(file, text);

            //formatControl.EditBox.Style = (Style)App.Current.Resources["RichEditTextBox"];
            //formatControl.EditBox.SelectionFlyout = null;

            switch (file)
            {
                case CreateFileEnum.NewFile:
                    //formatControl.EditBox.Document.GetText(TextGetOptions.None, out text);
                    Header = Stringer.GetString("KB_NewDocumentText");
                    break;
                case CreateFileEnum.OpenFile:
                    //formatControl.EditBox.Document.SetText(TextSetOptions.None, text);
                    Header = header;
                    break;
            }

            //formatControl.EditBox.TextChanged += CustomTabViewItem_TextChanged;
            //formatControl.EditBox.SelectionChanged += CustomTabViewItem_SelectionChanged;
            formatControl.InitialText = text;

            Content = formatControl;
        }

        #endregion

        #region Event Handlers

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
            
            if (closeButton != null)
                closeButton.Click += CloseButton_Click;
        }

        #endregion
    }
}
