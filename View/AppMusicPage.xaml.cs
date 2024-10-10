using Microsoft.UI.Xaml.Controls;
using KanBoard.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace KanBoard.View
{
    public sealed partial class AppMusicPage : Page
    {
        AppMusicViewModel ViewModel { get; }

        public AppMusicPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<AppMusicViewModel>();
        }
    }
}
