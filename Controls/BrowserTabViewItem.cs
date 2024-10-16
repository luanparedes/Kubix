using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace KanBoard.Controls
{
    public class BrowserTabViewItem : TabViewItem
    {
        #region Fields & Properties

        private Button closeButton;

        public event EventHandler<Button> CloseTab;

        #endregion

        #region Constructor

        public BrowserTabViewItem()
        {
            Content = new WebView2();            
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

            if (closeButton != null )
            {
                closeButton.Click += CloseButton_Click;
            }
        }

        #endregion
    }
}
