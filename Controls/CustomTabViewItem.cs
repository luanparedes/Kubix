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

            switch (file)
            {
                case CreateFileEnum.NewFile:
                    Header = Stringer.GetString("KB_NewDocumentText");
                    break;
                case CreateFileEnum.OpenFile:
                    Header = header;
                    break;
            }

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
