using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.ViewModel
{
    public class Office365ViewModel
    {
        public readonly string OfficeURL = "https://office.com/";

        public void OfficeAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            (sender as WebView2).CoreWebView2.Settings.IsWebMessageEnabled = false;
            (sender as WebView2).CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            (sender as WebView2).CoreWebView2.Settings.IsScriptEnabled = true;

            (sender as WebView2).CoreWebView2.Settings.AreHostObjectsAllowed = false;
        }

        public void OfficeAppWeb_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (!args.Uri.StartsWith("https://"))
                args.Cancel = true;
        }
    }
}
