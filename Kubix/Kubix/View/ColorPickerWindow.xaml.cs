
using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.Controls;
using Kubix.ViewModel;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using WinRT.Interop;

namespace Kubix.View
{
    public sealed partial class ColorPickerWindow : Window
    {
        public ColorPickerViewModel ViewModel { get; set; }

        public ColorPickerWindow()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<ColorPickerViewModel>();

            this.Activated += ColorPickerWindow_Activated;
            this.Closed += ColorPickerWindow_Closed;
        }

        private void ColorPickerWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            ViewModel.ColorPickerWindow_Activated(sender, args);
        }

        private void ColorPickerWindow_Closed(object sender, WindowEventArgs args)
        {
            ViewModel.ColorPickerWindow_Closed(sender, args);
        }

        private void KColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ColorPickerControl colorPicker)
            {
                ViewModel.KColorPicker_Loaded(colorPicker, e);

                IntPtr hWnd = WindowNative.GetWindowHandle(this);
                WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
                AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

                appWindow.Resize(new Windows.Graphics.SizeInt32((int)colorPicker.ActualWidth, (int)colorPicker.ActualHeight - 200));

                var presenter = appWindow.Presenter as OverlappedPresenter;

                presenter.IsResizable = false;
                presenter.IsMaximizable = false;
                presenter.SetBorderAndTitleBar(false, false);
            }
        }
    }
}
