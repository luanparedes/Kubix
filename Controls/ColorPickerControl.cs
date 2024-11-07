using Microsoft.UI.Xaml.Controls;
using System;

namespace Kubix.Controls
{
    public class ColorPickerControl : ColorPicker
    {
        private Button OkButton;
        public event EventHandler KClosePicker;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            OkButton = GetTemplateChild("OkButton") as Button;

            if (OkButton != null )
            {
                OkButton.Click += OkButton_Click;
            }
        }

        private void OkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            KClosePicker?.Invoke(OkButton, new EventArgs());
        }
    }
}
