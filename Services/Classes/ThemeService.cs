using KanBoard.Services.Interfaces;
using Microsoft.UI.Xaml;

namespace KanBoard.Services.Classes
{
    public class ThemeService : IThemeService
    {
        public void DarkTheme()
        {
            ChangeAppTheme(ElementTheme.Dark);
        }

        public void HighContrastTheme()
        {
            //throw new NotImplementedException();
        }

        public void LightTheme()
        {
            ChangeAppTheme(ElementTheme.Light);
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
