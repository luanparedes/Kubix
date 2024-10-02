using Microsoft.UI.Xaml;

namespace KanBoard.Services.Interfaces
{
    public interface IThemeService
    {
        void ChangeAppTheme(ElementTheme theme);
        void LightTheme();
        void DarkTheme();
        void HighContrastTheme();
    }
}
