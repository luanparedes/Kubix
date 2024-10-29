using Microsoft.UI.Xaml;

namespace Kubix.Services.Interfaces
{
    public interface IThemeService
    {
        void ChangeAppTheme(ElementTheme theme);
        void LightTheme();
        void DarkTheme();
        void HighContrastTheme();
    }
}
