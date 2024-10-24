using Microsoft.UI.Windowing;
using Microsoft.UI;
using Windows.Graphics;
using Microsoft.UI.Xaml;
using KanBoard.View;
using KanBoard.Controls;
using System;
using Microsoft.UI.Xaml.Controls;

namespace KanBoard.ViewModel
{
    public class ColorPickerViewModel
    {
        #region Fields & Properties

        ColorPickerControl colorPicker;
        ColorPickerWindow colorPickerWindow;
        AppWindow appWindow;
        WindowId windowId;

        public event EventHandler<ColorChangedEventArgs> KColorChanged;

        #endregion

        #region Event Handlers

        public void KColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            colorPicker = sender as ColorPickerControl;
            colorPicker.ColorChanged += ColorPicker_ColorChanged;
        }

        private void ColorPicker_ColorChanged(ColorPicker sender, Microsoft.UI.Xaml.Controls.ColorChangedEventArgs args)
        {
            KColorChanged?.Invoke(colorPicker, args);
        }

        public void ColorPickerWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            colorPickerWindow = sender as ColorPickerWindow;

            Initialize();
            //CustomizeWindow();
            SetWindowSize(550, 800);
            CenterWindow();
        }

        public void ColorPickerWindow_Closed(object sender, WindowEventArgs args)
        {
            appWindow = null;
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(colorPickerWindow);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            appWindow = AppWindow.GetFromWindowId(windowId);

        }

        private void SetWindowSize(int width, int height)
        {
            if (appWindow != null)
                appWindow.Resize(new SizeInt32(width, height));
        }

        public void CenterWindow()
        {
            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            var centerX = (displayArea.WorkArea.Width - appWindow.Size.Width) / 2;
            var centerY = (displayArea.WorkArea.Height - appWindow.Size.Height) / 2;

            if (appWindow != null)
                appWindow.Move(new PointInt32(centerX, centerY));
        }

        private void CustomizeWindow()
        {
            if (appWindow != null)
            {
                var presenter = appWindow.Presenter as OverlappedPresenter;

                if (presenter != null)
                {
                    presenter.IsResizable = false;        // Impede redimensionamento
                    presenter.IsMinimizable = false;      // Remove botão de minimizar
                    presenter.IsMaximizable = false;      // Remove botão de maximizar
                    presenter.SetBorderAndTitleBar(false, false); // Remove borda e barra de título
                }
            }
        }

        #endregion
    }
}
