using Microsoft.UI.Windowing;
using Microsoft.UI;
using Windows.Graphics;
using Microsoft.UI.Xaml;
using KanBoard.View;

namespace KanBoard.ViewModel
{
    public class ColorPickerViewModel
    {
        #region Fields & Properties

        ColorPickerWindow colorPickerWindow;
        AppWindow appWindow;
        WindowId windowId;

        #endregion

        #region Event Handlers

        public void ColorPickerWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            colorPickerWindow = sender as ColorPickerWindow;

            Initialize();
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

        #endregion
    }
}
