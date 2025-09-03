using Kubix.Helpers;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage;
using static System.Net.Mime.MediaTypeNames;

namespace Kubix.Controls
{
    public class CustomTabViewItem : TabViewItem
    {
        #region Fields & Properties

        private Button closeButton;

        public FormatTextControl FormatControl { get; set; }
        public StorageFile TabFile { get; set; }

        public event EventHandler HasChangesChanged;
        public event EventHandler<ITextSelection> SelectionTextChanged;
        public event EventHandler<Button> CloseTab;

        #endregion

        #region Constructor

        public CustomTabViewItem(CreateFileEnum file, string header = null, string text = "")
        {
            FormatControl = new FormatTextControl(file, text);

            switch (file)
            {
                case CreateFileEnum.NewFile:
                    Header = Stringer.GetString("KB_NewDocumentText");
                    break;
                case CreateFileEnum.OpenFile:
                    Header = header;
                    break;
            }

            FormatControl.InitialText = text;

            Content = FormatControl;
        }

        #endregion

        #region Event Handlers

        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (FormatControl.HasChanges)
            {
                CustomDialog dialog = new CustomDialog();
                var response = await dialog.ShowSaveDialog();

                if (response == ContentDialogResult.Primary)
                {
                    await FormatControl.SaveFile();
                }
            }

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
