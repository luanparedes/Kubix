using Microsoft.UI.Windowing;
using Microsoft.UI;
using Windows.Graphics;
using Microsoft.UI.Xaml;
using Kubix.View;
using Kubix.Controls;
using System;
using Microsoft.UI.Xaml.Controls;
using Kubix.Services.Interfaces;

namespace Kubix.ViewModel
{
    public class ColorPickerViewModel
    {
        #region Fields & Properties

        private readonly ILogger _logger;
        private ColorPickerControl colorPicker;
        private ColorPickerWindow colorPickerWindow;
        private AppWindow appWindow;
        private WindowId windowId;
        private bool isOpen = false;

        public event EventHandler<ColorChangedEventArgs> KColorChanged;

        #endregion

        #region Constructor
        public ColorPickerViewModel(ILogger logger)
        {
            _logger = logger;
            logger.InfoLog("ColorPickerViewModel initialized.");
        }
        #endregion

        #region Event Handlers

        public void KColorPicker_Loaded(ColorPickerControl colorPicker, RoutedEventArgs e)
        {
            colorPicker.ColorChanged += ColorPicker_ColorChanged;
        }

        private void ColorPicker_KClosePicker(object sender, EventArgs e)
        {
            _logger.InfoLog("Color Picker closed.");
            colorPickerWindow.Close();
        }

        private void ColorPicker_ColorChanged(ColorPicker sender, Microsoft.UI.Xaml.Controls.ColorChangedEventArgs args)
        {
            _logger.InfoLog($"Color changed to: {args.NewColor}");
            KColorChanged?.Invoke(colorPicker, args);
        }

        public void ColorPickerWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (!isOpen)
            {
                isOpen = true;

                colorPickerWindow = sender as ColorPickerWindow;

                Initialize();
                SetWindowSize(550, 800);
                CenterWindow();
                _logger.InfoLog("Color Picker window activated and centered.");
            }
        }

        public void ColorPickerWindow_Closed(object sender, WindowEventArgs args)
        {
            _logger.InfoLog("Color Picker window closed.");
            appWindow = null;
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(colorPickerWindow);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            appWindow = AppWindow.GetFromWindowId(windowId);
            _logger.InfoLog("Color Picker window initialized.");
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
                appWindow.Move(new PointInt32(centerX + 400, centerY));
        }

        #endregion
    }
}
