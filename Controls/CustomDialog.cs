using KanBoard.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace KanBoard.Controls
{
    public class CustomDialog : Control
    {
        public async Task<ContentDialogResult> ShowSaveDialog()
        {
            string title = Stringer.GetString("KB_SaveCloseTitleDialog");
            string content = Stringer.GetString("KB_SaveCloseContentDialog");
            string primaryBtn = Stringer.GetString("KB_SaveClosePrimaryDialog");
            string secondaryBtn = Stringer.GetString("KB_SaveCloseSecondaryDialog");

            ContentDialog dialog = new ContentDialog();
            dialog.Title = title;
            dialog.Content = content;
            dialog.PrimaryButtonText = primaryBtn;
            dialog.SecondaryButtonText = secondaryBtn;
            dialog.XamlRoot = App.Instance.MainWindow.Content.XamlRoot;

            ContentDialogResult response = await dialog.ShowAsync();

            return response;
        }
    }
}
