
using Microsoft.UI.Xaml.Controls;

namespace KanBoard.ViewModel
{
    public class YoutubeViewModel
    {
        public string YoutubeURL = "https://youtube.com";

        public void YoutubeAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            (sender as WebView2).CoreWebView2.Settings.IsWebMessageEnabled = false;
            (sender as WebView2).CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            (sender as WebView2).CoreWebView2.Settings.IsScriptEnabled = true; // Só ative se precisar de scripts

            (sender as WebView2).CoreWebView2.Settings.AreHostObjectsAllowed = false;
        }

        public void YoutubeAppWeb_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (!args.Uri.StartsWith("https://"))
                args.Cancel = true;
        }
    }
}
