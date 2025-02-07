using Kubix.Services.Interfaces;
using Microsoft.UI.Xaml;
using Windows.UI.ViewManagement;

namespace Kubix.Services.Classes
{
    public class ThemeService : IThemeService
    {
        public void DarkTheme()
        {
            ChangeAppTheme(ElementTheme.Dark);
        }

        public void LightTheme()
        {
            ChangeAppTheme(ElementTheme.Light);
        }

        public void DefaultTheme()
        {

            var uiSettings = new UISettings();
            var color = uiSettings.GetColorValue(UIColorType.Background);

            bool isDarkTheme = color.R < 128 && color.G < 128 && color.B < 128;

            if (isDarkTheme)
                DarkTheme();
            else
                LightTheme();
        }


        public void HighContrastTheme()
        {
            //throw new NotImplementedException();
        }

        public static ElementTheme GetSystemTheme()
        {
            var uiSettings = new UISettings();
            var color = uiSettings.GetColorValue(UIColorType.Background);

            bool isDarkTheme = color.R < 128 && color.G < 128 && color.B < 128;

            return isDarkTheme ? ElementTheme.Dark : ElementTheme.Light;
        }

        public void ChangeAppTheme(ElementTheme theme)
        {
            var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();

            dispatcherQueue.TryEnqueue(

                Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                () =>
                {
                    App.Instance.ChangeTheme(theme);
                });
        }
    }
}
